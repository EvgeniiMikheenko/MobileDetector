					   	
#include <stm32f10x.h>
#include <stm32f10x_flash.h>
#include <stm32f10x_rcc.h>
#include <stm32f10x_usart.h>
#include <stm32f10x_gpio.h>
#include <stm32f10x_adc.h>
#include <stm32f10x_tim.h>
#include <stm32f10x_dac.h>
#include <misc.h>
#include "md5.h"
#include "core_cm3.h"

#define MODE_CONTROL   0
#define MODE_FAILURE   1
#define MODE_READY     2
#define MODE_DISCHARGE 3
#define MODE_CONTROL_2   gsa3_flags&4

#define BEEP_MODE_NORMAL  gsa3_flags&=~8
#define BEEP_MODE_ALARM   gsa3_flags|= 8
#define MODE_CONTROL_2_ON    gsa3_flags|= 4
#define MODE_CONTROL_2_OFF    gsa3_flags&=~ 4
#define LED_MODE_AUTO     !(gsa3_flags&32)
#define LED_MODE_AUTO_OFF gsa3_flags|=32
#define LED_MODE_AUTO_ON  gsa3_flags&=~32

#define E11 (Pos_Value1*3300>>16) 
#define E12 (Neg_Value1*3300>>16)
#define E21 (Pos_Value2*3300>>16)
#define E22 (Neg_Value2*3300>>16)

#define P11 Params.Pos_Tresh_Danger
#define P12 Params.Neg_Tresh_Danger
#define P21 Params.Pos_Tresh
#define P22 Params.Neg_Tresh

#define CTR_GSA_ON  GPIO_SetBits(GPIOC,GPIO_Pin_13)
#define CTR_GSA_OFF GPIO_ResetBits(GPIOC,GPIO_Pin_13)

#define F_OFF GPIO_SetBits(GPIOC,GPIO_Pin_15)
#define F_ON  GPIO_ResetBits(GPIOC,GPIO_Pin_15)

#define L_OFF GPIO_SetBits(GPIOC,GPIO_Pin_14)
#define L_ON  GPIO_ResetBits(GPIOC,GPIO_Pin_14)

#define A_OFF GPIO_SetBits(GPIOB,GPIO_Pin_2)
#define A_ON GPIO_ResetBits(GPIOB,GPIO_Pin_2)

#define RDY_OFF GPIO_SetBits(GPIOB,GPIO_Pin_10)//;Params.Flags_Read&=~32
#define RDY_ON GPIO_ResetBits(GPIOB,GPIO_Pin_10)//;Params.Flags_Read|=32

#define DCH_OFF GPIO_SetBits(GPIOB,GPIO_Pin_11)
#define DCH_ON GPIO_ResetBits(GPIOB,GPIO_Pin_11)

#define OFF_OFF GPIO_SetBits(GPIOB,GPIO_Pin_14)
#define OFF_ON GPIO_ResetBits(GPIOB,GPIO_Pin_14)

#define H_OFF GPIO_SetBits(GPIOB,GPIO_Pin_3)
#define H_ON GPIO_ResetBits(GPIOB,GPIO_Pin_3)

#define CTRL_HL_ON  GPIO_SetBits(GPIOB,GPIO_Pin_15)
#define CTRL_HL_OFF GPIO_ResetBits(GPIOB,GPIO_Pin_15)

#define BEEP_ON  GPIO_SetBits(GPIOB,GPIO_Pin_7)
#define BEEP_OFF GPIO_ResetBits(GPIOB,GPIO_Pin_7)

#define CHRG_OFF GPIO_SetBits(GPIOA,GPIO_Pin_11)
#define CHRG_ON  GPIO_ResetBits(GPIOA,GPIO_Pin_11)

#define CTRL !(GPIOA->IDR&GPIO_Pin_8)

#define BS_ON !(GPIOA->IDR&GPIO_Pin_12)


//#define UNIQUE_ID1    ((u16)0x1FFFF7E8)

#define READY_TIMER_VALUE 30*1000 //30 sec

#define DEV_ADC_EDELWEIS   0
#define DEV_ADC_ASTRA   0
#define DEV_ADC_LIMB	   ((330<<16)/3300)
#define DELTA_ADC_DEV	   ((50<<16)/3300)
#define DEV_EDELWEIS	   1
#define DEV_ASTRA		   2
#define DEV_LIMB		   3


#define PWM_PULSE_LIMB		(24000*2)

