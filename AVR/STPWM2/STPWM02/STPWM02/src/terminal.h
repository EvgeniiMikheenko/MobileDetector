/*
 * terminal.h
 *
 * Created: 11.02.2016 13:39:31
 *  Author: Михеенко Е
 */ 


#ifndef TERMINAL_H_
#define TERMINAL_H_




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
#include <eeprom_store.h>




#define ringbuffsize		160
#define	RX_BUF_SIZE		36
#define GDU		11
#define SET_VAL		0
#define SIP		22
#ifdef DEBUG
#define DEBUG_PRINT( str ) debug_print_str(str , strlen(str));
#else
#define DEBUG_PRINT( str )
#endif
#define MAX_PARAMS	3				// max words in message from terminal
#define MAX_PARAMS_STR_LEN		16
#define kA	Coef[indexA]
#define kB	Coef[indexB]
#define kC	Coef[indexC]
#define kD	Coef[indexD]
#define indexA		0
#define indexB		1
#define indexC		2
#define indexD		3
#define POLYDEG		4			// approximation polynomial degree


void debug_print( uint16_t data );
void debug_print_str( const char *lpStr, int strLen );
void debug_print_u16buf( uint16_t* src, uint8_t size );
void rx_parse( void );




float Coef[POLYDEG];
uint8_t rx_num;
char rx_data_buf[RX_BUF_SIZE];
RingBuffer_t Buffer;
uint8_t      BufferData[ringbuffsize];


#endif /* TERMINAL_H_ */