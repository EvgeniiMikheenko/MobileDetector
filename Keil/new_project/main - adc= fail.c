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
#define clear	   			0xF801
#define home				0xF802
#define disp_Success		0xFA53
#define disp_Fail			0xFA46
#define ADC_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 2);
#define ADC_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 2);	
#define DISP_CS_ON			LPC_GPIO0 -> FIOCLR = (1 << 3);
#define DISP_CS_OFF			LPC_GPIO0 -> FIOSET = (1 << 3);

static uint16_t m_DispRows[DISPLAY_ROWS_COUNT][DISPLAY_ROW_LEN];

void delay(uint32_t count);
void adc_read(void );

void disp_show(char* lpStr, int strLen, int strNum, int column, bool isFullRow);
void disp_update( void );

uint16_t receivedata[20];
	SSP_CFG_Type SSPCFG;
uint16_t testdata[20];

uint8_t l = 0;
uint8_t flag1=0;
uint8_t flag2=1;

uint8_t del[20];
uint8_t receivedata_first[20];
uint8_t receivedata_second[20];
uint8_t receivedata_third[20];

////uint16_t receive_first[20];
////uint8_t receive_second[20];

int cint = 0;

uint8_t disp_adc = 1;
uint8_t read=0;
uint8_t k=0;
int tmp;


uint16_t cr0_state;



int main (){
	
	 
	
	


	
	memset( m_DispRows[0], 0, DISPLAY_ROW_LEN );
	memset( m_DispRows[1], 0, DISPLAY_ROW_LEN );
		
	LPC_SC -> PCONP |= ( 1 << 10 );
	LPC_PINCON -> PINSEL0 = 0xA8000;    

	LPC_GPIO0 -> FIODIR |= (1 << 3)|(1 << 2);


	
	
	ADC_CS_OFF;
	DISP_CS_OFF;
	delay(10000);
	
	

	
	SSPCFG.Databit = SSP_DATABIT_16;
	SSPCFG.CPHA = SSP_CPHA_FIRST;
	SSPCFG.CPOL = SSP_CPOL_HI;
	SSPCFG.Mode = SSP_MASTER_MODE;
	SSPCFG.FrameFormat = SSP_FRAME_SPI;
	SSPCFG.ClockRate = 50000;
	
	
	
	SSP_Init(LPC_SSP1, &SSPCFG);
	
	cr0_state = LPC_SSP1 -> CR0;
	
	
	
	SSP_Cmd(LPC_SSP1, ENABLE);
	
	
	
	
	ADC_CS_OFF
	delay(100);
	DISP_CS_ON
	
	
	delay(100);
	//SSP_SendData(LPC_SSP1, 0xF80C);		//Disp On
	delay(100);	
	
	
	//SSP_SendData(LPC_SSP1, 0xF828);		//Func_set
	delay(100);
	
	//SSP_SendData(LPC_SSP1, clear);
	delay(100);
	
	
	//SSP_SendData(LPC_SSP1, home);
	delay(10000);
	
	
	
	


SysTick_Config(SystemCoreClock / 1000);

  while(1){

//	 ADC_CS_OFF 
//	 DISP_CS_ON 
	//disp_show( DISPROW1_HDR, 9, 0, 0, false);


//	  if (	(LPC_SSP1 -> SR & 0x1) == 0x1 ){
//		  
//		  
//		  disp_adc > 254 ? disp_adc = 1 : disp_adc++;
//		  		 
//	  }
	// delay(1000);
	if (flag1%2 == 1 ){
	
		adc_read();
	};
//	if (disp_adc%2 == 0){
//		
//		
////	SSP_DeInit(LPC_SSP1);
////	SSPCFG.Databit = SSP_DATABIT_16;			//16 bit
////	SSPCFG.CPHA = SSP_CPHA_FIRST;
////	SSPCFG.CPOL = SSP_CPOL_HI;
////	SSPCFG.Mode = SSP_MASTER_MODE;
////	SSPCFG.FrameFormat = SSP_FRAME_SPI;
////	SSPCFG.ClockRate = 50000;
////	
////	
////	SSP_Init(LPC_SSP1, &SSPCFG);	
////	SSP_Cmd(LPC_SSP1, ENABLE);	
//		
//		
//		//LPC_SSP1 -> CR0 |= 0xF; 			
//		
//		//for(int i = 0; i < 81; i++){
//		//disp_update();
//		//};
//	};
	  
//	
//	ADC_CS_ON
//	DISP_CS_OFF

	
		

	


		
}
	
}

