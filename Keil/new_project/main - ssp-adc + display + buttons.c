#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
//#include <lpc17xx_gpio.h>
//#include <lpc17xx_systick.h>
//#include <lpc17xx_uart.h>
#include <lpc17xx_ssp.h>
//#include <lpc17xx_pwm.h>
//#include <stdbool.h>
//#include <stdlib.h>
//#include <stdint.h>
//#include <LPC17xx_timer.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>

#define DISPLAY_DATA			0xFA00
#define DISPLAY_CMD				0xF800
#define DISPLAY_ROWS_COUNT		2
#define DISPLAY_ROW_LEN			40

#define DISPROW1_HDR			"Read ADC:"

static uint16_t m_DispRows[DISPLAY_ROWS_COUNT][DISPLAY_ROW_LEN];

void delay(uint32_t count);
uint8_t select_channel_adc(uint8_t channel);
#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
//#include <lpc17xx_gpio.h>
//#include <lpc17xx_systick.h>
//#include <lpc17xx_uart.h>
#include <lpc17xx_ssp.h>
//#include <lpc17xx_pwm.h>
//#include <stdbool.h>
//#include <stdlib.h>
//#include <stdint.h>
//#include <LPC17xx_timer.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>

#define DISPLAY_DATA			0xFA00
#define DISPLAY_CMD				0xF800
#define DISPLAY_ROWS_COUNT		2
#define DISPLAY_ROW_LEN			40

#define DISPROW1_HDR			"Read ADC:"

static uint16_t m_DispRows[DISPLAY_ROWS_COUNT][DISPLAY_ROW_LEN];

void delay(uint32_t count);
uint8_t select_channel_adc(uint8_t channel);

void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow);
void disp_update( void );

uint8_t button;
uint8_t button1;
int main (){
	
	SystemInit (); 
	
	
#define clear	   			0xF801
#define home				0xF802
#define disp_Success		0xFA53
#define disp_Fail			0xFA46
#define ADC_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 2);
#define ADC_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 2);	
#define DISP_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 3);
#define DISP_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 3);
	
	
uint8_t channel = 1;	
uint16_t  startbyte =	select_channel_adc(channel);
	
int tmp;

	uint16_t receivedata[20];
	uint16_t testdata[4];
	for (uint16_t i = 0; i < 5; i++){
	
		testdata[i] = 0x1111*i;
	};
	
	
//	uint8_t space = 0x0;
	
	memset( m_DispRows[0], 0, DISPLAY_ROW_LEN );
	memset( m_DispRows[1], 0, DISPLAY_ROW_LEN );
		

	
	LPC_PINCON -> PINSEL0 = 0xA8000;    //UART PIN CFG
	LPC_PINCON -> PINSEL0 |= ( 0 << 8 ) | ( 0 << 9 ); 
	LPC_PINCON -> PINSEL2 |= ( 0 << 29 ) | ( 0 << 28 );
	LPC_PINCON -> PINSEL3 |= ( 0 << 2 ) | ( 0 << 3 ); 	//
	LPC_PINCON -> PINMODE0 |= ( 0 << 8 ) | ( 0 << 9 );
	LPC_PINCON -> PINMODE2 |= ( 0 << 29 ) | ( 0 << 28 );
	LPC_PINCON -> PINMODE3 |= ( 0 << 2 ) | ( 0 << 3 );	 //
	LPC_GPIO0 -> FIODIR |= (1 << 3)|(1 << 2);
	LPC_GPIO0 -> FIODIR |= ( 0 << 4 );   // DIP1 IN
	LPC_GPIO1 -> FIODIR |= ( 0 << 14 );	 // SIP3 IN
	LPC_GPIO1 -> FIODIR |= ( 1 << 17 );	 // DANG OUT	
	

	
	
	ADC_CS_OFF;
	DISP_CS_OFF;
	delay(10000);
	
	
	SSP_CFG_Type SSPCFG;
	
	SSPCFG.Databit = SSP_DATABIT_16;
	SSPCFG.CPHA = SSP_CPHA_FIRST;
	SSPCFG.CPOL = SSP_CPOL_HI;
	SSPCFG.Mode = SSP_MASTER_MODE;
	SSPCFG.FrameFormat = SSP_FRAME_SPI;
	SSPCFG.ClockRate = 100000;
	
	
	
	SSP_Init(LPC_SSP1, &SSPCFG);
	
	SSP_Cmd(LPC_SSP1, ENABLE);
	
	ADC_CS_OFF
	DISP_CS_ON
	
	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF80C);		//Disp On	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF828);		//Func_set
	delay(100);
	SSP_SendData(LPC_SSP1, clear);
	delay(100);
	SSP_SendData(LPC_SSP1, home);	
	

