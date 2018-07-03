

//#include <LPC17xx.H>
//#include <system_LPC17xx.h>
//#include <lpc17xx_pinsel.h>
//#include <lpc17xx_uart.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdint.h>
//#include <lpc17xx_timer.h>
#include "modbs_master.h"
#include "gpio.h"


void Timer_Init( void );
void USART_Init( uint16_t br_val );
bool mbPack( lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint16_t reg_num );
unsigned short CRC16_mbm(unsigned char *nData, unsigned short wLength);
void Port_Init( void );
bool TIMER_mbInterrupt( lpMbParam_t mbParam  );
void UART_Tx(uint8_t data);
void UART_mbInterrupt( void );

bool mbIsPacked = false;
uint8_t mbTxIndex = 0;
uint8_t mbRCVDPos = 0;
uint8_t txdata[8];



bool  Slave_request(lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num)
{
	mbParam->Flags.isRecvPacket = 0;
	if((reg_num*2 + 5) > mbParam->rxBufSize)			// rcv msg length > buf_len   ?for 0x03 func
		return false;
	
	mbParam->address = addr;
	mbParam->function = func;
	mbParam->num_regs = reg_num;
	
	mbParam->Flags.isPacket = mbPack(mbParam, addr, func, reg_start, reg_num);
	mb_start_tx(mbParam, 8);					//только 0x03
		return true;
};

bool Slave_write(lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_addr, uint16_t data)
{

	if((8) > mbParam->rxBufSize)			// answer size > rxBuf
	return false;
	
	mbParam->address = addr;
	mbParam->function = func;
	mbParam->reg_addr = reg_addr;
	mbParam->reg_data = data;
	
	mbParam->Flags.isPacket = mbPack(mbParam, addr, func, reg_addr, data);
	mbParam->Flags.isCRCValid = 0;
	mb_start_tx(mbParam, 8);					//только 0x03 и 0x06
		return true;
	
	
};


