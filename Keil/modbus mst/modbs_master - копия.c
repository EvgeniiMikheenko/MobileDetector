

#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
#include <lpc17xx_uart.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <lpc17xx_timer.h>
#include "modbs_master.h"




#define TIMER3_INTERVAL_US	100
#define TIMER3_PRIORITY			8
#define BR		115200
#define RE		LPC_GPIO0 -> FIOCLR |= 0x60000			//Rx Enable
#define DE		LPC_GPIO0 -> FIOSET |= 0x60000			//Tx Enable
#define TxInt	010
#define	RxInt	101


typedef struct {
	uint16_t crc;
	uint32_t counter;
} Crc16Info_t, *lpCrc16Info_t;




typedef void (* SetBit)(bool value);
typedef void (* UartInit)(uint16_t baudrate);
typedef void (* UartSend)(uint8_t data);
typedef void (* TimerCmd)( void );
typedef uint8_t (* UartRecv)( void );
typedef uint8_t (* IRRType)( void );







typedef enum
{
	bps9600 	= 0,
	bps14400 	= 1,
	bps19200 	= 2,
	bps38400 	= 3,
	bps57600 	= 4,
	bps115200 	= 5
} usart_baudrates;




bool  Slave_request(lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num);
void mbInit(lpMbParam_t mbParam);
uint32_t mb_convert_baudrate( uint16_t value );
void mb_stop_tx(lpMbParam_t Param);
void mb_start_tx(lpMbParam_t lpParam, uint32_t size);
void mb_rx_func(lpMbParam_t lpParam, uint8_t data, bool frameError, bool overflowErroe);
void mb_rx_tx_interrupt( lpMbParam_t mbParam );
void mb_tx_reset( lpMbParam_t mbParam);
void mb_rx_reset( lpMbParam_t mbParam );
void mb_start_timer( lpMbParam_t mbParam);
void mb_reset_timer( lpMbParam_t mbParam);



void Timer_Init( void );
void USART_Init( uint16_t br_val );
bool mbPack( lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num );
unsigned short CRC16_mbm(unsigned char *nData, unsigned short wLength);
//void mb_slave_timer_tick(lpMbSlaveParam_t lpParam, uint32_t us);
void Port_Init( void );
bool TIMER_mbInterrupt( lpMbParam_t mbParam  );
void UART_Tx(uint8_t data);
void UART_mbInterrupt( void );

bool mbIsPacked = false;
uint8_t mbTxIndex = 0;
bool mbIsLast = false;
uint32_t tickcounter;
bool fmbRCVFlag = false;
uint8_t mbRCVDPos = 0;
uint8_t txdata[8];

void Timer_Init( void )
{
	TIM_TIMERCFG_Type TMR_Cfg;
	TIM_MATCHCFG_Type TMR_Match;
		
	/* On reset, Timer0/1 are enabled (PCTIM0/1 = 1), and Timer2/3 are disabled (PCTIM2/3 = 0).*/
	/* Initialize timer 0, prescale count time of 100uS */
	TMR_Cfg.PrescaleOption = TIM_PRESCALE_USVAL;
	TMR_Cfg.PrescaleValue = 1;
	/* Use channel 0, MR0 */
	TMR_Match.MatchChannel = 0;
	/* Enable interrupt when MR0 matches the value in TC register */
	TMR_Match.IntOnMatch = ENABLE;		
	/* Enable reset on MR0: TIMER will reset if MR0 matches it */
	TMR_Match.ResetOnMatch = TRUE;		
	/* Don't stop on MR0 if MR0 matches it*/
	TMR_Match.StopOnMatch = FALSE;
	/* Do nothing for external output pin if match (see cmsis help, there are another options) */
	TMR_Match.ExtMatchOutputType = TIM_EXTMATCH_NOTHING;
	/* Set Match value, count value of 100 (100 * 1uS = 100us = 0.0001s --> 10000 Hz) */
	TMR_Match.MatchValue = TIMER3_INTERVAL_US;		
	/* Set configuration for Tim_config and Tim_MatchConfig */
	TIM_Init(LPC_TIM3, TIM_TIMER_MODE, &TMR_Cfg);
	TIM_ConfigMatch(LPC_TIM3, &TMR_Match);
	
	/* Set priority */
	NVIC_SetPriority(TIMER3_IRQn, TIMER3_PRIORITY);
	/* Enable interrupt for timer 0 */
	NVIC_EnableIRQ(TIMER3_IRQn);	
	/* Start timer 0 */
	//TIM_Cmd(LPC_TIM3, ENABLE);

};


bool  Slave_request(lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num)
{
	
	if((reg_num*2 + 5) > mbParam->rxBufSize)			// rcv msg length > buf_len   ?for 0x03 func
		return false;
	
	mbParam->address = addr;
	mbParam->function = func;
	mbParam->num_regs = reg_num;
	
	mbParam->Flags.isPacket = mbPack(mbParam, addr, func, reg_start, reg_num);
	mb_start_tx(mbParam, mbParam->txBufSize);
		
};