uint8_t k=0;
//	int n=0;
//	int j=0;

	SysTick_Config(SystemCoreClock / 2500);   // 25 MKS
	button = 0xAA;
	button1 = 0xAA;
	
  while(1){
		
//	DISP_CS_OFF
//	ADC_CS_ON		

	
	LPC_SSP1 -> CR0 = 0x7; 		//8 bit
	SSP_SendData(LPC_SSP1, startbyte);
	
		
		
	while ((LPC_SSP1->SR & 0x10)) {			
	};


	
	
	
	LPC_SSP1 -> CR0 = 0xF; 		//16 bit
		
		
	receivedata[k] = SSP_ReceiveData(LPC_SSP1);

	delay(1000);
		
	
	
		
	LPC_SSP1 -> CR0 = 0x7; 		//8 bit		
		
	
	
	tmp = SSP_ReceiveData(LPC_SSP1);
//	SSP_SendData(LPC_SSP1, space);
	
	while ((LPC_SSP1->SR & 0x10)) {			
		};
	receivedata[k] = (receivedata[k] << 1) + (tmp >> 8);
	
	
	
	
	
	LPC_SSP1 -> CR0 = 0xF; 		//16 bit
	
	delay(1000);
	ADC_CS_OFF		
	DISP_CS_ON
	

	//SSP_SendData(LPC_SSP1, clear);
//		SSP_SendData(LPC_SSP1, clear);

	disp_show( DISPROW1_HDR, strlen(DISPROW1_HDR), 0, 0, true);
	if (receivedata[k] != 0){
		disp_show( " ok", 12, 0, 9, true);
	}
	else {
		disp_show( " error", 15, 0, 9, true);
	}
	
	
		disp_show( "second_line ", 12, 1, 1, false);
		


	
	
	
//	
//	
//	char tBuf[6];
//	memset( tBuf, 0, 6 );
//	int len = sprintf(tBuf, "0x%04X", testdata[n]);
//	tBuf[len] = '\0';
//	disp_show( "  ", 15, 1, 13, false);
//	
//	delay(1000);
//	disp_show( tBuf, len+15, 1, 15, false);
//	delay(10000);
//	
//	
//	
//	if ( j == 180){
//	j=0;
//	n++;	
//	}
//	if ( n > 3 ){
//		n=0;
//	};
//	
//	j++; 
	

	
	
	
	
	
	//SSP_SendData(LPC_SSP1, clear);
//	if (receivedata[k] != 0){
//			for(int i = 0; i < 30; i++){
//			SSP_SendData(LPC_SSP1, disp_Success);
////				while ((LPC_SSP1->SR & 0x4)) {			
////				};
//				delay(100);
//			}
//	}
//	else{
//			for(int i = 0; i < 30; i++){
//			SSP_SendData(LPC_SSP1, disp_Fail);
////					while ((LPC_SSP1->SR & 0x4)) {			
////					};
//					delay(100);
//			}
//	}

	
	
	disp_update();
	delay(10000);
	
//	DISP_CS_OFF;
//	ADC_CS_ON;
	
	

		k == 20 ? k=0 : k++;
//	
	
	


		
}
	
}

void delay(uint32_t count) {
	volatile uint32_t tmp = count;
	while((--tmp) != 0) {
	};
}

void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow) {
	if((lpStr == (char*)NULL) || (strNum >= 2) || (strLen <= 0))
		return;
	
	for(int i = column  ; i < DISPLAY_ROW_LEN; i++) {
		if(i < strLen) {
			m_DispRows[strNum][i] = DISPLAY_DATA | (*lpStr);
			lpStr++;
		}
		else if(isFullRow){
			m_DispRows[strNum][i] = DISPLAY_DATA | ' ';
		}
		
	}
}

void disp_update( void ) {
	
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}
	
	DISP_CS_ON;
	delay(1000);
	static int row = 0;
	static int col = 0;
	
	SSP_SendData(LPC_SSP1, m_DispRows[row][col]);
	
	col++;

	if(col >= DISPLAY_ROW_LEN) {
			col = 0;
			row++;
			if(row >= DISPLAY_ROWS_COUNT) {
				row = 0;
				SSP_SendData(LPC_SSP1, home);	
			}
		}
	
	//SSP_SendData(LPC_SSP1, m_DispRows[row][col]);
	
	
	
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}	
		
	delay(1000);
	DISP_CS_OFF;
	//ADC_CS_ON;
	delay(1000);
	
}


uint8_t select_channel_adc(uint8_t channel){


	uint8_t ch = channel;
	
	uint8_t start_byte =	0x87 + (ch << 4);	

return start_byte;

}




