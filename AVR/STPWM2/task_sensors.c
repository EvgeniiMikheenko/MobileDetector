//==============================================================================
//
//==============================================================================

#include "task_sensors.h"

#include <app_config.h>
#include <app_types.h>

#include <spr010003_mcu_slave.h>

#include <Filters\median_filter.h>
#include <Filters\digital_filter.h>

#define CHECK_ADC_FREQUENCY

#define USE_LGAR_FLOW_SENSOR2

//------------------------------------------------------------------------------
typedef 	float (*ConvertRaw)( uint32_t value );

void ADC_IRQHandler( void );

float RawToVoltage( uint32_t raw );
float TempSensor_RawToValue( uint32_t raw );
float Flow_RawToValue( uint32_t raw );

uint32_t FilterTemp1Sensor( uint32_t value );
uint32_t FilterTemp2Sensor( uint32_t value );
uint32_t FilterFlow1Sensor( uint32_t value );
uint32_t FilterFlow2Sensor( uint32_t value );

void update_sensors_values( void );
float SetSensorData( uint32_t id, ConvertRaw lpConvertPtr , AdcChannelData_t *lpData, bool isUseF1, bool isUseF2, float k );

void init_adc_data( void );

extern void SetSensorsUpdateFlag( void );
//------------------------------------------------------------------------------
extern SlaveMcuDataPacket_t 			m_dataPacket;
extern SlaveMcuControlPacket_t 			m_ctrlPacket;
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
volatile	SensorsInfo_t				m_sensors[ADC_CHANNELS_COUNT];
			

#define 	MFILTER_SIZE				11			
			PairU16_t					m_medFiltersBufs[ADC_CHANNELS_COUNT][MFILTER_SIZE];
			MedianFltrU16Data_t			m_medFiltersData[ADC_CHANNELS_COUNT];
			
#ifdef CHECK_ADC_FREQUENCY
			uint32_t 					m_adcClock = 0;
#endif // CHECK_ADC_FREQUENCY

#define		ADC_OVERSAMPLING_BITS				3  /* 3 */
#define		ADC_OVERSAMPLING_COUNTER_MAX		64 /* 64 */
			uint32_t					m_adcRawBuf[ADC_CHANNELS_COUNT];
			bool 						m_isAdcInit = false;
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Interrupts
void ADC_IRQHandler( void ) {
	//mcu_slave_set_led4( true );
	
	static int ovs_counter = 0;
	static int chNum = 0;
	static uint8_t chMask = 0;
	uint8_t mask;
	static int measureCnt[ ADC_CHANNELS_COUNT ];
	
	if( !m_isAdcInit ) {
		for(int i = 0; i < ADC_CHANNELS_COUNT; i++) {
			measureCnt[i] = 0;
		}
		ovs_counter = 0;
		chNum = 0;
		chMask = 0;
		m_isAdcInit = true;
	}
	
	uint32_t tmp = MDR_ADC->ADC1_RESULT;
	
	uint32_t data = tmp & 0x0FFF;
	chNum = ((tmp >> 16) & 0x0FFF);
	int index = -1;
	
	switch( chNum ) {
	case 3:
		index = 0;
		break;
	case 4:
		index = 1;
		break;
	case 5:
		index = 2;
		break;
	case 7:
		index = 3;
		break;
	default:
		index = -1;
		break;
	}
	
	if( index >= 0 ) {
		mask = ( 1 << index );
		if( (chMask & ( mask )) == 0 ) {
			measureCnt[ index ]++;
			m_adcRawBuf[index] += data;
			chMask |= mask;
			ovs_counter++;
		}
		
		if( chMask == 0x0F )
			chMask = 0;
		
	}
	
	if( ovs_counter >= ( ADC_OVERSAMPLING_COUNTER_MAX * ADC_CHANNELS_COUNT ) ) {
		//mcu_slave_set_led4( true );
		//
		ovs_counter = 0;
		for(int i = 0; i < ADC_CHANNELS_COUNT; i++) {
			tmp = m_adcRawBuf[i] >> ADC_OVERSAMPLING_BITS;
			m_adcRawBuf[i] = 0;
			m_sensors[i].rawValue = tmp;
			if( m_ctrlPacket.chConfigs[ i ].Bits.isUseFilter1 == 1 ) {
				u16_med_fltr_add( &m_medFiltersData[i], tmp );
			}
			//
			m_sensors[i].valid = ((tmp >= m_sensors[i].rawMin) && (tmp <= m_sensors[i].rawMax));
			
			measureCnt[ i ] = 0;
		}
		//
		SetSensorsUpdateFlag();
	}
	/* Clear ADC1 OUT_OF_RANGE interrupt bit */
	MDR_ADC->ADC1_STATUS |= (ADCx_IT_END_OF_CONVERSION | ADCx_IT_OUT_OF_RANGE);
}