bool mbPack( lpMbParam_t mbParam, uint8_t addr, uint8_t func, uint16_t reg_start, uint16_t reg_num )
{
	
		if(func == 0x03 && mbParam->txBufSize < 8)
		{return false;};
		
		
		
		switch(mbParam->function)
		{
			case(0x03):
						mbParam->txBuf[0] = addr;		
						mbParam->txBuf[1] = func;
						mbParam->txBuf[2] = (reg_start >> 8);
						mbParam->txBuf[3] = (reg_start & 0xFF);
						mbParam->txBuf[4] = (reg_num >> 8);
						mbParam->txBuf[5] = (reg_num & 0xFF);		
						uint16_t mbCRC16;		
						mbCRC16 = CRC16_mbm((unsigned char *)(mbParam->txBuf), 6);					// BufSize = 8;
						mbParam->txBuf[6] = (mbCRC16 & 0xFF);
						mbParam->txBuf[7] = (mbCRC16 >> 8);
					break;
			case(0x06):
						mbParam->txBuf[0] = addr;
						mbParam->txBuf[1] = func;
						mbParam->txBuf[2] = (reg_start >> 8);
						mbParam->txBuf[3] = (reg_start & 0xFF);
						mbParam->txBuf[4] = (reg_num >> 8);
						mbParam->txBuf[5] = (reg_num & 0xFF);
						uint16_t mbCRC16_t = CRC16_mbm((unsigned char *)(mbParam->txBuf), 6);
						mbParam->txBuf[6] = (mbCRC16_t & 0xFF);
						mbParam->txBuf[7] = (mbCRC16_t >> 8);
				break;
		};

		
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
		//mbParam->timeOut = (((4 * 10 * 1000000L) / baudrate));
	
		
		
		mbParam->Flags.value = 0;
		mbParam->Flags.isRxDataMain = 1;
		//mbParam->hostTimeLeft = 0;
		mbParam->timeLeft = 0;
		mbParam->rxCount = 0;
		mbParam->txCount = 0;
		mbParam->errorCount = 0;
		mbParam->Flags.isBeginValid = 0;

		
		float end_packet_delay = (100 * 10 * 1000000L)/baudrate;					// +/- время на 4 символа в микросекундах
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
	HAL_GPIO_WritePin(GPIOB, GPIO_PIN_1, GPIO_PIN_SET);
	
	if(lpParam->lpSetTxEn != NULL)
		(*lpParam->lpSetTxEn)(true);
	if(lpParam->lpSetRxEn != NULL)
		(*lpParam->lpSetRxEn)(false);
	
	
	

	
//	if(lpParam->lpSetLed != NULL)
//		(*lpParam->lpSetLed)(true);
	
	lpParam->txCount = size;						//////    так
	lpParam->txIndex = 0;
	lpParam->Flags.isEndOfPacket = 0;
	lpParam->Flags.isBusy = true;
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
		//lpParam->Flags.isRxDataMain = 1;
		
		mb_rx_reset( lpParam );
		lpParam->Flags.isRxDataMain = 1;
		return;
	}
	
	// Сбрасываем время
	lpParam->timeLeft = 0;
	lpParam->Flags.isRxDataMain = 1;
	if(!lpParam->Flags.isRxDataMain) {
		return;
	}
	
	if(lpParam->rxCount == 0) {								//!!!!!!! ТУТ !!!!!!!
		// Принят первый байт пакета - адрес устройтсва
		// Для ускорения сразу проверяем адрес
		// Адрес = 0 - широковещательный запрос, поэтому реагируем на него
		if(!((data == lpParam->address) || (data == 0))) {
			// Адрес не совпал
			lpParam->Flags.isRxDataMain = 1;
			//lpParam->Flags.enableTimer = 1;
			mb_rx_reset( lpParam );
			return;
		}
	}
	if(lpParam->rxCount == 1)
	{
		if(!(data == lpParam->function))
		{
			lpParam->Flags.isRxDataMain = 1;
			//lpParam->Flags.enableTimer = 1;
			mb_rx_reset( lpParam );
			return;
		}
		else
		{
				lpParam->Flags.isBeginValid = 1;
		};
		
			
	};
	
	// Сбрасываем флаг таймаута связи с хостом
	lpParam->Flags.isHostTimeout = 0;
	// Сбрасывем время прошедшее после последней транзакции
	//lpParam->timeLeft = 0;
	// Забираем данные
	lpParam->rxBuf[lpParam->rxCount] = data;
	lpParam->rxCount++;
	// Активируем флаг разрешения работы таймера
	//lpParam->Flags.enableTimer = 1;
	lpParam->timeLeft = 0;
	
//	if(lpParam->lpSetLed != NULL) 
//		(*lpParam->lpSetLed)(true);
	
	// Для ускорениярассчета CRC16 - рассчитываем очередное значение CRC каждого принятого байта,
	// , чтобы не считать CRC всего пакета после окончания приема
//	lpParam->crcInfo.counter = lpParam->rxCount;
//	lpParam->crcInfo.crc = CalcCrc16Next(lpParam->crcInfo.crc, data, lpParam->crcInfo.counter == 1);
}

