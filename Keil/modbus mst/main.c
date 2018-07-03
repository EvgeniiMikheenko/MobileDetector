//////////////






#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
#include <lpc17xx_uart.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <lpc17xx_timer.h>
//#include <port.h>
//#include <mbcrc.h>
//#include <mb.h>
//#include <mb_port.h>
#include "modbs_master.h"

#define REG_INPUT_START 	1
#define REG_INPUT_NREGS 	4

#define	DE_true			LPC_GPIO0 -> FIOSET |= 0x20000		//0x6001E
#define	DE_false		LPC_GPIO0 -> FIOCLR |= 0x20000		
#define RE_true			LPC_GPIO0 -> FIOCLR |= 0x40000
#define RE_false		LPC_GPIO0 -> FIOSET |= 0x40000

#define BR 115200
#define MBRXBUF_SIZE			10
#define MBTXBUF_SIZE			10




typedef enum
{
    MB_ENOERR,                  /*!< no error. */
    MB_ENOREG,                  /*!< illegal register address. */
    MB_EINVAL,                  /*!< illegal argument. */
    MB_ETIMEDOUT                /*!< timeout error occurred. */
} ErrorCode;



uint8_t cmddata[8];
uint8_t mbRCVData[9];
//volatile uint8_t mbRCVDPos = 0;
//uint8_t *mbRCVTr = &mbRCVData[0];

union Data
{
	uint8_t 	SLAVE_ADDR;
	uint8_t 	FUNC;
	uint16_t	START_REG_ADDR;
	uint16_t 	REG_NUM;
	uint16_t 	MBCRC16;
} DataPacket;




bool MBMCommand( uint8_t addr, uint8_t func, uint16_t reg_addr, uint16_t reg_num );
void UART_SendData( uint8_t sendbyte);
ErrorCode	MBMasterRXHandle( union Data *lp, uint8_t byte);
void RxIntEnable( void );
void RxIntDisable( void );
bool mbRxParse( uint8_t Data[]);

unsigned short CRC16 (unsigned char *nData, unsigned short wLength);
void UART_Cfg( uint32_t baudrate);
void Port_Cfg( void );
void UART_Wait( void );


volatile uint32_t tickcounter;
volatile bool fmbRCVFlag = false;
volatile bool mbIsPacket = false;
volatile uint8_t mbSendInd = 0;
uint8_t mbBufLng = sizeof(cmddata)/sizeof(uint8_t);
//uint8_t mbBufLng = 0;
volatile bool mbIsLast = false;








MbParam_t    PARAMS;
void MbParam_Init( void );
void uart_init( uint32_t baudrate );
void uart_send( uint8_t data );
uint8_t uart_recv( void );
void uart_rx_en( bool value );
void uart_tx_en( bool value );
void timer_start( void );
void timer_reset( void );
uint8_t get_int_sourse( void );


uint8_t		MbRxBuf[MBRXBUF_SIZE];
uint8_t 	MbTxBuf[MBTXBUF_SIZE];

TIM_TIMERCFG_Type		Timer_Params;
TIM_MATCHCFG_Type		Timer_Match;
uint8_t timer_period = 10;
int main( void )
{
	
	
	Timer_Params.PrescaleOption = TIM_PRESCALE_USVAL;
	Timer_Params.PrescaleValue = timer_period;
	Timer_Match.IntOnMatch = SET;
	
	
	TIM_Init(LPC_TIM1, TIM_TIMER_MODE, &Timer_Params);
	TIM_ConfigMatch(LPC_TIM1, &Timer_Match);
	
	Port_Cfg();
	MbParam_Init();
	//UART_Cfg(PARAMS.serialBaudrate);
	mbInit(&PARAMS);
	
	__enable_irq();
	NVIC_EnableIRQ(UART1_IRQn);
	NVIC_EnableIRQ(TIMER1_IRQn);

	
	
	
	SysTick_Config(SystemCoreClock / BR/5);
	while(1)
	{
					if(tickcounter >= 1000000)
					{
							tickcounter = 0;
							//bool message = Slave_request(&PARAMS, 0x01, 0x03, 0x001E, 0x0002);
							bool message = Slave_write(&PARAMS, 0x01, 0x06, 0x001E,	0xDD);
							
							
					};
	};
};	