//------------------------------------------------------------------------------
// Private

void update_sensors_values( void ) {
	int chNum;
	AdcChannelData_t *lpChConfig;
	bool f1En, f2En;
	//
	chNum = ADC_TEMP1_SENSOR_NUM;
	lpChConfig = &m_dataPacket.adcChsData[ chNum ];
	f1En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter1 == 1;
	f2En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter2 == 1;
	SetSensorData( chNum, TempSensor_RawToValue, lpChConfig, f1En, f2En, 1.0 );
	//
	chNum = ADC_TEMP2_SENSOR_NUM;
	lpChConfig = &m_dataPacket.adcChsData[ chNum ];
	f1En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter1 == 1;
	f2En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter2 == 1;
	SetSensorData( chNum, TempSensor_RawToValue, lpChConfig, f1En, f2En, 1.0 );
	//
	chNum = ADC_FLOW_TOTAL_NUM;
	lpChConfig = &m_dataPacket.adcChsData[ chNum ];
	f1En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter1 == 1;
	f2En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter2 == 1;
	SetSensorData( chNum, Flow_RawToValue, lpChConfig, f1En, f2En, 2.0 );
	//
	chNum = ADC_FLOW_IN_NUM;
	lpChConfig = &m_dataPacket.adcChsData[ chNum ];
	f1En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter1 == 1;
	f2En = m_ctrlPacket.chConfigs[ chNum ].Bits.isUseFilter2 == 1;
	SetSensorData( chNum, Flow_RawToValue, lpChConfig, f1En, f2En, 2.0 );
	
	for( int i = 0; i < ADC_CHANNELS_COUNT; i++ ) {
		if( m_sensors[ i ].valid )
			m_dataPacket.adcChsData[ i ].flags |= ADC_CH_FLAG_CH_VALID;
		else
			m_dataPacket.adcChsData[ i ].flags &= ~ADC_CH_FLAG_CH_VALID;
		
		m_dataPacket.sensorsValue[ i ] = m_sensors[ i ].value;
	}
}

float SetSensorData( uint32_t id, ConvertRaw lpConvertPtr , AdcChannelData_t *lpData, bool isUseF1, bool isUseF2, float k ) {
	uint32_t dValue;
	float value;
	
	if( (lpConvertPtr == NULL) || (lpData == NULL ) )
		return -1;
	
	lpData->raw = m_sensors[ id ].rawValue;
	
	if( m_sensors[id].valid ) {
		/* Первый фильтр */
		if( isUseF1 ) {
			dValue = m_medFiltersData[id].value;
		}
		else {
			dValue = m_sensors[ id ].rawValue;
		}
		
		// Сохраняем данные после первого фильтра
		lpData->f1 = dValue;
		
		if(m_sensors[id].lpFilterFunc != NULL) {
			/* Второй фильтр */
			if( isUseF2 )
				dValue = ( m_sensors[id].lpFilterFunc )( dValue );
		}
		
		// Сохраняем данные после второго фильтра
		lpData->f2 = dValue;
		lpData->voltage = RawToVoltage( dValue ) * k;
		/* Пересчитываем в единицы измерения */
		value = lpConvertPtr( dValue );
		m_sensors[id].value = value;
		return value;
	}
	//
	
	// Данные канала не верны
	m_sensors[id].value = 0;
	lpData->f1 = 0;
	lpData->f1 = 0;
	lpData->voltage = RawToVoltage( lpData->raw ) * k;
	return 0;
}

float RawToVoltage( uint32_t raw ) {
	return (BOARD_VREF * raw) / ((1 << (12 + ADC_OVERSAMPLING_BITS )) - 1);
}

float TempSensor_RawToValue( uint32_t raw )  {
	float voltage = RawToVoltage(raw);
	if(voltage == 0.0)
		voltage = 0.000000001;
	float Rt = ((BOARD_VREF * 1100) / voltage) - 1100;
	float T = ((Rt / 1000) - 1) / 0.00385;
	return T;
}