void mb_rx_reset( lpMbParam_t mbParam )
{
	
	mbParam->rxCount = 0;
	//mbParam->Flags.isCRCValid = 0;
	mbParam->Flags.isRxDataMain = 0;
	mbParam->Flags.isParsedStart = 0;
	mbParam->Flags.isBeginValid = 0;
	mbParam->Flags.isBusy = false;
	
	return;
	
};


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
		if((mbParam->txIndex) < (mbParam->txCount))
		{
		//	(* mbParam->lpSetTxEn)(true);
			(*mbParam->lpUartSend)(mbParam->txBuf[mbParam->txIndex++]);
		}
		else
		{
			mb_stop_tx(mbParam);
			mb_tx_reset(mbParam);			

			//mbParam ->Flags.enableTimer = 0;
			//mb_start_timer(mbParam);
			//mbParam->Flags.isRxDataMain = 1;
			//mbParam->Flags.isRxDataMain = 1;
			//(* mbParam->lpSetTxEn)(false);
			//(* mbParam->lpSetRxEn)(true);
		}
	}
	
	if(irr_type == RxInt)																															// Rx state																	
	{
			mbParam->Flags.isBusy  = true;
			mb_reset_timer(mbParam);
			uint8_t data = (*mbParam->lpUartRecv)();
			mb_rx_func(mbParam, data, false, false);
			mbParam ->Flags.enableTimer = 1;
			mbParam ->Flags.isHostTimeout = 0;
			mb_start_timer(mbParam);
			//HAL_GPIO_WritePin(GPIOA, GPIO_PIN_15, 1);
	}
	
	
	
};



void mb_tx_reset( lpMbParam_t mbParam)
{
		mbParam->txIndex = 0;
		mbParam->txCount = 0;
		mbParam->Flags.enableParseRxData = 0;
		//mbParam->Flags.enableTimer = 0;
		mbParam->Flags.isRxDataMain = 1;
		mbParam->Flags.isPacket = 0;
		
};

void mb_Timer_Int( lpMbParam_t mbParam )
{
	
	
		if(!mbParam->Flags.enableTimer)
		{
			return;
		};
		
		if((mbParam->timeLeft < mbParam->timeOut) && (mbParam->Flags.enableTimer == 1))
		{
				if(mbParam->timeLeft == 0x14)
					{uint8_t fff = 1;};
				mbParam->timeLeft += mbParam->timer_set;

				return;
		}
		else
		{

			//HAL_GPIO_WritePin(GPIOA, GPIO_PIN_15, 0);
			if(mbParam->Flags.isBeginValid)
			{
					mb_reset_timer( mbParam );
					mbParam->Flags.isBusy = false;
					mbParsePacket( mbParam );
				
			};
			mbParam->timeLeft = 0;
			mb_rx_reset( mbParam );
			mb_reset_timer(mbParam);
			mbParam->Flags.isHostTimeout = 1;
			mbParam->Flags.isEndOfPacket = 1;
			
		};

		//HAL_GPIO_WritePin(GPIOA, GPIO_PIN_15, 0);

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
		
			mbParam->Flags.enableTimer = 0;
			mbParam->Flags.isHostTimeout = 0;
			mbParam->Flags.enableParseRxData = 0;
			mbParam->timeLeft = 0;
			(* mbParam->lpTimerReset)();
	
};




void mbParsePacket( lpMbParam_t mbParam )
{

		mbParam->Flags.isParsedStart = 1;
		mbParam->Flags.isRecvPacket = 1;
		
		if( (mbParam->function == 0x03) && (mbParam->rxCount  == ((mbParam->num_regs)*2 + 5)))				// если читаем 0x03, то в ответ должено быть (	адрес(1) + функция(1) + количество(1) + CRC(2) + (num_regs*2)	)	байт
		{	
				uint16_t checkCRC = CRC16_mbm((unsigned char *)(mbParam->rxBuf), (mbParam->rxCount) );
			
				if(!checkCRC)
					mbParam->Flags.isCRCValid = 1;
				else
				{
					mbParam->Flags.isCRCValid = 0;
					return;
				}
					
		}
		else
		{
			return;
		}
		mbParam->Flags.isParsedStart = 0;
		
		uint8_t num_regs = mbParam->rxBuf[2];
		for( int i = 0; i < num_regs/2; i+=1)
		{
		
			mbParam->rxData[i] = (mbParam->rxBuf[2*i+3]<<8)+(mbParam->rxBuf[2*i+4]);
		
		};
		mbParam->Flags.isRecvPacket = 1;
		
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