void MbParam_Init( void )
{

	PARAMS.address = 0;
	PARAMS.function = 0;
	PARAMS.num_regs = 0;
	PARAMS.stopBits = 1;
	PARAMS.serialBaudrate = bps115200;
	
	
	PARAMS.rxBuf = MbRxBuf;
	PARAMS.rxBufSize = MBRXBUF_SIZE;
	PARAMS.rxCount = 0;
	
	
	PARAMS.txBuf = MbTxBuf;
	PARAMS.txBufSize = MBTXBUF_SIZE;
	PARAMS.txCount = 0;
	PARAMS.txIndex = 0;
	
	PARAMS.timeLeft = 0;
	PARAMS.timeOut = 0;
	PARAMS.timer_set = timer_period;
	
	
	PARAMS.lpUartInit = uart_init;
	PARAMS.lpUartSend = uart_send;
	PARAMS.lpUartRecv = uart_recv;
	
	PARAMS.lpSetRxEn = uart_rx_en;
	PARAMS.lpSetTxEn = uart_tx_en;
	
	PARAMS.lpTimerStart = timer_start;
	PARAMS.lpTimerReset = timer_reset;
	PARAMS.GetIntSourse = get_int_sourse;
	
	
	
	PARAMS.Flags.enableTimer = 0;
	PARAMS.Flags.enableParseRxData = 0;
	PARAMS.Flags.isParsedStart = 0;
	PARAMS.Flags.isRxDataMain = 0;
	PARAMS.Flags.isHostTimeout = 0;
	PARAMS.Flags.isConfigChangeLock = 0;
	PARAMS.Flags.eepromNeedUpdate = 0;
	PARAMS.Flags.isPasswordValid = 0;
	PARAMS.Flags.isPacket = 0;

};



void Port_Cfg( void )
{
	LPC_GPIO0 -> FIODIR |= 0x60000;				//RE DE is output
		
	LPC_PINCON -> PINSEL0 |= (1 << 30);		//Tx P0.15
	LPC_PINCON -> PINSEL1 |= 1;						//Rx P0.16

};


void UART_Cfg( uint32_t baudrate)
{
	
	UART_CFG_Type		UART0CFGSTR;
	
	
	UART0CFGSTR.Baud_rate = baudrate;
	UART0CFGSTR.Databits = UART_DATABIT_8;
	UART0CFGSTR.Parity = UART_PARITY_NONE;
	UART0CFGSTR.Stopbits = UART_STOPBIT_1;
	
	
	UART_Init((LPC_UART_TypeDef *)LPC_UART1, &UART0CFGSTR);
	UART_TxCmd((LPC_UART_TypeDef *)LPC_UART1, ENABLE);
	UART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_THRE, ENABLE);	
	UART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_RBR, ENABLE);
};

void uart_init( uint32_t baudrate )
{

		UART_Cfg(baudrate);

};





void uart_send( uint8_t sendbyte)
{
	
	
	UART_Wait();
	UART_SendByte((LPC_UART_TypeDef *)LPC_UART1, sendbyte);
};



void UART_Wait( void )
{
		while( !(LPC_UART1 -> LSR & (1 << 5))){};
	return;
};



void SysTick_Handler(void)
{
	
	
		tickcounter++;
		return;
};
	
	
	
void	UART1_IRQHandler(void)
{
		mb_rx_tx_interrupt(&PARAMS);
		return;
};	


void TIMER1_IRQHandler(void)
{
	
		TIM_ClearIntPending(LPC_TIM1, TIM_MR1_INT);
		mb_Timer_Int(&PARAMS);
		return;

};

uint8_t uart_recv( void )
{

		return UART_ReceiveByte((LPC_UART_TypeDef *)LPC_UART1);

};

void uart_rx_en( bool value )
{
		if( value )
		{
			//UART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_THRE, ENABLE);
			RE_true;
			DE_false;
		}
		else
		{
			RE_false;
			DE_true;
			//UART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_THRE, DISABLE);
		};
};

void uart_tx_en( bool value )
{

		if( value )
		{
			//UART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_RBR, ENABLE);
			DE_true;
			RE_false;
		}
		else
		{
			//sUART_IntConfig((LPC_UART_TypeDef *)LPC_UART1, UART_INTCFG_RBR, DISABLE);
			DE_false;
			RE_true;
		};
	
};



uint8_t get_int_sourse( void )
{

		uint32_t mbIIRStatus = UART_GetIntId((LPC_UART_TypeDef *)LPC_UART1);
		uint8_t mb_int_sourse;


		if(mbIIRStatus == 0x2)			//tx state
		{		
				mb_int_sourse = TxInt;
				//mb_int_sourse = 0x10;
		};
		
		if(mbIIRStatus == 0x4)		//rx state
		{
				mb_int_sourse = RxInt;
				//mb_int_sourse  = 0x01;
		};
		
		return mb_int_sourse;
};





void timer_start( void )
{
		TIM_ResetCounter(LPC_TIM1);
		TIM_Cmd(LPC_TIM1, ENABLE);
	
};
void timer_reset( void )
{
	
		
		TIM_ResetCounter(LPC_TIM1);
		TIM_Cmd(LPC_TIM1, DISABLE);
	
};
	
	