uint32_t FilterTemp1Sensor( uint32_t value ) {
	return value; // uint16_dfilter_add_ret( &m_IntAdcDFilterData0, value );
}

uint32_t FilterTemp2Sensor( uint32_t value ) {
	return value;//uint16_dfilter_add_ret( &m_IntAdcDFilterData1, value );
}

uint32_t FilterFlow1Sensor( uint32_t value ) {
	static uint16_t Ad = 25;
	static uint16_t Bd = 7;
	static uint16_t Kd = 5;
	static uint16_t y = 0;
	
	if( !m_isAdcInit )
		y = 0;
	
	y = (uint16_t)(((y * Ad) + ((value) * Bd)) >> Kd);
	
	return y; // uint16_dfilter_add_ret( &m_IntAdcDFilterData2, y );
}

uint32_t FilterFlow2Sensor( uint32_t value ) {
	static uint16_t Ad = 25;
	static uint16_t Bd = 7;
	static uint16_t Kd = 5;
	static uint16_t y = 0;
	
	if( !m_isAdcInit )
		y = 0;
	
	y = (uint16_t)(((y * Ad) + ((value) * Bd)) >> Kd);
	
	return y; // uint16_dfilter_add_ret( &m_IntAdcDFilterData3, y );
}

#ifdef USE_LGAR_FLOW_SENSOR2

float Flow_RawToValue( uint32_t raw ) {
	float voltage = RawToVoltage(raw);
	voltage *= 2; //
	float v2 = voltage * voltage;
	//float v3 = v2 * voltage;
	//
	// Датчик потока самодельный 1 :)
	// |   0   |  100  |  200  |  300  |  400  |  500  |  600  |  700  |  800  | поток см3
	// | 2.910 | 3.321 | 3.555 | 3.773 | 3.896 | 3.968 | 4.028 | 4.121 | 4.182 | показпния V (t=35)
	//float y = ( 502.7067 * v3 ) - ( 4797.4712 * v2 ) + ( 15490.5359 * voltage ) - 16839.6938;
	// Датчик потока самодельный 1 :)
	// |   0   |  200  |  600  |  поток см3
	// | 2.10  | 2.40  | 2.68  |  показпния V (t=35)
	//float y = ( 1313.6289 * v2 ) - ( 5244.6634 * voltage ) + 5220.6897;
	float y = (( 1313.6289 * v2 ) - ( 5244.6634 * voltage ) + 5220.6897) / 2;
	return y;
}

#else
	#error "Преобразование значения АЦП в единицы потока не реализовано"
#endif // USE_LGAR_FLOW_SENSOR2

float GetTemp( void ) {
	
	float t1 = m_sensors[ADC_TEMP1_SENSOR_NUM].value;
	float t2 = m_sensors[ADC_TEMP2_SENSOR_NUM].value;
	
	if( ( m_sensors[ADC_TEMP1_SENSOR_NUM].valid ) && ( m_sensors[ADC_TEMP2_SENSOR_NUM].valid ))
		return ( t1 + t2 ) / 2;
	
	if( m_sensors[ADC_TEMP1_SENSOR_NUM].valid )
		return t1;
	
	if( m_sensors[ADC_TEMP2_SENSOR_NUM].valid )
		return t2;
	
	return 1000;
}

float GetFlow( void ) {
	if( m_sensors[ADC_FLOW_TOTAL_NUM].valid )
		return m_sensors[ADC_FLOW_TOTAL_NUM].value;
	
	return 0;
}
//------------------------------------------------------------------------------
// Init

