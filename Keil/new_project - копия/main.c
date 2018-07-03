//////////////

//////rfm12b - 433s1 hoperf Tx     Õ≈ –¿¡Œ“¿≈“ Õ≈“ œ–≈–€¬¿Õ»ﬂ Œ“ œ≈–≈ƒ¿◊»

#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
#include <lpc17xx_ssp.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <lpc17xx_timer.h>
#include <lpc17xx_gpio.h>

#define RF43B_PWRSTATE_READY		0x01
#define RF43B_PACKET_SENT_INTERRUPT		0x04
#define RF43B_PWRSTATE_TX		0x09
#define CS_ON			LPC_GPIO0 -> FIOCLR |= ( 1 << 20 );
#define CS_OFF		LPC_GPIO0 -> FIOSET |= ( 1 << 20 );

#define nIRQ			LPC_GPIO0 -> FIOPIN & ( 1 << 19 );

#define SSP0		LPC_SSP0
#define HL_ON		LPC_GPIO0 -> FIOSET |= (1 << 23);
#define HL_OFF	LPC_GPIO0 -> FIOCLR |= (1 << 23);



	void RF43b_init( void );
	void delay( int del );
	void to_tx_mode( void );
	void to_ready_mode( void );
	void rx_reset( void );
	void spi_write( uint8_t address, uint8_t cmd );
	uint8_t spi_read( uint8_t address );
	uint8_t send_command( void );
	void read_reg( void );
	void wait_ssp( void );
	void wait_ready( void );
	

	SSP_CFG_Type SSP0CFG;
	uint8_t trash;
	uint8_t TxData[17] = {0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3a,0x3b,0x3c,0x3d,0x3e,0x3f,0x78}; 
	int flag = 0;

int main()
{

	LPC_SC -> PCONP |= (1 << 21)|(1 << 15);
	LPC_SC -> PCLKSEL1 |= (1 << 10);
	LPC_PINCON -> PINSEL0 |= 0x80000000;
	LPC_PINCON -> PINSEL1 |= 0X28;
	// ok
	
	
	
	
	
	LPC_GPIO0 -> FIODIR |= (1 << 20);
	//LPC_PINCON -> PINMODE1 |= (0 << 2) & (0 << 3);
	
	
	LPC_GPIO0 -> FIODIR2 |= 0x80;
	LPC_PINCON -> PINMODE1 |= 0xC000;
	
	SSP0CFG.ClockRate = 100000;
	SSP0CFG.CPHA = SSP_CPHA_SECOND;
	SSP0CFG.CPOL = SSP_CPOL_HI;
	SSP0CFG.Databit = SSP_DATABIT_8;
	SSP0CFG.FrameFormat = SSP_FRAME_SPI;
	SSP0CFG.Mode = SSP_MASTER_MODE;
	
	SSP_Init(SSP0, &SSP0CFG);
	SSP_Cmd(SSP0, ENABLE);
	
	
	
	__enable_irq();
	//LPC_GPIOINT -> IO0IntEnF |= (1 << 4);
//	LPC_GPIOINT -> IO0IntEnF |= (1 << 19);
	LPC_GPIOINT -> IO0IntEnR |= (1 << 19);
//	LPC_GPIOINT -> IO0IntEnR |= (1 << 4);
	NVIC_EnableIRQ(EINT3_IRQn);
	
	//wait_ready();
	CS_OFF;
	delay(1000000);
	delay(1000000);
	delay(1000000);
	delay(1000000);
	delay(1000000);
	delay(1000000);
	delay(1000000);
	delay(1000000);	
	delay(1000000);
	delay(1000000);
	
	read_reg();
	delay(100000);
	
	RF43b_init();
	delay(100000);
	
	

	read_reg();
	
	
	
	
	int flah = 0;
	
	while(1)
	{

		//if(flag)
		if(!(LPC_GPIO0 -> FIOPIN &(1 << 4)) && flah == 0)
		{
			
			flah = 1;
			HL_ON;
			flag = 0;
			//RF43b_init();
			to_tx_mode();
			uint8_t test = spi_read(0x03);
			delay(10000);
			delay(10000);
			HL_OFF;
			//read_reg();
		};

		if( (LPC_GPIO0 -> FIOPIN & (1 << 4)) && flah == 1)
		{
			flah = 0;
		};
		
	};


}


