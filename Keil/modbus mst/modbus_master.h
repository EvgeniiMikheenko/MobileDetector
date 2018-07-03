
#include <LPC17xx.H>
#include <system_LPC17xx.h>
#include <lpc17xx_pinsel.h>
#include <lpc17xx_uart.h>
#include <string.h>
#include <stdbool.h>
#include <stdio.h>
#include <lpc17xx_timer.h>

#include "modbs_master.c"




uint8_t* Slave_request(uint8_t* buf, uint8_t buf_len, uint8_t addr, uint8_t func, uint16_t reg_start, uint8_t reg_num);

unsigned short CRC16_mbm(unsigned char *nData, unsigned short wLength);



void TInit( void );
void TStart( void );