bool mbPack( lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num )
{
		//uint8_t txdata[8];
		
//		txdata[0] = m_MBParam ->address;
//		txdata[1] = m_MBParam ->function;
//		txdata[2] = (m_MBParam ->register_start) >> 8;
//		txdata[3] = (m_MBParam ->register_start) & 0xFF;
//		txdata[4] = (m_MBParam ->register_num) >> 8;
//		txdata[5] = (m_MBParam ->register_num) & 0xFF;
	
		if(func == 0x03 && mbParam->txBufSize < 8)
		{return false;};
	
		mbParam->txBuf[0] = addr;
		mbParam->txBuf[1] = func;
		mbParam->txBuf[2] = (reg_start >> 8);
		mbParam->txBuf[3] = (reg_start & 0xFF);
		mbParam->txBuf[4] = (reg_num >> 8);
		mbParam->txBuf[5] = (reg_num & 0xFF);
		
		
		uint16_t mbCRC16;
		
		mbCRC16 = CRC16_mbm((unsigned char *)(mbParam->txBuf), 6);					// BufSize = 8;

		
		mbParam->txBuf[6] = (mbCRC16 >> 8);
		mbParam->txBuf[7] = (mbCRC16 & 0xFF);
		
		
		
		
		
		return true;
};	
	
	









void mbInit(lpMbParam_t mbParam)
{
		if(mbParam == NULL)
			return;
	
	
		uint32_t baudrate = mb_convert_baudrate((uint16_t)mbParam->serialBaudrate);
	
		
		if(mbParam->lpUartInit != NULL) {
			// Инициализация UART
			(*mbParam->lpUartInit)(baudrate);
		}
	
		// Рассчитываем таймаут окончания транзакции modbus
		mbParam->timeout = (((4 * 10 * 1000000L) / baudrate));
	
		
		
		mbParam->Flags.value = 0;
		mbParam->Flags.isRxDataMain = 1;
		mbParam->hostTimeLeft = 0;
		mbParam->timeLeft = 0;
		mbParam->rxCount = 0;
		mbParam->txCount = 0;
		mbParam->errorCount = 0;
	
		mb_stop_tx(mbParam);

};



uint32_t mb_convert_baudrate( uint16_t regValue )
{
		uint32_t result = 0;
	
	switch(regValue) {
		case bps9600:
			result = 9600;
			break;
		case bps14400:
			result = 14400;
			break;
		case bps19200:
			result = 19200;
			break;
		case bps38400:
			result = 38400;
			break;
		case bps57600:
			result = 57600;
			break;
		case bps115200:
			result = 115200;
			break;
		default:
			
			break;
	}
	return result;
};


void mb_stop_tx(lpMbParam_t lpParam)
{
		if(lpParam == NULL)
		return;
	
	lpParam->rxCount = 0;
	lpParam->Flags.enableParseRxData = 0;
	lpParam->Flags.isParsedStart = 0;
	
	if(lpParam->lpSetTxEn != NULL) {
		(*lpParam->lpSetTxEn)(false);
	}

};


void mb_start_tx(lpMbParam_t lpParam, uint32_t size)
	{
	if(lpParam == NULL)
		return;
	if(!lpParam->Flags.isPacket)
		return;	
	
	
	if(lpParam->lpSetTxEn != NULL)
		(*lpParam->lpSetTxEn)(true);
	if(lpParam->lpSetRxEn != NULL)
		(*lpParam->lpSetRxEn)(false);
	
	
	

	
//	if(lpParam->lpSetLed != NULL)
//		(*lpParam->lpSetLed)(true);
	
	lpParam->txCount = size;
	lpParam->txIndex = 0;
	
	if(lpParam->lpUartSend != NULL)
		(*lpParam->lpUartSend)(lpParam->txBuf[lpParam->txIndex++]);
	
}




void mb_rx_func(lpMbParam_t lpParam, uint8_t data, bool frameError, bool overflowError)
	{
	if(lpParam == NULL)
		return;
	
	if(lpParam->Flags.isParsedStart) {
		// Текущий пакет еще не обработан
		lpParam->timeLeft = 0;
		return;
	}
	
	if(frameError || overflowError) {
		// Ошибка порта - сбрасываем весь пакет
		lpParam->rxCount = 0;
		// Выставляем флаг готовности к приему
		lpParam->Flags.isRxDataMain = 1;
		return;
	}
	
	if(lpParam->rxCount >= lpParam->rxBufSize) {
		// Переполнение буфера
		lpParam->rxCount = 0;
		lpParam->Flags.isRxDataMain = 1;
		return;
	}
	
	// Сбрасываем время
	lpParam->timeLeft = 0;
	if(!lpParam->Flags.isRxDataMain) {
		return;
	}
	
	if(lpParam->rxCount == 0) {
		// Принят первый байт пакета - адрес устройтсва
		// Для ускорения сразу проверяем адрес
		// Адрес = 0 - широковещательный запрос, поэтому реагируем на него
		if(!((data == lpParam->address) || (data == 0))) {
			// Адрес не совпал
			lpParam->Flags.isRxDataMain = 1;
			lpParam->Flags.enableTimer = 1;
			return;
		}
	}
	
	// Сбрасываем флаг таймаута связи с хостом
	lpParam->Flags.isHostTimeout = 0;
	// Сбрасывем время прошедшее после последней транзакции
	lpParam->timeLeft = 0;
	// Забираем данные
	lpParam->rxBuf[lpParam->rxCount] = data;
	lpParam->rxCount++;
	// Активируем флаг разрешения работы таймера
	lpParam->Flags.enableTimer = 1;
	lpParam->timeLeft = 0;
	
//	if(lpParam->lpSetLed != NULL) 
//		(*lpParam->lpSetLed)(true);
	
	// Для ускорениярассчета CRC16 - рассчитываем очередное значение CRC каждого принятого байта,
	// , чтобы не считать CRC всего пакета после окончания приема
//	lpParam->crcInfo.counter = lpParam->rxCount;
//	lpParam->crcInfo.crc = CalcCrc16Next(lpParam->crcInfo.crc, data, lpParam->crcInfo.counter == 1);
}