void RF43b_init( void )
{
/*
	uint8_t ItStatus1 = spi_read(0x03); // read status, clear interrupt  
	uint8_t ItStatus2 = spi_read(0x04); 
	
	spi_write(0x07, 0x01);
	
	spi_write(0x05, 0x04);
	spi_write(0x06, 0x02);   // no wakeup up, lbd, 
	spi_write(0x07, 0x18);    // disable lbd, wakeup timer, use internal 32768,xton = 1; in ready mode 
	spi_write(0x09, 0x01); // c = 12.5p 
	spi_write(0x0a, 0x05); 
//	spi_write(0x0b, 0xf2);   // gpio0 for Tx state
//	spi_write(0x0c, 0xef);   // gpio 1 for clk output 
//	spi_write(0x0d, 0xf2);   // gpio 2 micro-controller clk output 
//	spi_write(0x0e, 0x00);   // gpio   0, 1,2 NO OTHER FUNCTION. 
	spi_write(0x70, 0x20);   // disable manchest Data Rates below 30 kbps
	 //case RATE_24K: // 2.4k 
	spi_write(0x6e, 0x13); 
	spi_write(0x6f, 0x6f); 
	//PH+FIFO 
	spi_write(0x30, 0x6d);   // enable packet handler, msb first, enable crc, crc-16 //8c
	 // 0x31 only readable 
	//spi_write(0x32, 0xff);   // 0x32address enable for headere byte 0, 1,2,3, receive header check 	for byte 0, 1,2,3 
	spi_write(0x33, 0x2d); // header 3, 2, 1,0 used for head length, fixed packet length, 	synchronize word length 3, 2, 1,0
	spi_write(0x34, 0x08);     // 64 nibble = 32byte preamble 
	spi_write(0x36, 0x2d);   // synchronize word 
	spi_write(0x37, 0xd4); 
	spi_write(0x38, 0x55); 
//	spi_write(0x39, 0x00); 
	spi_write(0x3a, 's');   // tx header 
	spi_write(0x3b, 'o'); 
//	spi_write(0x3c, 'n'); 
//	spi_write(0x3d, 'g'); 
	spi_write(0x3e, 0x10);   // total tx 17 byte 
	// 0x52, 53, 54, 55 set to default 
	// 0x56 ---------0x6c  ?????????????????????????? 
	spi_write(0x6d, 0x07); // set power max power //0x0f
	spi_write(0x79, 0x00);   // no hopping 
	spi_write(0x7a, 0x00);   // no hopping 
	spi_write(0x71, 0x3A); // Gfsk, fd[8] =0, no invert for Tx/Rx data, fifo mode, txclk -->gpio //0x22
	spi_write(0x72, 0x38); // frequency deviation setting to 45k = 72*625 
	spi_write(0x73, 0x00); 
	spi_write(0x74, 0x00);   // no offset 
	//band 434 
	spi_write(0x75, 0x53);   // hbsel = 0, sbsel =1 ???, fb = 19 
	spi_write(0x76, 0x64);   // 25600= 0x6400 for 434Mhz 
	spi_write(0x77, 0x00); 
		
	spi_write(0x07, 0x09);	
	
	*/
	
uint8_t	ItStatus1 = spi_read(0x03); // read status, clear interrupt  
uint8_t ItStatus2 = spi_read(0x04); 




 spi_write(0x06, 0x00);   // no wakeup up, lbd, 
 spi_write(0x07, RF43B_PWRSTATE_READY);    // disable lbd, wakeup timer, use internal 32768,xton = 1; in ready mode 

 spi_write(0x09, 0x7f); // c = 12.5p 
spi_write(0x0a, 0x05); 
 spi_write(0x0b, 0xf4);   // gpio0 for received data output 
 spi_write(0x0c, 0xef);   // gpio 1 for clk output 
 spi_write(0x0d, 0xfd);   // gpio 2 micro-controller clk output 
 spi_write(0x0e, 0x00);   // gpio   0, 1,2 NO OTHER FUNCTION. 
 spi_write(0x70, 0x20);   // disable manchest 
 //case RATE_24K: // 2.4k 
spi_write(0x6e, 0x13); 
spi_write(0x6f, 0xa9); 
//PH+FIFO 
 spi_write(0x30, 0x8c);   // enable packet handler, msb first, enable crc, 
 // 0x31 only readable 
 spi_write(0x32, 0xff);   // 0x32address enable for headere byte 0, 1,2,3, receive header check for byte 0, 1,2,3 

 spi_write(0x33, 0x42); // header 3, 2, 1,0 used for head length, fixed packet length, synchronize word length 3, 2, 

 spi_write(0x34, 64);     // 64 nibble = 32byte preamble 

 spi_write(0x36, 0x2d);   // synchronize word 
spi_write(0x37, 0xd4); 
spi_write(0x38, 0x00); 
spi_write(0x39, 0x00); 
 spi_write(0x3a, 's');   // tx header 
spi_write(0x3b, 'o'); 
spi_write(0x3c, 'n'); 
spi_write(0x3d, 'g'); 
 spi_write(0x3e, 17);   // total tx 17 byte 
// 0x52, 53, 54, 55 set to default 
// 0x56 ---------0x6c  ?????????????????????????? 
 spi_write(0x6d, 0x0f); // set power max power 
 spi_write(0x79, 0x0);   // no hopping 
 spi_write(0x7a, 0x0);   // no hopping 
 spi_write(0x71, 0x22); // Gfsk, fd[8] =0, no invert for Tx/Rx data, fifo mode, txclk -->gpio 
 spi_write(0x72, 0x38); // frequency deviation setting to 45k = 72*625 
spi_write(0x73, 0x0); 
 spi_write(0x74, 0x0);   // no offset 
//band 434 
  spi_write(0x75, 0x53);   // hbsel = 0, sbsel =1 ???, fb = 19 
 spi_write(0x76, 0x64);   // 25600= 0x6400 for 434Mhz 
spi_write(0x77, 0x00);
	spi_write(0x05, 0x04);		// packet send interrupt
	
	
};