void init_adc( void ) {
	NVIC_DisableIRQ( ADC_IRQn );
	m_isAdcInit = false;
	init_adc_data();
	
	ADC_InitTypeDef sADC;
	ADCx_InitTypeDef sADCx;
	
	ADC_DeInit();
	// Inti clock ADC  
	RST_CLK_ADCclkSelection( RST_CLK_ADCclkHSI_C1 );//(RST_CLK_ADCclkLSE);
	RST_CLK_ADCclkPrescaler( RST_CLK_ADCclkDIV128 );
	RST_CLK_HSIclkPrescaler( RST_CLK_HSIclkDIV1 );
	// Enable clock ADC
	RST_CLK_ADCclkEnable(ENABLE);
	RST_CLK_PCLKcmd(RST_CLK_PCLK_ADC, ENABLE);
	ADC_StructInit(&sADC);
	//
	sADC.ADC_StartDelay = 15;
	sADC.ADC_TempSensor = ADC_TEMP_SENSOR_Disable;
	//sADC.ADC_TempSensorAmplifier = ADC_TEMP_SENSOR_AMPLIFIER_Enable;
	//sADC.ADC_TempSensorConversion = ADC_TEMP_SENSOR_CONVERSION_Enable;
	sADC.ADC_IntVRefConversion = ADC_VREF_CONVERSION_Disable;
	sADC.ADC_IntVRefTrimming = 1;
	ADC_Init(&sADC);
	
	/* ADC1 Configuration */
	ADCx_StructInit(&sADCx);
	sADCx.ADC_ClockSource = ADC_CLOCK_SOURCE_ADC;
	sADCx.ADC_SamplingMode = ADC_SAMPLING_MODE_CICLIC_CONV;
	sADCx.ADC_ChannelSwitching = ADC_CH_SWITCHING_Enable;
	sADCx.ADC_ChannelNumber = ADC_CH_ADC3;
	sADCx.ADC_Channels = (1 << ADC_CH_ADC7) | (1 << ADC_CH_ADC5) | (1 << ADC_CH_ADC4) | (1 << ADC_CH_ADC3); // 
	sADCx.ADC_LevelControl = ADC_LEVEL_CONTROL_Disable;
	sADCx.ADC_LowLevel = 0;
	sADCx.ADC_HighLevel = 0;
	sADCx.ADC_VRefSource = ADC_VREF_SOURCE_EXTERNAL;
	sADCx.ADC_Prescaler = ADC_CLK_div_512;
	sADCx.ADC_DelayGo = 7;
	ADC1_Init(&sADCx);
	//
	/* Enable ADC1 EOCIF and AWOIFEN interupts */
	ADC1_ITConfig(ADCx_IT_END_OF_CONVERSION, ENABLE);
	/* Enable ADC IRQ */
	NVIC_SetPriority(ADC_IRQn, INTERRUPT_PRIORITY_INT_ADC);
	NVIC_EnableIRQ(ADC_IRQn);
	//
#ifdef CHECK_ADC_FREQUENCY
	RST_CLK_FreqTypeDef clocks;
	RST_CLK_GetClocksFreq( &clocks );
	m_adcClock = clocks.ADC_CLK_Frequency;
#endif // CHECK_ADC_FREQUENCY
	/* ADC1 enable */
	ADC1_Cmd(ENABLE);

	NVIC_SetPriority( ADC_IRQn, INTERRUPT_PRIORITY_INT_ADC );
	NVIC_EnableIRQ( ADC_IRQn );
}

