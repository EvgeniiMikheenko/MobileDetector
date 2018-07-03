#ifndef MAIN_H 
#define MAIN_H

#include <mb_slave.h>
#include <modbus_config.h>
//#include "mb.h"


//////////////////////////////

#define SET		1
#define RESET	0


#define CH1_IN0(mode)	 		(mode == SET ? HAL_GPIO_WritePin(GPIOA, GPIO_PIN_6, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOA, GPIO_PIN_6, GPIO_PIN_RESET))
#define CH1_IN1(mode)			(mode == SET ? HAL_GPIO_WritePin(GPIOA, GPIO_PIN_7, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOA, GPIO_PIN_7, GPIO_PIN_RESET))

#define CH2_IN0(mode)	 		(mode == SET ? HAL_GPIO_WritePin(GPIOC, GPIO_PIN_4, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOC, GPIO_PIN_4, GPIO_PIN_RESET))
#define CH2_IN1(mode)			(mode == SET ? HAL_GPIO_WritePin(GPIOC, GPIO_PIN_5, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOC, GPIO_PIN_5, GPIO_PIN_RESET))

#define CH3_IN0(mode)	 		(mode == SET ? HAL_GPIO_WritePin(GPIOB, GPIO_PIN_0, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOB, GPIO_PIN_0, GPIO_PIN_RESET))
#define CH3_IN1(mode)			(mode == SET ? HAL_GPIO_WritePin(GPIOB, GPIO_PIN_1, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOB, GPIO_PIN_1, GPIO_PIN_RESET))

#define CH4_IN0(mode)	 		(mode == SET ? HAL_GPIO_WritePin(GPIOB, GPIO_PIN_2, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOB, GPIO_PIN_2, GPIO_PIN_RESET))
#define CH4_IN1(mode)			(mode == SET ? HAL_GPIO_WritePin(GPIOB, GPIO_PIN_12, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOB, GPIO_PIN_12, GPIO_PIN_RESET))

#define LATCH(mode) 			(mode == SET ? HAL_GPIO_WritePin(GPIOB, GPIO_PIN_13, GPIO_PIN_SET): HAL_GPIO_WritePin(GPIOB, GPIO_PIN_13, GPIO_PIN_RESET))







#define FLASH_LOCATION			((uint32_t)0x08008000-FLASH_PAGE_SIZE)			//FLASH_BANK1_END
#define EndAddr					((uint32_t)0x08008000)
#define FLASH_SIZE				(((unsigned short *)&Params.crc - (unsigned short *) & Params.dev_addr)/2+1)
#define CONFIG_REGISTERS_COUNT 	((unsigned short *)&Params.crc - (unsigned short *) & Params.dev_addr)


#define SAVE_CONFIG 	(1 << 0)
#define REINIT_UART		(1 << 1)
#define SET_TIME			(1 << 2)



#endif //MAIN_H