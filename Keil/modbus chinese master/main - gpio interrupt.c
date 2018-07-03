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
#define clear	   			0xF801
#define home				0xF802
#define ADC_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 2);
#define ADC_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 2);	
#define DISP_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 3);
#define DISP_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 3);




void delay(uint32_t count);
void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow);
void disp_update( void );

#define DISPLAY_ROWS_COUNT		2
#define DISPLAY_ROW_LEN			40
static uint16_t m_DispRows[DISPLAY_ROWS_COUNT][DISPLAY_ROW_LEN];




int c=0;

void EINT3_IRQHandler(void){
	LPC_GPIOINT->IO0IntClr=(1 << 5);
	if (c%2 == 0){
		disp_show( "dip2_is_down ", 13, 0, 0, true);
	}
	else{ disp_show( "dip2_is_up ",11 , 0, 0, true);};
	c++;
	return;
}











int main (){
	
	LPC_PINCON -> PINSEL0 = 0xA8000;    //UART PIN CFG
//	LPC_PINCON -> PINSEL0 |= ( 0 << 8 ) | ( 0 << 9 ); 
//	LPC_PINCON -> PINSEL2 |= ( 0 << 29 ) | ( 0 << 28 );
//	LPC_PINCON -> PINSEL3 |= ( 0 << 2 ) | ( 0 << 3 ); 	//
//	LPC_PINCON -> PINMODE0 |= ( 0 << 8 ) | ( 0 << 9 );
//	LPC_PINCON -> PINMODE2 |= ( 0 << 29 ) | ( 0 << 28 );
//	LPC_PINCON -> PINMODE3 |= ( 0 << 2 ) | ( 0 << 3 );	 //
	LPC_GPIO0 -> FIODIR |= (1 << 3)|(1 << 2);
//	LPC_GPIO0 -> FIODIR |= ( 0 << 4 );   // DIP1 IN
//	LPC_GPIO1 -> FIODIR |= ( 0 << 14 );	 // SIP3 IN
//	LPC_GPIO1 -> FIODIR |= ( 1 << 17 );	 // DANG OUT	
	
	LPC_GPIO0->FIODIR |= (0 << 5);
	LPC_GPIOINT->IO0IntEnR |= (1 << 5);
	NVIC_EnableIRQ(EINT3_IRQn);
	
	
	memset( m_DispRows[0], 0, DISPLAY_ROW_LEN );
	memset( m_DispRows[1], 0, DISPLAY_ROW_LEN );
		
	

	DISP_CS_OFF;
	
	
	
	SSP_CFG_Type SSPCFG;
	
	SSPCFG.Databit = SSP_DATABIT_16;
	SSPCFG.CPHA = SSP_CPHA_FIRST;
	SSPCFG.CPOL = SSP_CPOL_HI;
	SSPCFG.Mode = SSP_MASTER_MODE;
	SSPCFG.FrameFormat = SSP_FRAME_SPI;
	SSPCFG.ClockRate = 100000;
	
	
	
	SSP_Init(LPC_SSP1, &SSPCFG);
	
	SSP_Cmd(LPC_SSP1, ENABLE);
	

	DISP_CS_ON
	
	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF80C);		//Disp On	
	delay(100);
	SSP_SendData(LPC_SSP1, 0xF828);		//Func_set
	delay(100);
	SSP_SendData(LPC_SSP1, clear);
	delay(100);
	SSP_SendData(LPC_SSP1, home);	
	delay(100);






	disp_show( "start", 5, 0, 0, true);
  while(1){
	
	DISP_CS_ON
	disp_update();
//	delay(100);
	LPC_GPIOINT->IO0IntEnR |= (1 << 5);
	NVIC_EnableIRQ(EINT3_IRQn);
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
	
	
	
	
	
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}	
		
	delay(1000);
	DISP_CS_OFF;
	//ADC_CS_ON;
	delay(1000);
	
}