void init_adc_data( void ) {
	// RTD 1000
	//  150 C = 1573.270 ohm
	// -50  C = 803.113  ohm
	//          ____       ____
	// +3.0V --|____|--*--|____|-- GND
	//        RTD 1000     1.1k
	// Чем больше измеренное значение - тем меньше температура
	// -50 С => U = ( 3.0 * 1100) / (RTD + 1100) = 1.734
	//          raw = (1.734 * 0xFFF) / 3 = 2366
	// rawMax = 2366, т.к. разрешаем измерение отрицательной температуры ( до -50 )
	// 
	// 150 С => U = ( 3.0 * 1100) / (RTD + 1100) = 1.234
	//         raw = (1.234 * 0xFFF) / 3 = 1685
	//
	m_sensors[ADC_TEMP1_SENSOR_NUM].value = 0;
	m_sensors[ADC_TEMP1_SENSOR_NUM].rawValue = 0;
	m_sensors[ADC_TEMP1_SENSOR_NUM].rawMin = (uint32_t)( (1.234 * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 1685;
	m_sensors[ADC_TEMP1_SENSOR_NUM].rawMax = (uint32_t)( (1.734 * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 2366;
	m_sensors[ADC_TEMP1_SENSOR_NUM].valid = false;
	m_sensors[ADC_TEMP1_SENSOR_NUM].id = ADC_TEMP1_SENSOR_NUM;
	m_sensors[ADC_TEMP1_SENSOR_NUM].error = 0;
	m_sensors[ADC_TEMP1_SENSOR_NUM].lpFilterFunc = FilterTemp1Sensor;
	
	m_sensors[ADC_TEMP2_SENSOR_NUM].value = 0;
	m_sensors[ADC_TEMP2_SENSOR_NUM].rawValue = 0;
	m_sensors[ADC_TEMP2_SENSOR_NUM].rawMin = (uint32_t)( (1.234 * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 1685;
	m_sensors[ADC_TEMP2_SENSOR_NUM].rawMax = (uint32_t)( (1.734 * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 2366;
	m_sensors[ADC_TEMP2_SENSOR_NUM].valid = false;
	m_sensors[ADC_TEMP2_SENSOR_NUM].id = ADC_TEMP2_SENSOR_NUM;
	m_sensors[ADC_TEMP2_SENSOR_NUM].error = 0;
	m_sensors[ADC_TEMP2_SENSOR_NUM].lpFilterFunc = FilterTemp2Sensor;
	
	m_sensors[ADC_FLOW1_SENSOR_NUM].value = 0;
	m_sensors[ADC_FLOW1_SENSOR_NUM].rawValue = 0;
#ifdef USE_LGAR_FLOW_SENSOR2
	m_sensors[ADC_FLOW1_SENSOR_NUM].rawMin = (uint32_t)( ((2.3 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 1911; // (2.8 / 2) V  => ( x = ( (2.8 / 2) * 0xFFF) / 3 )
	// !!! Специально завышаю максимальное значение
	m_sensors[ADC_FLOW1_SENSOR_NUM].rawMax = (uint32_t)( ((7.3 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 2934; // (4.30 / 2) V  => ( x = ( (4.30 / 2) * 0xFFF) / 3 )
#else
	m_sensors[ADC_FLOW1_SENSOR_NUM].rawMin = (uint32_t)( ((0.5 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 200; //341 - 140;  // (0.50 / 2) V  => ( x = ( (0.50 / 2) * 0xFFF) / 3 )
	m_sensors[ADC_FLOW1_SENSOR_NUM].rawMax = (uint32_t)( ((2.6 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 1774; // (2.60 / 2) V  => ( x = ( (2.60 / 2) * 0xFFF) / 3 )
#endif // USE_LGAR_FLOW_SENSOR2
	m_sensors[ADC_FLOW1_SENSOR_NUM].valid = false;
	m_sensors[ADC_FLOW1_SENSOR_NUM].id = ADC_FLOW1_SENSOR_NUM;
	m_sensors[ADC_FLOW1_SENSOR_NUM].error = 0;
	m_sensors[ADC_FLOW1_SENSOR_NUM].lpFilterFunc = FilterFlow1Sensor;
	
	m_sensors[ADC_FLOW2_SENSOR_NUM].value = 0;
	m_sensors[ADC_FLOW2_SENSOR_NUM].rawValue = 0;
#ifdef USE_LGAR_FLOW_SENSOR2
	m_sensors[ADC_FLOW2_SENSOR_NUM].rawMin = (uint32_t)( ((2.3 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );//1911; // (2.8 / 2) V  => ( x = ( (2.8 / 2) * 0xFFF) / 3 )
	// !!! Специально завышаю максимальное значение
	m_sensors[ADC_FLOW2_SENSOR_NUM].rawMax = (uint32_t)( ((7.3 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );//2934; // (4.30 / 2) V  => ( x = ( (4.30 / 2) * 0xFFF) / 3 )
#else
	m_sensors[ADC_FLOW2_SENSOR_NUM].rawMin = (uint32_t)( ((0.5 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 200; // 341 - 140;  // (0.50 / 2) V  => ( x = ( (0.50 / 2) * 0xFFF) / 3 )
	m_sensors[ADC_FLOW2_SENSOR_NUM].rawMax = (uint32_t)( ((2.6 / 2) * (1 << (12 + ADC_OVERSAMPLING_BITS ))) / BOARD_VREF );// 1774; // (2.60 / 2) V  => ( x = ( (2.60 / 2) * 0xFFF) / 3 )
#endif // USE_LGAR_FLOW_SENSOR2
	m_sensors[ADC_FLOW2_SENSOR_NUM].valid = false;
	m_sensors[ADC_FLOW2_SENSOR_NUM].id = ADC_FLOW2_SENSOR_NUM;
	m_sensors[ADC_FLOW2_SENSOR_NUM].error = 0;
	m_sensors[ADC_FLOW2_SENSOR_NUM].lpFilterFunc = FilterFlow2Sensor;
	
	for(int i = 0; i < ADC_CHANNELS_COUNT; i++) {
		m_medFiltersData[i].stopValue = 0;
		u16_med_fltr_init( &m_medFiltersData[i], m_medFiltersBufs[i], MFILTER_SIZE ); 
	}
}
