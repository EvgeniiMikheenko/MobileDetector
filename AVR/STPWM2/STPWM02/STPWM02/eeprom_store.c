/*
 * eeprom_store.h
 *
 * Created: 11.02.2016 16:15:32
 *  Author: Михеенко Е
 */ 


#include <eeprom_store.h>



bool read_eeprom( void )
{
	delay_ms(200);
	cli();
	uint8_t tmpbuf[EEPROM_DATABUF_SIZE];
	eeprom_busy_wait();
	eeprom_read_block((void *)tmpbuf, (const void *)(databuf + EEPROM_ADDR_SHIFT), EEPROM_DATABUF_SIZE);		//Read 18 bits from *databuf + EEPROM_ADDR_SHIFT eeprom address

	uint16_t crc = 0;
	for(int i =0; i < EEPROM_DATABUF_SIZE - 2; i++)						//Crc calculation
	{
		crc = _crc16_update( crc, tmpbuf[i]);
	}

	uint8_t bufss[4];
	for(int j = 0; j < EEPROM_DATABUF_SIZE - 2; j += 4)				// Get float val from eeprom
	{
		for( int i = 0 ; i < 4; i++)
		{
			bufss[i] = tmpbuf[i+j];
		}
		float *ptrf = (float *)bufss;
		Coef[(int)j/4] = *ptrf;
	}
	//
	
	if( crc == (((uint16_t)tmpbuf[EEPROM_DATABUF_SIZE-1] << 8) + (uint16_t)tmpbuf[EEPROM_DATABUF_SIZE-2]) )		//Check read data and read crc
	{
		DEBUG_PRINT("\ncrc ok\n");
		sei();
		return true;
	}
	else														// Read data is incorrect
	{
		DEBUG_PRINT("\n\rwrite params\n\r");					//Write GDU coef
		write_params(GDU);
		
	}
	sei();
	return false;
}


void write_params( uint8_t mode )			//Calculate crc and write params with crc to eeprom
{
	cli();

	if(mode == GDU)
	{
		kA = 348.49;
		kB = -2168.1;
		kC = 4625.9;
		kD = -3154.9;
		
		
		DEBUG_PRINT("\n\rGDU def coef is set\n\r")
	}
	if(mode == GDU)
	{
		kA = 0;
		kB = 476.43;
		kC = -668.44;
		kD = 236.04;
		DEBUG_PRINT("\n\rSIP def coef is set\n\r")
	}
	
	if(mode == SET_VAL)
	{
		DEBUG_PRINT("\n\rcoef is set\n\r")
	}

	
	uint8_t *ptr8 = ( uint8_t * )Coef;
	eeprom_busy_wait();
	eeprom_update_block ((const uint8_t *)Coef, (void *)(databuf+EEPROM_ADDR_SHIFT), EEPROM_DATABUF_SIZE - 2);
	
	uint16_t crc = 0;
	for(int i =0; i < EEPROM_DATABUF_SIZE - 2; i++)
	{
		crc = _crc16_update( crc, *ptr8++);
	}
	eeprom_busy_wait();
	eeprom_write_word( (uint16_t *)(databuf+16+EEPROM_ADDR_SHIFT), crc);
	sei();
}