void delay( int del )
{

	while( del )
	del--;

};


void to_tx_mode( void )
{
		
 	HL_ON;
	to_ready_mode(); 
	delay(10000);
	delay(10000);
	delay(10000);
	spi_write(0x08, 0x03);   // disABLE AUTO TX MODE, enble multi packet clear fifo 
	//spi_write(0x08, 0x00);   // disABLE AUTO TX MODE, enble multi packet, clear fifo 

		// ph +fifo mode 
	spi_write(0x34, 64);   // 64 nibble = 32byte preamble 
	spi_write(0x3e, 17);   // total tx 17 byte 
	for (int i = 0; i<17; i++) 
	{ 
		spi_write(0x7f, TxData[i]); 
	} 
	
	
	spi_write(0x05, RF43B_PACKET_SENT_INTERRUPT); 
	uint8_t ItStatus1 = spi_read(0x03); //read the Interrupt Status1 register 
	uint8_t ItStatus2 = spi_read(0x04);  spi_write(0x07, RF43B_PWRSTATE_TX);   // to tx mode 
	uint8_t ItStatus3;
	while ( !(LPC_GPIO0 -> FIOPIN & (1 << 19)))
	{
		ItStatus3 = spi_read(0x0E);
		int trace = 0;
	
	};
	
	
	to_ready_mode(); 

	delay(5000000);
	LPC_GPIO0 -> FIOCLR |= (1 << 23);
		
};

void to_ready_mode( void )
{
	
	spi_write(0x07, RF43B_PWRSTATE_READY); 
	
};


void rx_reset( void )
{
		
	spi_write(0x07, 0x01);
	trash = spi_read(0x03);
	trash = spi_read(0x04);
	spi_write(0x7e, 0x17);
	spi_write(0x08, 0x03);
	spi_write(0x08, 0x00);
	spi_write(0x07, 0x05);
	spi_write(0x05, 0x02);
	
};


