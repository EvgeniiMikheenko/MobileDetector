//////////////

//////rfm31b - 433s1 hoperf Rx

#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
#include <lpc17xx_ssp.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <lpc17xx_timer.h>



#define CS_ON			LPC_GPIO0 -> FIOCLR |= ( 1 << 20 );
#define CS_OFF		LPC_GPIO0 -> FIOSET |= ( 1 << 20 );

#define nIRQ			LPC_GPIO0 -> FIOPIN & ( 1 << 19 );

#define SSP0		LPC_SSP0


	void RF31b_init( void );
	void delay( int del );
	void to_rx_mode( void );
	void to_ready_mode( void );
	void rx_reset( void );
	void spi_write( uint8_t address, uint8_t cmd );
	uint8_t spi_read( uint8_t address );
	uint8_t send_command( void );
	void read_reg( void );
	void wait_ssp( void );
	
	

	SSP_CFG_Type SSP0CFG;
	uint8_t trash;
	uint8_t RxData[17];


int main()
{

	LPC_SC -> PCONP |= (1 << 21);
	LPC_SC -> PCLKSEL1 |= (1 << 10);
	LPC_PINCON -> PINSEL0 |= 0x80000000;
	LPC_PINCON -> PINSEL1 |= 0X28;
	
	LPC_GPIO0 -> FIODIR |= (1 << 20);
	LPC_PINCON -> PINMODE1 |= (0 << 2) & (0 << 3);
	
	
	
	SSP0CFG.ClockRate = 100000;
	SSP0CFG.CPHA = SSP_CPHA_FIRST;
	SSP0CFG.CPOL = SSP_CPOL_HI;
	SSP0CFG.Databit = SSP_DATABIT_8;
	SSP0CFG.FrameFormat = SSP_FRAME_SPI;
	SSP0CFG.Mode = SSP_MASTER_MODE;
	
	SSP_Init(SSP0, &SSP0CFG);
	SSP_Cmd(SSP0, ENABLE);
	
	
	
	__enable_irq();
	LPC_GPIOINT -> IO0IntEnF |= (1 << 19);
	LPC_GPIOINT -> IO0IntEnR |= (1 << 19);
	NVIC_EnableIRQ(EINT3_IRQn);
	
	
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
	
	read_reg();
	delay(100000);
	
	RF31b_init();
	delay(100000);
	
	to_rx_mode();

	read_reg();

	while(1)
	{
	
		if(!(LPC_GPIO0 -> FIOPIN & ( 1 << 19 )))
		{
		
			spi_read(0x7f);
			
			for(int i =0; i<17; i++)
			{
			
				RxData[i] = send_command();
			
			};
			
			int chksm = 0;
				for( int i =0; i<17; i++)
					chksm +=RxData[i];
			if((chksm == RxData[16]) && (RxData[0] == 0x30))
			{
			
				to_ready_mode();
			
			}
			else
			{
			
				rx_reset();
			
			}
			
		}
		else
		{
		
			read_reg();
			
		};
		
	};


}