#define LIMB_TEST_VAL1      (500*PWM_PULSE_LIMB/3300)
#define LIMB_TEST_VAL2      (3000*PWM_PULSE_LIMB/3300)
#define LIMB_TEST_VAL3      (2500*PWM_PULSE_LIMB/3300)
#define LIMB_TEST_VAL4      (2000*PWM_PULSE_LIMB/3300)
#define LIMB_TEST_VAL5      (1500*PWM_PULSE_LIMB/3300)
#define LIMB_TEST_VAL6      (1000*PWM_PULSE_LIMB/3300)

#define LIMB_NONE_VAL    (1000*PWM_PULSE_LIMB/3300)
#define LIMB_PI_VAL      (1500*PWM_PULSE_LIMB/3300)
#define LIMB_PF_VAL      (2000*PWM_PULSE_LIMB/3300)
#define LIMB_PI_PF_VAL   (2500*PWM_PULSE_LIMB/3300)


#define FLAG_FILTER      (Params.Flags_Write&4)

#define FLASH_PAGE_SIZE    ((u16)0x400)
#define FLASH_LOCATION  ((u32)0x08004000-FLASH_PAGE_SIZE)
#define EndAddr    ((u32)0x08004000)
#define FLASH_SIZE (((unsigned short *)&Params.crc - (unsigned short *) & Params)/2+1)


#define DE_ON GPIO_SetBits(GPIOB,GPIO_Pin_12)
#define DE_OFF GPIO_ResetBits(GPIOB,GPIO_Pin_12)

#define RE_ON GPIO_ResetBits(GPIOB,GPIO_Pin_13)
#define RE_OFF GPIO_SetBits(GPIOB,GPIO_Pin_13)

#define READY_ON	Params.Flags_Read|=32;if(Dev_ID==DEV_EDELWEIS)GPIO_SetBits(GPIOA,GPIO_Pin_15)
#define READY_OFF	Params.Flags_Read&=~32;if(Dev_ID==DEV_EDELWEIS)GPIO_ResetBits(GPIOA,GPIO_Pin_15)
#define FAILURE_ON  Params.Flags_Read|=16
#define FAILURE_OFF Params.Flags_Read&=~16

//#define PI_PF_OFF GPIO_ResetBits(GPIOA,GPIO_Pin_11|GPIO_Pin_12)
#define PI_DANGER_OFF (GPIO_ResetBits(GPIOA,GPIO_Pin_12),Params.Flags_Read&=~2)
#define PF_DANGER_OFF (GPIO_ResetBits(GPIOA,GPIO_Pin_11),Params.Flags_Read&=~1)

#define PI_DANGER_ON (GPIO_SetBits(GPIOA,GPIO_Pin_12),GPIO_ResetBits(GPIOA,GPIO_Pin_11),Params.Flags_Read|=2)
#define PF_DANGER_ON (GPIO_SetBits(GPIOA,GPIO_Pin_11),GPIO_ResetBits(GPIOA,GPIO_Pin_12),Params.Flags_Read|=1)

#define PI_TRESH_OFF Params.Flags_Read&=~8
#define PF_TRESH_OFF Params.Flags_Read&=~4

#define PI_TRESH_ON Params.Flags_Read|=8
#define PF_TRESH_ON Params.Flags_Read|=4


#define MAX_PARAMS 10000

#define MB_MAXPARAMS  MAX_PARAMS

#define MB_MAX_INPUT_PARAMS 64



//#define MB_SENDPAUSE  20
#define MB_SENDPAUSE  20
#define MB_RCVPAUSE   20
#define MB_SL_TIMEOUT 3000
#define MB_SLAVE_BUFSIZE 1024
#define MB_SEND_MODE  1
#define MB_RECEIVE_MODE 0
#define UART_RDA  0x4
#define UART_CTI  0xC
#define UART_RLS  0x6
#define UART_THRE  0x2



#define MB_SOURCE 1
#define MB_ADDR 1

#define MB_RBR ((MB_SOURCE)?RS485_RBR:RS232_RBR)

#define RS485_IIR USART1->SR
#define RS485_LSR USART1->SR

#define RS232_IIR USART1->SR
#define RS232_LSR USART1->SR

#define RS485_THR USART1->DR
#define RS232_THR USART1->DR

#define RS485_RBR USART1->DR
#define RS232_RBR USART1->DR



#define MB_LSR ((MB_SOURCE)?RS485_LSR:RS232_LSR)