void mb_rx_tx_interrupt( lpMbParam_t mbParam )
{

	
//	uint32_t mbIIRStatus = UART_GetIntId((LPC_UART_TypeDef *)LPC_UART1);
//	bool frameError = false;
//	bool overflowError = false;
//	
	//IntType irr_type;
	
	uint8_t	irr_type;
	irr_type = (*mbParam->GetIntSourse)();
	
	
	
	
	
	if(irr_type == TxInt)																																//Tx state
//	if(mbIIRStatus == 0x02)																															
	{
		if((mbParam->txIndex) <= (mbParam->txCount))
			(*mbParam->lpUartSend)(mbParam->txBuf[mbParam->txIndex++]);
		else
			mb_tx_reset(mbParam);			
			mb_stop_tx(mbParam);
			mbParam ->Flags.enableTimer = 1;
			mb_start_timer(mbParam);
			(* mbParam->lpSetTxEn)(false);
			(* mbParam->lpSetRxEn)(true);
	}
	
	if(irr_type == RxInt)																																// Rx state
//	if(mbIIRStatus == 0x04)																															
	{
			/*Get frameError, overflowError*/
			uint8_t data = (*mbParam->lpUartRecv)();
			mb_rx_func(mbParam, data, false, false);
			mbParam ->Flags.enableTimer = 0;
			mb_reset_timer(mbParam);
	}
	
	
	
};



void mb_tx_reset( lpMbParam_t mbParam)
{
		mbParam->txIndex = 0;
		mbParam->txCount = 0;
		//mbParam->Flags.enableParseRxData = 1;
};

void mb_Timer_Int( lpMbParam_t mbParam )
{

	
		//mbParam->timer_set;
	
		float end_packet_delay = 4000000/mbParam->serialBaudrate;
		if((float)mbParam->timer_set > end_packet_delay)
		{
			mbParam->timeLeft = 1;
			mbParam->timeOut = 1;
		}
		else
		{
				mbParam->timeOut = end_packet_delay;
				mbParam->timeLeft = 0;		
		}
		
		
		if((mbParam->timeLeft < mbParam->timeOut) && (mbParam->Flags.enableTimer == 1))
		{
				mbParam->timeLeft += mbParam->timer_set;
				return;
		}
		else
		{
			mbParam->timeLeft = 0;
			mb_rx_reset( mbParam );
			mbParam->Flags.enableTimer = 0;
			mbParam->Flags.isHostTimeout = 1;
			mbParam->Flags.enableParseRxData = 1;
		};

};




void mb_start_timer( lpMbParam_t mbParam)
{
			mbParam->Flags.enableTimer = 1;
			mbParam->Flags.isHostTimeout = 0;
			mbParam->Flags.enableParseRxData = 0;
			(* mbParam->lpTimerStart)();
};


void mb_reset_timer( lpMbParam_t mbParam)
{
		
			mbParam->Flags.enableTimer = 1;
			mbParam->Flags.isHostTimeout = 0;
			mbParam->Flags.enableParseRxData = 0;
			(* mbParam->lpTimerReset)();
	
};



unsigned short CRC16_mbm(unsigned char *nData, unsigned short wLength)
{
static const unsigned short wCRCTable[] = {
   0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
   0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
   0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
   0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
   0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,

   0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
   0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
   0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
   0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
   0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
   0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
   0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
   0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,

   0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
   0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
   0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
   0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
   0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
   0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
   0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
   0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,

   0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
   0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
   0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
   0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
   0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
   0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
   0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
   0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,

   0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
   0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
   0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040 };

unsigned char nTemp;
unsigned short wCRCWord = 0xFFFF;

   while (wLength--)
   {
      nTemp = *nData++ ^ wCRCWord;
      wCRCWord >>= 8;
      wCRCWord  ^= wCRCTable[nTemp];
   }
   return wCRCWord;

};








