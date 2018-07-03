#include "main.h"
#define MODE_CONTROL   0
#define MODE_FAILURE   1
#define MODE_READY     2
#define MODE_DISCHARGE 3

#define BEEP_MODE_NORMAL gsa3_flags|= 8
#define BEEP_MODE_ALARM  gsa3_flags&=~8
#define CONTROL_MODE_2   gsa3_flags|= 4

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

#define F_OFF GPIO_SetBits(GPIOC,GPIO_Pin_14)
#define F_ON  GPIO_ResetBits(GPIOC,GPIO_Pin_14)

#define L_OFF GPIO_SetBits(GPIOC,GPIO_Pin_15)
#define L_ON  GPIO_ResetBits(GPIOC,GPIO_Pin_15)

#define A_OFF GPIO_SetBits(GPIOB,GPIO_Pin_2)
#define A_ON GPIO_ResetBits(GPIOB,GPIO_Pin_2)

#define RDY_OFF GPIO_SetBits(GPIOB,GPIO_Pin_10);Params.Flags_Read&=~32
#define RDY_ON GPIO_ResetBits(GPIOB,GPIO_Pin_10);Params.Flags_Read|=32

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

#define CTRL !(GPIOA->IDR&GPIO_Pin_8)


typedef unsigned short ushort;
ushort mode = MODE_CONTROL,control=0; 
ushort Pos_Value1,Pos_Value2,Neg_Value1,Neg_Value2,Temp_Bat;
ushort ticks=0,beep_ticks=0,button_ticks=0,ticks_off=0;
ushort gsa3_flags=0;
/*
bits
0-battery low  1
1-UNUSED       2
2-control2     4
3-beep_mode    8
*/