void RF31b_init( void )
{

	uint8_t ItStatus1 = spi_read(0x03); // read status, clear interrupt  
	uint8_t ItStatus2 = spi_read(0x04); 
	
	spi_write(0x06, 0x00);  // interrupt all disable
	spi_write(0x07, 0x01);    // to ready mode
	spi_write(0x09, 0x7f);  // cap = 12.5pf
	spi_write(0x0a, 0x05);   //clk output is 2MHz
	spi_write(0x0b, 0xf5);  // GPIO0 is for Rx state
	spi_write(0x0c, 0xef); // GPIO1 Tx/Rx data clk output
	spi_write(0x0d, 0x00);   // GPIO2 for MCLK output
	spi_write(0x0e, 0x00);   //GPIO port use default value
	spi_write(0x0f, 0x70);  // NO ADC used
	spi_write(0x10, 0x00);  //no adc used
	spi_write(0x12, 0x00);  // no temperature sensor used
	spi_write(0x13, 0x00);  // no temperature sensor used
	spi_write(0x70, 0x20);   // no mancheset code, no data whiting, data rate < 30Kbps
	spi_write(0x1c, 0x1d);  // IF filter bandwidth
	spi_write(0x1d, 0x40);  // AFC LOOP 
	spi_write(0x1e, 0x08); //AFC timing
	spi_write(0x20, 0xa1);  //clock recovery 
	spi_write(0x21, 0x20);  //clock recovery
	spi_write(0x22, 0x4e);  //clock recovery
	spi_write(0x23, 0xa5);  //clock recovery
	spi_write(0x24, 0x00);  //clock recovery timing
	spi_write(0x25, 0x0a);  //clock recovery timing
	spi_write(0x2a, 0x1e);
	spi_write(0x2c, 0x29);
	spi_write(0x2d, 0x04);
	spi_write(0x2e, 0x29);
	//  spi_write(0x6e, 0x27);  // Tx data rate 1
	//  spi_write(0x6f, 0x52);  // Tx data rate 0
	spi_write(0x30, 0x41);    // data access control
	spi_write(0x32, 0xff);  // header control
	spi_write(0x33, 0x42);  //  //  header  3,  2,  1,0  used  for  head  length,  fixed  packet  length, synchronize word length 3, 2,
	spi_write(0x34, 64);     // 64 nibble = 32byte preamble
	spi_write(0x35, 0x20);  //0x35 need to detect 20bit preamble
	spi_write(0x36, 0x2d);  // synchronize word
	spi_write(0x37, 0xd4);
	spi_write(0x38, 0x00);
	spi_write(0x39, 0x00);
	//  spi_write(0x3a, 's');      // set tx header 
	//  spi_write(0x3b, 'o');
	//  spi_write(0x3c, 'n');
	//  spi_write(0x3d, 'g');
	//  spi_write(0x3e, 17);   // total tx 17 byte
	spi_write(0x3f, 's');    // set rx header
	spi_write(0x40, 'o');
	spi_write(0x41, 'n');
	spi_write(0x42, 'g');
	spi_write(0x43, 0xff); // all the bit to be checked
	spi_write(0x44, 0xff); // all the bit to be checked
	spi_write(0x45, 0xff); // all the bit to be checked
	spi_write(0x46, 0xff); // all the bit to be checked
	//  spi_write(0x56, 0x01);
	//  spi_write(0x6d, 0x07); // tx power to Max
	spi_write(0x79, 0x0);   // no frequency hopping
	spi_write(0x7a, 0x0);   // no frequency hopping
	spi_write(0x71, 0x22); // Gfsk, fd[8] =0, no invert for Tx/Rx data, fifo mode, txclk -->gpio
	//  spi_write(0x72, 0x48); // frequency deviation setting to 45k = 72*625
	spi_write(0x73, 0x0);   // no frequency offset 
	spi_write(0x74, 0x0);    // no frequency offset 
	spi_write(0x75, 0x53); // frequency set to 434MHz
	spi_write(0x76, 0x64); // frequency set to 434MHz
	spi_write(0x77, 0x00);// frequency set to 434MHz


};

void delay( int del )
{

	while( del )
	del--;

};


void to_rx_mode( void )
{
	
	to_ready_mode();	
	delay(50000);
	rx_reset();	
	
};

void to_ready_mode( void )
{
	
		spi_write(0x07, 0x01);
	
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
	}
	delay(1000);
	
	SSP_SendData(SSP0, cmd);
	wait_ssp();
	while( LPC_SSP0 -> SR & (1 << 2))
	{
		trash = SSP_ReceiveData(SSP0);
	}

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
	}

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
			readrg[index] = spi_read(0x20);
			index++;
			readrg[index] = spi_read(0x21);
			index++;
			readrg[index] = spi_read(0x22);
			index++;
			readrg[index] = spi_read(0x23);
			index++;
			readrg[index] = spi_read(0x24);
			index++;
			readrg[index] = spi_read(0x25);
			index++;
			readrg[index] = spi_read(0x2A);
			index++;
			readrg[index] = spi_read(0x02);
			index++;
			readrg[index] = spi_read(0x03);
			index++;
			readrg[index] = spi_read(0x04);
			index++;
////	spi_write(0x20, 0xa1);  //clock recovery 
////	spi_write(0x21, 0x20);  //clock recovery
////	spi_write(0x22, 0x4e);  //clock recovery
////	spi_write(0x23, 0xa5);  //clock recovery
////	spi_write(0x24, 0x00);  //clock recovery timing
////	spi_write(0x25, 0x0a);  //clock recovery timing
////	spi_write(0x2a, 0x1e);
////	spi_write(0x2c, 0x29);
////	spi_write(0x2d, 0x04);
////	spi_write(0x2e, 0x29);
////	spi_write(0x09, 0x7f);  // cap = 12.5pf
////	spi_write(0x0a, 0x05);   //clk output is 2MHz
////			
			
			
	};

void wait_ssp( void )
{

		while( LPC_SSP0 -> SR & (1 << 4)){};


};


void EINT3_IRQHandler( void )
{

		int k =0;

};
	
	