//void SysTick_Handler(void) {
void adc_read(void){	
		flag2 = 0;
	DISP_CS_OFF
	delay(100);
	ADC_CS_ON
	
	
	uint8_t channel = 6;
	uint8_t startbyte =	0x87 + (channel << 4);	
	
	  
	  
	LPC_SSP1 -> CR0 |= 0x87; 		//8 bit
	
//	SSP_DeInit(LPC_SSP1);

//	SSPCFG.Databit = SSP_DATABIT_8;		
//	SSPCFG.CPHA = SSP_CPHA_FIRST;
//	SSPCFG.CPOL = SSP_CPOL_HI;
//	SSPCFG.Mode = SSP_MASTER_MODE;
//	SSPCFG.FrameFormat = SSP_FRAME_SPI;
//	SSPCFG.ClockRate = 50000;
//	
//	
//	
//	
//	
//	SSP_Init(LPC_SSP1, &SSPCFG);
//	SSP_Cmd(LPC_SSP1, ENABLE);
//	
	
//	del = SSP_ReceiveData(LPC_SSP1);

	
	
	
	SSP_SendData(LPC_SSP1, startbyte);
	
	delay(1000);
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}	
	
	del[k] = SSP_ReceiveData(LPC_SSP1);
		
		
		
	LPC_SSP1 -> CR0 |= 0x80;
	

	delay(100);
		
		
		
		
	//LPC_SSP1 -> CR0 += 0x87;
	SSP_SendData(LPC_SSP1, 0x00);
	//delay(100);
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}		

	receivedata_first[k] = SSP_ReceiveData(LPC_SSP1);
	
	SSP_SendData(LPC_SSP1, 0x00);
	//delay(100);
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}	
	receivedata_second[k] = SSP_ReceiveData(LPC_SSP1);
		
	SSP_SendData(LPC_SSP1, 0x00);
	//delay(100);
	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}	//	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {}	
	receivedata_third[k] = SSP_ReceiveData(LPC_SSP1);
	
		
	delay(100);
		
		
	//LPC_SSP1 -> CR0 = cr0_state;

	
	receivedata[k] = (((uint16_t)receivedata_first [k]) << 9) + (((uint16_t)receivedata_second [k]) << 1) + ( 0x0001 & (((uint16_t)receivedata_third [k]) >> 7));
	testdata[k] = (((uint16_t)del [k]) << 9) + (((uint16_t)receivedata_first [k]) << 1) + ( 0x0001 & (((uint16_t)receivedata_second [k]) >> 7));
	
	
	delay(100);
	//ADC_CS_OFF;	
//////	LPC_SSP1 -> CR0 |= 0xF; 		
//////	
//////	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}
//////	SSP_SendData(LPC_SSP1, 0x00);
//////	
//////	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}
//////	receive_first[k] = SSP_ReceiveData(LPC_SSP1) ;	
//////		
//////	

//////	delay(100);
//////	LPC_SSP1 -> CR0 |= 0x7;
//////	SSP_SendData(LPC_SSP1, 0x00);
//////	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}
//////	receive_second[k] = SSP_ReceiveData(LPC_SSP1) ;		
//////		
//////		
//////	receivedata[k] = ((uint16_t)receive_first[k] << 1) + ((uint16_t)receive_second[k] >> 7);	
	delay(100);
		
	float u_val;
	float u_ref = 2.5;
	u_val = ((float)receivedata[4] * u_ref)/((1 << 16)-1);
		
		char tBuf[12];
	memset( tBuf, 0, 12 );
	int len = sprintf(tBuf, "%04f", u_val);
	tBuf[len] = '\0';
	//disp_show( "  ", 1, 1, 0, false);
	

	disp_show( tBuf, len+1, 1, 2, false);
	delay(1000);
//	disp_show ("test", 14, 0, 10, false);
//	disp_show ("test1", 5, 1, 0, false);
	
	
	float R1=1000;
	
	float Rt= (  R1 * u_val )/( u_ref - u_val);
	
	
//	float R2= 3300/2;
//	float Rt= ( R2*u_ref - R1*u_val )/( u_val - u_ref );
	
	
	float temperature;
	
	float A = 3.81E-3;
	
	float Rz=1000;
	temperature = (Rt - Rz)/ A;
		
	sprintf(tBuf, "%04f", temperature);
	
	//disp_show( tBuf, 9, 1, 1, false);
	
		
		
	
	
	k > 19 ? k=0 : k++;
	ADC_CS_OFF
	delay(100);
	
	
	
//	for(int j =0; j < 81; j++){
//	disp_update();
//	delay(10000);
//	};
	flag1=0;
	flag2=1;
	cint++;
	///return;
};


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



void disp_update( void ){
	
	DISP_CS_ON
	ADC_CS_OFF
	
	
//	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}
//	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_BUSY) == SET) {};
	delay(100);
	DISP_CS_ON
	ADC_CS_OFF	
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
		
//	while(SSP_GetStatus(LPC_SSP1, SSP_STAT_TXFIFO_EMPTY) == RESET) {}
};



void SysTick_Handler(void) {
	
	if (flag2 == 1){
			l++;
			if ( l > 99 ){
				flag1=1;
				l=0;

			};
	
	
		};


};