int c = 1;
void SysTick_Handler(void) {
	
	
	
	
	button = button << 1;
	if ( (LPC_GPIO0 -> FIOPIN0 & 0x10 ) == 0x10){
		button++;
	};
	
	
	if (button == 0xFF){
		
		disp_show( " up  ", 18, 1, 13, false);
	};
	
	if (button == 0x0){
		
		disp_show( " down ", 19, 1, 13, false);
	};
	
	/////////////////////
		button1 = button1 << 1;
	if ( (LPC_GPIO1 -> FIOPIN & 0x4000) == 0x4000){
		button1++;
	};

	
	
	if (button1 == 0xFF){
		c++;
	};
	
	if (c%2 == 1){
		
		LPC_GPIO1 -> FIOCLR = ( 1 << 17 );
		delay(100);
	}
	else {
	LPC_GPIO1 -> FIOSET = ( 1 << 17 );
	delay(100);
	};
	
	
	
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow);
void disp_update( void );

uint8_t button;
uint8_t button1;
int main (){
	
	SystemInit (); 
	
	
#define clear	   			0xF801
#define home				0xF802
#define disp_Success		0xFA53
#define disp_Fail			0xFA46
#define ADC_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 2);
#define ADC_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 2);	
#define DISP_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 3);
#define DISP_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 3);
	
	
uint8_t channel = 1;	
uint16_t  startbyte =	select_channel_adc(channel);
	
int tmp;

	uint16_t receivedata[20];
	uint16_t testdata[4];
	for (uint16_t i = 0; i < 5; i++){
	
		testdata[i] = 0x1111*i;
	};
	
	
//	uint8_t space = 0x0;
	
	memset( m_DispRows[0], 0, DISPLAY_ROW_LEN );
	memset( m_DispRows[1], 0, DISPLAY_ROW_LEN );
		

	
	LPC_PINCON -> PINSEL0 = 0xA8000;    //UART PIN CFG
	LPC_PINCON -> PINSEL0 |= ( 0 << 8 ) | ( 0 << 9 ); 
	LPC_PINCON -> PINSEL2 |= ( 0 << 29 ) | ( 0 << 28 );
	LPC_PINCON -> PINSEL3 |= ( 0 << 2 ) | ( 0 << 3 ); 	//
	LPC_PINCON -> PINMODE0 |= ( 0 << 8 ) | ( 0 << 9 );
	LPC_PINCON -> PINMODE2 |= ( 0 << 29 ) | ( 0 << 28 );
	LPC_PINCON -> PINMODE3 |= ( 0 << 2 ) | ( 0 << 3 );	 //
	LPC_GPIO0 -> FIODIR |= (1 << 3)|(1 << 2);
	LPC_GPIO0 -> FIODIR |= ( 0 << 4 );   // DIP1 IN
	LPC_GPIO1 -> FIODIR |= ( 0 << 14 );	 // SIP3 IN
	LPC_GPIO1 -> FIODIR |= ( 1 << 17 );	 // DANG OUT	
	

	
	
	ADC_CS_OFF;
	DISP_CS_OFF;
	delay(10000);
	
	
	SSP_CFG_Type SSPCFG;
	
	SSPCFG.Databit = SSP_DATABIT_16;
	SSPCFG.CPHA = SSP_CPHA_FIRST;
	SSPCFG.CPOL = SSP_CPOL_HI;
	SSPCFG.Mode = SSP_MASTER_MODE;
	SSPCFG.FrameFormat = SSP_FRAME_SPI;
	SSPCFG.ClockRate = 100000;
	
	
	
	SSP_Init(LPC_SSP1, &SSPCFG);
	
	SSP_Cmd(LPC_SSP1, ENABLE);
	
	ADC_CS_OFF
	DISP_CS_ON
	
	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF80C);		//Disp On	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF828);		//Func_set
	delay(100);
	SSP_SendData(LPC_SSP1, clear);
	delay(100);
	SSP_SendData(LPC_SSP1, home);	
	