void spi_write( uint8_t address, uint8_t cmd )
{
	
	CS_ON;
	//address += 0x80;
	SSP_SendData(SSP0, address + 0x80);
	wait_ssp();	
	while( LPC_SSP0 -> SR & (1 << 2))
	{
			trash = SSP_ReceiveData(SSP0);
	};

	delay(1000);
	SSP_SendData(SSP0, cmd);
	wait_ssp();
	while(LPC_SSP0 -> SR & (1 << 2))
	{
			trash = SSP_ReceiveData(SSP0);
	};

	CS_OFF;
	
};


uint8_t spi_read( uint8_t address )
{

	CS_ON;
	SSP_SendData(SSP0, address);
	wait_ssp();
	delay(1000);
	while( LPC_SSP0 -> SR & (1 << 2))
	{
			trash = SSP_ReceiveData(SSP0);
	};

	SSP_SendData(SSP0, 0x00);
	wait_ssp();
	delay(1000);
	//trash = SSP_ReceiveData(SSP0);
	uint8_t answer = SSP_ReceiveData(SSP0);
	CS_OFF;
	return answer;

};


uint8_t send_command( void )
{

	CS_ON;
	SSP_SendData(SSP0, 0xAA);
	delay(10000);
	uint8_t data = SSP_ReceiveData(SSP0);
	CS_OFF;
	
	return data;
};

	void read_reg( void )
	{
		
		
		while( LPC_SSP0 -> SR & ( 1 << 2 ))
		{
		
			trash = SSP_ReceiveData(SSP0);
			
		};
		
		
			int index = 0;
			uint8_t readrg[10];
			readrg[index] = spi_read(0x05);
			index++;
			readrg[index] = spi_read(0x03);
			index++;
			readrg[index] = spi_read(0x04);
			index++;
			readrg[index] = spi_read(0x05);
			index++;
			readrg[index] = spi_read(0x06);
			index++;
			readrg[index] = spi_read(0x30);
			index++;
			readrg[index] = spi_read(0x70);
			index++;
			readrg[index] = spi_read(0x75);
			index++;
			readrg[index] = spi_read(0x76);
			index++;
			readrg[index] = spi_read(0x77);
			index++;
////	spi_write(0x09, 0x7f); // c = 12.5p 
////	spi_write(0x0a, 0x05); 
////	spi_write(0x0b, 0xf4);   // gpio0 for received data output 
////	spi_write(0x0c, 0xef);   // gpio 1 for clk output 
////	spi_write(0x0d, 0xfd);   // gpio 2 micro-controller clk output 
////	spi_write(0x0e, 0x00);   // gpio   0, 1,2 NO OTHER FUNCTION. 
////	spi_write(0x70, 0x20);   // disable manchest 
////	spi_write(0x75, 0x53);   // hbsel = 0, sbsel =1 ???, fb = 19 
////	spi_write(0x76, 0x64);   // 25600= 0x6400 for 434Mhz 
////	spi_write(0x77, 0x00); 
			
			
	};

void wait_ssp( void )
{

		while( LPC_SSP0 -> SR & (1 << 4)){};


};





void EINT3_IRQHandler( void )
{		
	
		
	
	
	//	LPC_GPIOINT -> IO0IntClr = (1 << 4);
		LPC_GPIOINT -> IO0IntClr = (1 << 19);
	
		delay(100000);
	
	
		
	
		if(!( LPC_GPIO0 -> FIOPIN & (1 << 4)))
		{
			flag = 1;
		}
		else
		{
				int k = 23;
		}
			
		if(LPC_GPIO0 -> FIOPIN & (1 << 19))
		{
			int hh = 0;
		}
		if(!(LPC_GPIO0 -> FIOPIN & (1 << 19)))
		{
			int hh = 1;
		}
		
		

};
	
void wait_ready( void )
{
	uint8_t i = 1;
	uint8_t status = 0;
	while(i)
	{
		status = spi_read( 0x04 );
		if(status & 0x02)
		{
			i = 0;
		}
		//else{spi_write(0x05, 0x02);};
	}
	
};


