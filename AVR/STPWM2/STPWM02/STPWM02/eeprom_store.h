/*
 * eeprom_store.h
 *
 * Created: 11.02.2016 16:15:14
 *  Author: Михеенко Е
 */ 


#ifndef EEPROM_STORE_H_
#define EEPROM_STORE_H_



#include <avr/interrupt.h>
#include <avr/io.h>
#include <delay.h>
#include <math.h>
#include <avr/cpufunc.h>
#include <string.h>
#include <RingBuffer.h>
#include <stdbool.h>
#include <stdlib.h>
#include <crc16.h>
#include <avr/eeprom.h>
#include <util/crc16.h>
#include <stdio.h>
#include <terminal.h>



#define EEPROM_DATABUF_SIZE		18
#define EEPROM_ADDR_SHIFT		64
#define EE_COEF_ADDR		0



bool read_eeprom( void );
void write_params( uint8_t mode );



uint8_t EEMEM databuf[EEPROM_DATABUF_SIZE];



#endif /* EEPROM_STORE_H_ */