uint8_t k=0;
//	int n=0;
//	int j=0;

	SysTick_Config(SystemCoreClock / 2500);   // 25 MKS
	button = 0xAA;
	button1 = 0xAA;
	
  while(1){
		
//	DISP_CS_OFF
//	ADC_CS_ON		

	
	LPC_SSP1 -> CR0 = 0x7; 		//8 bit
	SSP_SendData(LPC_SSP1, startbyte);
	
		
		
	while ((LPC_SSP1->SR & 0x10)) {			
	};


	
	
	
	LPC_SSP1 -> CR0 = 0xF; 		//16 bit
		
		
	receivedata[k] = SSP_ReceiveData(LPC_SSP1);

	delay(1000);
		
	
	
		
	LPC_SSP1 -> CR0 = 0x7; 		//8 bit		
		
	
	
	tmp = SSP_ReceiveData(LPC_SSP1);
//	SSP_SendData(LPC_SSP1, space);
	
	while ((LPC_SSP1->SR & 0x10)) {			
		};
	receivedata[k] = (receivedata[k] << 1) + (tmp >> 8);
	
	
	
	
	
	LPC_SSP1 -> CR0 = 0xF; 		//16 bit
	
	delay(1000);
	ADC_CS_OFF		
	DISP_CS_ON
	

	//SSP_SendData(LPC_SSP1, clear);
//		SSP_SendData(LPC_SSP1, clear);

	disp_show( DISPROW1_HDR, strlen(DISPROW1_HDR), 0, 0, true);
	if (receivedata[k] != 0){
		disp_show( " ok", 12, 0, 9, true);
	}
	else {
		disp_show( " error", 15, 0, 9, true);
	}
	
	
		disp_show( "second_line ", 12, 1, 1, false);
		


	
	
	
//	
//	
//	char tBuf[6];
//	memset( tBuf, 0, 6 );
//	int len = sprintf(tBuf, "0x%04X", testdata[n]);
//	tBuf[len] = '\0';
//	disp_show( "  ", 15, 1, 13, false);
//	
//	delay(1000);
//	disp_show( tBuf, len+15, 1, 15, false);
//	delay(10000);
//	
//	
//	
//	if ( j == 180){
//	j=0;
//	n++;	
//	}
//	if ( n > 3 ){
//		n=0;
//	};
//	
//	j++; 
	

	
	
	
	
	
	//SSP_SendData(LPC_SSP1, clear);
//	if (receivedata[k] != 0){
//			for(int i = 0; i < 30; i++){
//			SSP_SendData(LPC_SSP1, disp_Success);
////				while ((LPC_SSP1->SR & 0x4)) {			
////				};
//				delay(100);
//			}
//	}
//	else{
//			for(int i = 0; i < 30; i++){
//			SSP_SendData(LPC_SSP1, disp_Fail);
////					while ((LPC_SSP1->SR & 0x4)) {			
////					};
//					delay(100);
//			}
//	}

	
	
	disp_update();
	delay(10000);
	
//	DISP_CS_OFF;
//	ADC_CS_ON;
	
	

		k == 20 ? k=0 : k++;
//	
	
	


		
}
	
}

void delay(uint32_t count) {
	volatile uint32_t tmp = count;
	while((--tmp) != 0) {
	};
}

void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow) {
	if((lpStr == (char*)NULL) || (strNum >= 2) || (strLen <= 0))
		return;
	
	for(int i = column  ; i < DISPLAY_ROW_LEN; i++) {
		if(i < strLen) {
			m_DispRows[strNum][i] = DISPLAY_DATA | (*lpStr);
			lpStr++;
		}
		else if(isFullRow){
			m_DispRows[strNum][i] = DISPLAY_DATA | ' ';
		}
		
	}
}

void disp_update( void ) {
	
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}
	
	DISP_CS_ON;
	delay(1000);
	static int row = 0;
	static int col = 0;
	
	SSP_SendData(LPC_SSP1, m_DispRows[row][col]);
	
	col++;

	if(col >= DISPLAY_ROW_LEN) {
			col = 0;
			row++;
			if(row >= DISPLAY_ROWS_COUNT) {
				row = 0;
				SSP_SendData(LPC_SSP1, home);	
			}
		}
	
	//SSP_SendData(LPC_SSP1, m_DispRows[row][col]);
	
	
	
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}	
		
	delay(1000);
	DISP_CS_OFF;
	//ADC_CS_ON;
	delay(1000);
	
}


uint8_t select_channel_adc(uint8_t channel){


	uint8_t ch = channel;
	
	uint8_t start_byte =	0x87 + (ch << 4);	

return start_byte;

}




int c = 1;
void SysTick_Handler(void) {
	
	
	
	
	button = button << 1;
	if ( (LPC_GPIO0 -> FIOPIN0 & 0x10 ) == 0x10){
		button++;
	};
	
	
	if (button == 0xFF){
		
		disp_show( " up  ", 18, 1, 13, false);
	};
	
	if (button == 0x0){
		
		disp_show( " down ", 19, 1, 13, false);
	};
	
	/////////////////////
		button1 = button1 << 1;
	if ( (LPC_GPIO1 -> FIOPIN & 0x4000) == 0x4000){
		button1++;
	};

	
	
	if (button1 == 0xFF){
		c++;
	};
	
	if (c%2 == 1){
		
		LPC_GPIO1 -> FIOCLR = ( 1 << 17 );
		delay(100);
	}
	else {
	LPC_GPIO1 -> FIOSET = ( 1 << 17 );
	delay(100);
	};
	
	
	
}
*/