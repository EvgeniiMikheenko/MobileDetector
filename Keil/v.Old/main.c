//==============================================================================
//
//==============================================================================

#include "main.h"

//------------------------------------------------------------------------------

//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
MD5_CTX context;
unsigned char digest[16];

typedef unsigned short ushort;
ushort mode = MODE_CONTROL,control=0,last_step = 0; 
ushort debug[4];
ushort Pos_Value1,Pos_Value2,Neg_Value1,Neg_Value2,Temp_Bat;
ushort ticks=0,beep_ticks=0,button_ticks=0,ticks_off=0,ticks_charge=0;
const ushort Tarr[15] = {0x262,0x32F,0x43C,0x7FF,0xC3E,0xCF0,0xD8C,0xE11,0xE80,0xF24,0xF5C,0xF88,0xFA9,0xFC2,0xFCC};
//temperature             70    60    50    25    0     -5    -10   -15   -20   -30   -35   -40   -45   -50   -52
ushort gsa3_flags=0;
/*
bits
0-battery low  1
1-UNUSED       2
2-control2     4
3-beep_mode    8
4-control_1    16
5-led_mode     32
*/

ushort debug1=0,debug2=0;

ushort init=1;
int mb_addr, mb_sl_bytecount = 0, mb_sl_timer = 0, mb_sl_datasize = 0;
signed char mb_sl_mode = MB_RECEIVE_MODE, uart_free = 1, flash_write = 0, check4flash_write = 0, work_enable = 0, triac_on = 0, triac_on_prev = 0, Tset_in_flag = 0, Tallow_Tset_update = 1, Tdiff_zone = 0, Tdiff_zone_prev = 0;
unsigned mb_sl_timeout = 0;
int mb_read_flag = 0;
unsigned char mb_slave_buf[MB_SLAVE_BUFSIZE];

unsigned short mb_input_params[MB_MAX_INPUT_PARAMS + 1];

#define mb_addr mb_slave_buf[0]
#define mb_func mb_slave_buf[1]



unsigned *UNIQUE_ID1 =(unsigned*)0x1FFFF7E8;
unsigned *UNIQUE_ID2 =(unsigned*)0x1FFFF7EC;
	
typedef struct
{
  unsigned short Flags_Write;
  unsigned short HV_Value;
  unsigned short Freq;
  unsigned short HV_Offset;
  unsigned short Pos_Tresh;
  unsigned short Neg_Tresh;
  unsigned short Set_Null;
  unsigned short Pos_Tresh_Danger;
  unsigned short Neg_Tresh_Danger;
  unsigned short Filter_Level;
  unsigned short src_num[2];//
  unsigned short Date[3];
  unsigned short Set_Null2;
  unsigned short crc;
  unsigned short Dummy[13];
  
  unsigned short Flags_Read;
  unsigned short Pos_Value;
  unsigned short Neg_Value;
  unsigned short HV;
  unsigned short Ass;
  unsigned short Temp_Int;
  unsigned short ID1;
  unsigned short ID2;
  unsigned short ID3;
  unsigned short ID4;
  unsigned short ID5;
  unsigned short ID6;
  unsigned short hash[8];
  unsigned short ver;
//   unsigned short Pos2_Value;
//   unsigned short Neg2_Value;
  unsigned short Dummy1[9];
} Params_Struct;
Params_Struct Params;

unsigned short * PARAMS_BUF = (unsigned short *) & Params;
unsigned short * FLASH_PARAMS_BUF = (unsigned short *) FLASH_LOCATION;

Params_Struct * Flash_Params = (Params_Struct *) FLASH_LOCATION;

/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/  
typedef enum { FAILED = 0, PASSED = !FAILED} TestStatus;
u32 EraseCounter = 0x00, Address = 0x00;
u32 Data;
vu32 NbrOfPage = 0x00;
volatile FLASH_Status FLASHStatus;
volatile TestStatus MemoryProgramStatus;
ErrorStatus HSEStartUpStatus;

unsigned * BUF1 = (unsigned *) &Params;
unsigned * BUF2 = (unsigned *) FLASH_LOCATION;

char in_byte;
int adc_channel=0;
int Pos_Value_int=0;
int Neg_Value_int=0;
int Pos_Value_int2=0;
int Neg_Value_int2=0;
int Ehv_Value_int=0;
int Eass_Value_int=0;
int Temp_Value_int=0;
int Temp_Bat_int=0;
int Dev_ID=0;
int DTI_1 = 0,DTI_2 = 0,DtT = 0; 
int ready_timer=0;

USART_InitTypeDef USART_InitStructure;
GPIO_InitTypeDef GPIO_InitStructure;
ADC_InitTypeDef ADC_InitStructure;

TIM_TimeBaseInitTypeDef  TIM_TimeBaseStructure;
TIM_OCInitTypeDef  TIM_OCInitStructure;
u16 CCR1_Val = 32768;
u16 CCR2_Val = 375;
u16 CCR3_Val = 250;
u16 CCR4_Val = 125;
int i;
char eg=0;//ebanaya golovka suka
char egc=0;//eg counter
//unsigned short debug1=0, debug2=0; 
//------------------------------------------------------------------------------

int main()
{
#ifdef newadc
		  //ADC_RegularChannelConfig(ADC1, ADC_Channel_4, 1, ADC_SampleTime_239Cycles5);
#endif
	
#ifndef NDEBUG
 	DBGMCU->CR = 1 << 10;
#endif
	
	MD5_Calc();
	Params.ver = 0x0300;
	RCC_Configuration();

	RCC_APB2PeriphClockCmd(RCC_APB2Periph_TIM1|RCC_APB2Periph_TIM16|RCC_APB2Periph_TIM17| RCC_APB2Periph_USART1, ENABLE);
  	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM2 |RCC_APB1Periph_TIM3, ENABLE);
  	RCC_APB2PeriphClockCmd(RCC_APB2Periph_ADC1 | RCC_APB2Periph_GPIOA| RCC_APB2Periph_GPIOB| RCC_APB2Periph_GPIOC|RCC_APB2Periph_AFIO, ENABLE);

  	AFIO->MAPR|=AFIO_MAPR_TIM2_REMAP_PARTIALREMAP1|AFIO_MAPR_TIM3_REMAP_PARTIALREMAP|AFIO_MAPR_SWJ_CFG_JTAGDISABLE;
	pInit();
#ifdef DEBUG
  	debug();
#endif
	
  
	while(	ADC_GetCalibrationStatus(ADC1)	);

	Params.HV_Value=3000;
	Params.Freq=4000;
	Params.HV_Offset=32768;
	Params.Set_Null=32768;
	FlashRestore();
	Params.Flags_Read=0;
	Params.ID1=*UNIQUE_ID1;
	Params.ID2=(*UNIQUE_ID1)>>16;
	Params.ID3=(*UNIQUE_ID2);

	NVIC_Configuration();
  	while (1)
  	{
 		if(!init)
 			GSA3();
 		else 
				ticks=0;
		__WFI();
		TIM3->CCR1=128-128*(Params.HV_Offset-32768)/90; //Cont2Ass	
		//eiaa?ney ?aaoee?iaee ioey aua eiaa?oe?iaaiu aoiau
		TIM16->CCR1=256-(128+128*(Params.Set_Null-32768)/200); //Set_Null
		TIM2->CCR1=256-(128+128*(Params.Set_Null2-32768)/200); //Set_Null

		if (init==0)
		{
			if ((Neg_Value1*3300>>16)>Params.Neg_Tresh_Danger){
				PI_DANGER_ON;
			}
			else{
				PI_DANGER_OFF;
			}
	  
			if ((Pos_Value1*3300>>16)>Params.Pos_Tresh_Danger){
				PF_DANGER_ON;
			}
			else{
				PF_DANGER_OFF;
			}

			if ((Neg_Value2*3300>>16)>Params.Neg_Tresh){
				PI_TRESH_ON;
			}else{
				PI_TRESH_OFF;
			}
			
			if ((Pos_Value2*3300>>16)>Params.Pos_Tresh){
				PF_TRESH_ON;
			}
			else{
				PF_TRESH_OFF;
			}
  
		}

		if(Params.Flags_Write&16)
		{
			Params.Pos_Value = Pos_Value2;
			Params.Neg_Value = Neg_Value2;
		}
		else
		{
			Params.Pos_Value = Pos_Value1;
			Params.Neg_Value = Neg_Value1;
		}
		
	  	if (Params.Flags_Write&32 || (mode == MODE_CONTROL && control>0 && control <6))
		  CTR_GSA_ON;
	  	else
		  CTR_GSA_OFF;

		if (Params.Flags_Write&2) 
			mode = MODE_READY ;

		if (Params.Flags_Write&1)  
		{
					__disable_irq();
					__disable_fiq();
					FlashStore();
					__enable_irq();
					__enable_fiq();
					Params.Flags_Write&=~0x0001;
		}

	  //i=TIM17->CCER;					  
	  
	  
	  //TIM3->CCR2=2*Params.HV_Value*256/10/200/3.3; //2*Um

	  TIM3->CCR2=1+256*Params.HV_Value/3350; //2*Um


	  if((Params.Freq>499)&&(Params.Freq<8001))
	  {
		  TIM17->ARR=10*24000000/(Params.Freq*2)/4;  //2*Freq
		  TIM17->CCR1=TIM17->ARR/2;
		//  TIM16->ARR=10*24000000/(Params.Freq*2)/4;  //2*Freq
		//  TIM16->CCR1=TIM17->ARR/2;

	  }	
  


  

  }
}

//------------------------------------------------------------------------------
// Private

void FlashStore( void ) 
{
  int i,j;
  FLASHStatus = FLASH_COMPLETE;
  MemoryProgramStatus = PASSED;
  if((BUF2[0]&8)&&((BUF2[0]&0xFF)!=0xFF)) return;
  j=0;
  for (i=0;i<FLASH_SIZE;i++)
    if (BUF1[i]!=BUF2[i]) j=1;

  if (!j) return;
  
  FLASH_Unlock();
  NbrOfPage = (EndAddr - FLASH_LOCATION) / FLASH_PAGE_SIZE;
  
  FLASH_ClearFlag(FLASH_FLAG_BSY | FLASH_FLAG_EOP | FLASH_FLAG_PGERR | FLASH_FLAG_WRPRTERR);	
  
  for(EraseCounter = 0; (EraseCounter < NbrOfPage) && (FLASHStatus == FLASH_COMPLETE); EraseCounter++)
  {
    FLASHStatus = FLASH_ErasePage(FLASH_LOCATION + (FLASH_PAGE_SIZE * EraseCounter));
  }
  
  
  Address = FLASH_LOCATION;
  i=FLASH_SIZE;

  Params.crc = CRC16((unsigned char *) & Params, FLASH_SIZE*4-4);
  for (i=0;i<FLASH_SIZE;i++)
  if (BUF1[i]!=BUF2[i])
  FLASH_ProgramWord(FLASH_LOCATION+(i<<2), BUF1[i]);
}

int FlashRestore( void ) {
  int i;
//FIXME:crc not equals
  if (CRC16((unsigned char *) Flash_Params, FLASH_SIZE*4-4) != Flash_Params->crc)
  {
    for (i = 0; i < FLASH_SIZE ; i++)
      PARAMS_BUF[i] = 0;
    return 0;
  }
  else
  {
    for (i = 0; i < FLASH_SIZE  ; i++)
      BUF1[i] = BUF2[i];
    return 1;
  }
}

void RCC_Configuration( void ) {
  /* RCC system reset(for debug purpose) */
  RCC_DeInit();
  

  /* Enable HSE */
//  ..RCC_HSEConfig(RCC_HSE_ON);

  /* Wait till HSE is ready */
  //HSEStartUpStatus = RCC_WaitForHSEStartUp();

  //if(HSEStartUpStatus == SUCCESS)
  {
    /* Enable Prefetch Buffer */
    FLASH_PrefetchBufferCmd(FLASH_PrefetchBuffer_Enable);

    /* Flash 2 wait state */
    FLASH_SetLatency(FLASH_Latency_2);
  
    /* HCLK = SYSCLK */
    RCC_HCLKConfig(RCC_SYSCLK_Div1); 
  
    /* PCLK2 = HCLK */
    RCC_PCLK2Config(RCC_HCLK_Div1); 

    /* PCLK1 = HCLK/2 */
    RCC_PCLK1Config(RCC_HCLK_Div2);

    /* ADCCLK = PCLK2/4 */
    RCC_ADCCLKConfig(RCC_PCLK2_Div4); 
  
    /* PLLCLK = 8MHz * 7 = 56 MHz */
    RCC_PLLConfig(RCC_PLLSource_HSI_Div2,RCC_PLLMul_6);

    /* Enable PLL */ 
    RCC_PLLCmd(ENABLE);

    /* Wait till PLL is ready */
    while(RCC_GetFlagStatus(RCC_FLAG_PLLRDY) == RESET)
    {
    }

    /* Select PLL as system clock source */
    RCC_SYSCLKConfig(RCC_SYSCLKSource_PLLCLK);

    /* Wait till PLL is used as system clock source */
    while(RCC_GetSYSCLKSource() != 0x08)
    {
    }
  }
}

void NVIC_Configuration( void ) {
  NVIC_InitTypeDef NVIC_InitStructure;

  #ifdef  VECT_TAB_RAM
	  /* Set the Vector Table base location at 0x20000000 */
	  NVIC_SetVectorTable(NVIC_VectTab_RAM, 0x0);
  #else  /* VECT_TAB_FLASH  */
	  /* Set the Vector Table base location at 0x08000000 */
	  NVIC_SetVectorTable(NVIC_VectTab_FLASH, 0x0);
  #endif


  /* Enable the USART1 Interrupt */
  NVIC_InitStructure.NVIC_IRQChannel = USART1_IRQn;
  NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
  NVIC_InitStructure.NVIC_IRQChannelSubPriority = 1;
  NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
  NVIC_Init(&NVIC_InitStructure);

  /* Enable the TIM2 Interrupt */
  NVIC_InitStructure.NVIC_IRQChannel = TIM1_UP_IRQn ;
  NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
  NVIC_InitStructure.NVIC_IRQChannelSubPriority = 1;
  NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
  NVIC_Init(&NVIC_InitStructure);

}

void Control( void ) {
	debug1=E21;
	debug2=E22;
  switch(control)
  {
    case 0:
		LED_MODE_AUTO_OFF;
		DCH_OFF;
		RDY_OFF;
		if (ticks>500)
		{
			control++;
			ticks=0;
		}
		else
		{
			if((ticks%100)<50)
			{
				F_ON;
				L_ON;
				A_ON;
				RDY_ON;
				H_ON;
				CTRL_HL_ON;
				DCH_ON;
			}
			else
			{
				F_OFF;
				L_OFF;
				A_OFF;
				RDY_OFF;
				H_OFF;
				CTRL_HL_OFF;
				DCH_OFF;
			}	
	   }						
	break;
    case 1:
	  LED_MODE_AUTO_ON;
      if(ticks<300)
      {
		CTR_GSA_ON;
        if(gsa3_flags&1) mode = MODE_DISCHARGE;
		if( ticks>10){
			if(E22>P22){
					gsa3_flags|=16;
			}
		}
      }
      else{
		if( gsa3_flags&16){
          control++;
		  gsa3_flags&=~16;
		}
		else
			mode = MODE_FAILURE;
	  }
      break;
    case 2:
      if(E11<P11 && E12>P12 && E21<P21 && E22>P22)
        control++;
      else mode = MODE_FAILURE;
      break;
    case 3:
		//i?ioo i?inoeou iaiy ca yoio aeeee iecaao
		if(eg!=20)
		{
			if(E21>P21) 
				control++;
			if(ticks>1000) 
			{
				if(E21>P21)
					control++;
				else 
					mode = MODE_FAILURE;
			}
			else if ((ticks%100)>98)
			{
			  eg++;
				if(E22<P22 && E21<P21) 
						egc++;
				else 
						egc=0;
				if (egc>=5 && E21<P21)
				{
					eg=20;
				}
			}
		}
		else
			if(ticks<1500)
			{
				LED_MODE_AUTO_OFF;
				A_ON;		
				H_OFF;					
			}
			else 
			{
				if(E21>P21) 
				  control++;
				else{
					control=6;
				  CTR_GSA_OFF;
				  ticks=0;
				}
				LED_MODE_AUTO_ON;
				eg=0;
				egc=0;
			}
      break;
    case 4:
      if(ticks<1500) 
      {
        if(!(E11<P11 && E12>P12 && E21>P21 && E22<P22))
          mode = MODE_FAILURE;;
      }
      else 
        control++;
      break;
    case 5:
      if(E11<P11 && E12>P12 && E21<P21 && E22<P22)
      {
        control++;
        ticks=0;
        CTR_GSA_OFF;
      }
      else
        if (ticks>11000) 
		{
			if(E11<P11 && E12>P12 && E21>P21 && E22<P22)
			{
				LED_MODE_AUTO_OFF;
				A_OFF;
				control++;
				ticks=0;
				CTR_GSA_OFF;
			}
			else  mode = MODE_FAILURE;
		}
     break;
    case 6:
      if (E12<P12)
      {
		LED_MODE_AUTO_ON;
        mode = MODE_READY;
        READY_ON;
				//control = 0;
        CTRL_HL_OFF;
		MODE_CONTROL_2_OFF;
      }
      else
        if (ticks>700)
          mode = MODE_FAILURE;
      break;
  }
	last_step = control;
}

void Beep( void ) {
  if (mode==MODE_DISCHARGE) return;
  if(!(gsa3_flags & 8))
  {
    if(beep_ticks<99)
      BEEP_OFF;
    else{if(beep_ticks>98)beep_ticks=0;BEEP_ON;}
  }
  else 
  {
    if(beep_ticks<40)
      BEEP_OFF;
    else{if(beep_ticks>60)beep_ticks=0;BEEP_ON;}
  }
}

void GSA3 ( void ) {
  switch (mode)
  {
    case MODE_CONTROL:
      Control();
      break;
    case MODE_DISCHARGE:
      DCH_ON;
      if((beep_ticks>0 && beep_ticks<3)||(beep_ticks>30 && beep_ticks<33))
        BEEP_ON;
      else
        BEEP_OFF;
      if (ticks_off>3000) 
        OFF_ON;
      if (beep_ticks>100) 
        beep_ticks=0;
      break;
    case MODE_FAILURE:
	  if (!(Params.Flags_Write&32))
		CTR_GSA_OFF;
	  BEEP_MODE_ALARM;
      if(ticks<40)
        CTRL_HL_ON;
      else
      {
        if(ticks>=60)
          ticks=0;
        CTRL_HL_OFF;
      }
    break;
    case MODE_READY:
      READY_ON;
      if (Params.Flags_Read&15)
        BEEP_MODE_ALARM;
      else
        BEEP_MODE_NORMAL;
    break;
  }
}

void MD5_Calc( void ) {
	unsigned char *buffer;
  	int trbr=0;
  	buffer = (unsigned char*)0x08000000;
  
  	MD5Init( &context );
  	MD5Update( &context, buffer, (0x0801FFFF-0x08000000));
  	MD5Final( digest, &context);
  	while(trbr<8){
    	Params.hash[trbr] = (unsigned short)digest[trbr*2]<<8|(unsigned short)digest[trbr*2+1];	//2 char in 1 ushort
    	trbr++;
  	}
}

void pInit( void ) {
	GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_8|GPIO_Pin_12;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	SysTick_Config(240000);
	
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11|GPIO_Pin_12;
  GPIO_InitStructure.GPIO_Mode =  GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed =  GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3|GPIO_Pin_4|GPIO_Pin_5|GPIO_Pin_6;
  GPIO_InitStructure.GPIO_Mode =  GPIO_Mode_AIN;
  GPIO_InitStructure.GPIO_Speed =  GPIO_Speed_2MHz; 
  GPIO_Init(GPIOA, &GPIO_InitStructure);

  /* Configure USART1 Tx (PA.09) as alternate function push-pull */
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(GPIOA, &GPIO_InitStructure);

  /* Configure USART1 Rx (PA.10) as input floating */
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_Init(GPIOA, &GPIO_InitStructure);



  /*GPIOA Configuration: TIM3 channel 1 and 2 as alternate function push-pull */
  //GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_7;
  //GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  //GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;

  //GPIO_Init(GPIOA, &GPIO_InitStructure);


  

  GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_3|GPIO_Pin_4|GPIO_Pin_5|GPIO_Pin_6|GPIO_Pin_9;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_Init(GPIOB, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_12|GPIO_Pin_13|GPIO_Pin_14|GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_Init(GPIOB, &GPIO_InitStructure);
  
  GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_13;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_OD;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOC, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOC, &GPIO_InitStructure);
	
//GSA-3M
  GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_2|GPIO_Pin_3|GPIO_Pin_7|GPIO_Pin_10|GPIO_Pin_11|GPIO_Pin_14|GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOB, &GPIO_InitStructure);
	
	GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_13|GPIO_Pin_14|GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOC, &GPIO_InitStructure);
	
	GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_0|GPIO_Pin_1;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOB, &GPIO_InitStructure);
  
#ifdef newadc
  	GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_0|GPIO_Pin_1|GPIO_Pin_7;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
#endif
  
	
	GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_2|GPIO_Pin_3;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_11;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_12;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	GPIO_InitStructure.GPIO_Pin =   GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	TIM_TimeBaseStructure.TIM_Period = 256;
  TIM_TimeBaseStructure.TIM_Prescaler = 0;
  TIM_TimeBaseStructure.TIM_ClockDivision = 0;
  TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;
  TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);
	
	TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
  TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
  TIM_OCInitStructure.TIM_Pulse = 128;
  TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;
  TIM_OC1Init(TIM2, &TIM_OCInitStructure);
  TIM_OC1PreloadConfig(TIM2, TIM_OCPreload_Enable);

  TIM2->BDTR=(1<<15) ;
  TIM2->CCER=13;					  

  TIM_Cmd(TIM2, ENABLE);	
  

    
  /* ADC1 configuration ------------------------------------------------------*/
/* ADC1 Configuration ------------------------------------------------------*/
  /* ADC1 and ADC2 operate independantly */
  ADC_InitStructure.ADC_Mode = ADC_Mode_Independent;
  /* Disable the scan conversion so we do one at a time */
  ADC_InitStructure.ADC_ScanConvMode = DISABLE;
  /* Don't do contimuous conversions - do them on demand */
  ADC_InitStructure.ADC_ContinuousConvMode = DISABLE;
  /* Start conversin by software, not an external trigger */
  ADC_InitStructure.ADC_ExternalTrigConv = ADC_ExternalTrigConv_None;
  /* Conversions are 12 bit - put them in the lower 12 bits of the result */
  ADC_InitStructure.ADC_DataAlign = ADC_DataAlign_Left;
  /* Say how many channels would be used by the sequencer */
  ADC_InitStructure.ADC_NbrOfChannel = 1;
  ADC1->CR2|=(1<<23);

  /* Now do the setup */
  ADC_Init(ADC1, &ADC_InitStructure);
  /* Enable ADC1 */
  ADC_Cmd(ADC1, ENABLE);

  /* Enable ADC1 reset calibaration register */
  ADC_ResetCalibration(ADC1);
  /* Check the end of ADC1 reset calibration register */
  while(ADC_GetResetCalibrationStatus(ADC1));
  /* Start ADC1 calibaration */
  ADC_StartCalibration(ADC1);
  /* Check the end of ADC1 calibration */
  

  /* TIM2 configuration */
  TIM_TimeBaseStructure.TIM_Period = 24000*2;          
  //TIM_TimeBaseStructure.TIM_Period = 65000;          
  TIM_TimeBaseStructure.TIM_Prescaler = 0;       
  TIM_TimeBaseStructure.TIM_ClockDivision = 0x0;    
  TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;  
  TIM_TimeBaseStructure.TIM_RepetitionCounter = 0x0000;
  TIM_TimeBaseInit(TIM1, &TIM_TimeBaseStructure);
  
//   //TIM_OCStructInit(&TIM_OCInitStructure);
//   /* Output Compare Timing Mode configuration: Channel1 */
//   //TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_Timing;                   
//   //TIM_OCInitStructure.TIM_Pulse = 0x0;  
//   //TIM_OC1Init(TIM2, &TIM_OCInitStructure);
//   TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
//   TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
//   TIM_OCInitStructure.TIM_Pulse = 12000;
//   TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;

//   TIM_OC2Init(TIM2, &TIM_OCInitStructure);

//   TIM_OC2PreloadConfig(TIM2, TIM_OCPreload_Enable);



  TIM1->DIER=1;
    TIM_Cmd(TIM1, ENABLE);



  TIM_TimeBaseStructure.TIM_Period = 24000;
  TIM_TimeBaseStructure.TIM_Prescaler = 3;
  TIM_TimeBaseStructure.TIM_ClockDivision = 0;
  TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;

//

  TIM_TimeBaseInit(TIM17, &TIM_TimeBaseStructure);
  


  TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
  TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
  TIM_OCInitStructure.TIM_Pulse = 0;
  TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;

  TIM_OC1Init(TIM17, &TIM_OCInitStructure);

  TIM_OC1PreloadConfig(TIM17, TIM_OCPreload_Enable);

  TIM17->BDTR=(1<<15) ;
  TIM_Cmd(TIM17, ENABLE);




  TIM_TimeBaseStructure.TIM_Period = 256;
  TIM_TimeBaseStructure.TIM_Prescaler = 0;
  TIM_TimeBaseStructure.TIM_ClockDivision = 0;
  TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;
  
  TIM_TimeBaseInit(TIM16, &TIM_TimeBaseStructure);


  TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
  TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
  TIM_OCInitStructure.TIM_Pulse = 128;
  TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;

  TIM_OC1Init(TIM16, &TIM_OCInitStructure);

  TIM_OC1PreloadConfig(TIM16, TIM_OCPreload_Enable);

  TIM16->BDTR=(1<<15) ;
  TIM16->CCER=13;					  

  TIM_Cmd(TIM16, ENABLE);	 



  TIM_TimeBaseStructure.TIM_Period = 256;
  TIM_TimeBaseStructure.TIM_Prescaler = 0;
  TIM_TimeBaseStructure.TIM_ClockDivision = 0;
  TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;

  TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
//  TIM_TimeBaseInit(TIM16, &TIM_TimeBaseStructure);


  TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
  TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
  TIM_OCInitStructure.TIM_Pulse = 128;
  TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;

  TIM_OC2Init(TIM3, &TIM_OCInitStructure);
  //TIM_OC1Init(TIM16, &TIM_OCInitStructure);

  TIM_OC2PreloadConfig(TIM3, TIM_OCPreload_Enable);
//  TIM_OC1PreloadConfig(TIM16, TIM_OCPreload_Enable);



  TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
  TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
  TIM_OCInitStructure.TIM_Pulse = 10;
  TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;

  TIM_OC1Init(TIM3, &TIM_OCInitStructure);


  TIM_OC1PreloadConfig(TIM3, TIM_OCPreload_Enable);
  



  TIM_Cmd(TIM3, ENABLE);

//   TIM16->BDTR=(1<<15) ;

//   TIM_Cmd(TIM16, ENABLE);




  /*
  TIM17->ARR=999;
  TIM17->PSC=0;
  //TIM17->RCR=0;
  TIM17->EGR = TIM_PSCReloadMode_Immediate;
  TIM17->CR2 = 0;
  TIM17->CCMR1 =0x70 ;
  //TIM17->BDTR=1<<15;   
  TIM17->CCR1 = 500; 
  TIM17->CCR2 = 375; 
  TIM17->CR1=0x1;   
  //TIM17->CCER = ;
  */

	DE_OFF;
	RE_ON;
	READY_OFF;

  USART_InitStructure.USART_BaudRate = 115200;
  USART_InitStructure.USART_WordLength = USART_WordLength_8b;
  USART_InitStructure.USART_StopBits = USART_StopBits_1;
  USART_InitStructure.USART_Parity = USART_Parity_No;
  USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
  USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;

  USART_Init(USART1, &USART_InitStructure);

  USART_ITConfig(USART1, USART_IT_TXE, ENABLE);

  USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);
  USART_Cmd(USART1, ENABLE); 
	
  /*	
  USART1->CR1=(1<<13)|(1<<7)|(1<<5)|(1<<3)|(1<<2);
  USART1->CR3=(1<<11);
  USART1->BRR=0xD0;  
  */
}

//------------------------------------------------------------------------------
// Interrupts

void USART1_IRQHandler( void ) {
  unsigned int stat, crc, i;
  unsigned int startaddr, count;
  char         stat_rs485; 
  //char         stat_rs232, mb_lsr; 

  stat_rs485 = (RS485_IIR);

  if ((stat_rs485&0x80)) 
  stat=0x2; 

  if (stat_rs485&0x8) i=MB_RBR; 
  if (stat_rs485&0x20) stat=0x4; 
  stat_rs485 = (USART1->CR1);

  switch (stat)
  {
    case 0x2:
      if ((MB_LSR & 0x80)  && (mb_sl_bytecount < mb_sl_datasize))
      {
		RS485_THR = mb_slave_buf[mb_sl_bytecount++];
      }
      if (mb_sl_bytecount == mb_sl_datasize)
      {
		mb_sl_timer = MB_RCVPAUSE; // ??
		mb_sl_mode = MB_RECEIVE_MODE;
		mb_sl_bytecount = 0;
		mb_sl_datasize = 0;
		USART_ITConfig(USART1, USART_IT_TXE, DISABLE);	
      }
      break;
    case 0xC:
    case 0x4:
      in_byte = MB_RBR;
     switch (mb_sl_bytecount)
     {
		case 0:
	  		if (in_byte == MB_ADDR)
	    	mb_addr = in_byte;
	  else
	    mb_sl_bytecount = - 1;
	  break;
	  case 1:
	  mb_func = in_byte;
	  if ((mb_func != 4) && (mb_func != 3) && (mb_func != 15) && (mb_func != 16) && (mb_func != 6) && (mb_func != 1) && (mb_func != 2) && (mb_func != 5))
	    mb_sl_bytecount = - 1;
	  else
	    mb_sl_datasize = 8;
	  break;
	  default:
	  mb_slave_buf[mb_sl_bytecount] = in_byte;
	  if ((mb_sl_bytecount == 6) && ((mb_func == 16) || (mb_func == 15)))
	    mb_sl_datasize = 9 + mb_slave_buf[6];
	  if (mb_sl_datasize - 2 < mb_sl_bytecount)
	  {
	    mb_sl_bytecount = - 1;
	    if (CRC16((unsigned char *) & mb_slave_buf, mb_sl_datasize))
	    {
	      mb_sl_datasize = 0;
	      break;
	    }
	    RE_OFF;
	    USART_ITConfig(USART1, USART_IT_RXNE, DISABLE);
	    startaddr = mb_slave_buf[3] + (mb_slave_buf[2] << 8);
	    count = mb_slave_buf[5] + (mb_slave_buf[4] << 8);
	     switch (mb_func)
	     {
	      case 5:
			if ((startaddr > 7777) || mb_sl_timer)
			{
			  mb_func = 0x85;
			  mb_slave_buf[2] = 2;
			  mb_sl_datasize = 3;
			}
			else
			{
			  mb_sl_datasize = 6;
			  i = (mb_slave_buf[5] + (mb_slave_buf[4] << 8));
			}
		  break;
	      case 6:
			if (((startaddr) > MB_MAXPARAMS - MB_MAX_INPUT_PARAMS) || mb_sl_timer)
			{
			  mb_func = 0x86;
			  mb_slave_buf[2] = 2;
			  mb_sl_datasize = 3;
			}
			else
			{
			  mb_sl_datasize = 6;
			  PARAMS_BUF[startaddr] = mb_slave_buf[5] + (mb_slave_buf[4] << 8);
			}
		  break;
	      case 3:
			if (((startaddr + count) > MB_MAXPARAMS) || ((startaddr + count) < 1) || mb_sl_timer)
			{
			  mb_func = 0x80 + 3;
			  mb_slave_buf[2] = 2;
			  mb_sl_datasize = 3;
			}
			else
			{
			  mb_slave_buf[2] = count << 1;
			  mb_sl_datasize = 3 + mb_slave_buf[2];
			  for (i = 0; i <= count; i++)
			  {
				if ((startaddr + i) == 0)
				  mb_read_flag = 1;
				mb_slave_buf[3 + (i << 1)] = (int) PARAMS_BUF[startaddr + i] >> 8;
				mb_slave_buf[4 + (i << 1)] = PARAMS_BUF[startaddr + i];
			  }
			}
		  break;
	      case 4:
			if (((startaddr + count) > MB_MAX_INPUT_PARAMS) || ((startaddr + count) < 1) || mb_sl_timer)
			{
			  mb_func = 0x80 + 4;
			  mb_slave_buf[2] = 2;
			  mb_sl_datasize = 3;
			}
			else
			{
			  mb_slave_buf[2] = count << 1;
			  mb_sl_datasize = 3 + mb_slave_buf[2];
			  for (i = 0; i <= count; i++)
			  {
				mb_slave_buf[3 + (i << 1)] = (int) mb_input_params[startaddr + i] >> 8;
				mb_slave_buf[4 + (i << 1)] = mb_input_params[startaddr + i];
			  }
			}
		 break;
	      case 16:
			if (((startaddr + count) > MB_MAXPARAMS) || ((startaddr + count) < 1) || mb_sl_timer)
			{
			  mb_func = 0x80 + 16;
			  mb_slave_buf[2] = 2;
			  mb_sl_datasize = 3;
			}
			else
			{
			  count = (mb_slave_buf[4] << 8) + mb_slave_buf[5];
			  for (i = 0; i < count; i++)
				PARAMS_BUF[i] = ((mb_slave_buf[7 + (i << 1)] << 8) + mb_slave_buf[8 + (i << 1)]);
			  if (0)
			  {
				mb_func = 0x80 + 16;
				mb_slave_buf[2] = 4;
				mb_sl_datasize = 3;
			  }
			  else
				mb_sl_datasize = 6;
			}
		  break;
	      default:
			mb_func = 0x80 + mb_func;
			mb_slave_buf[2] = 1;
			mb_sl_datasize = 3;
	    }
			crc = CRC16(mb_slave_buf, mb_sl_datasize);
			mb_slave_buf[mb_sl_datasize++] = crc;
			mb_slave_buf[mb_sl_datasize++] = crc >> 8;
			mb_sl_timer = MB_SENDPAUSE;
			mb_sl_mode = MB_SEND_MODE;
			mb_sl_timeout = 0;
	   }
     }
	mb_sl_bytecount++;
	break;
    default:
      break;
  }
}



#ifdef newadc
    int DT = 0;
    int DT1 = 0;
  	int Vl_3 = 0;
	int ADC_Value;
	int Gb_3 = 0;
void TIM1_UP_IRQHandler( void ) 
{	
    
  //ADC_Cmd(ADC1, ENABLE);
  ADC_SoftwareStartConvCmd(ADC1, ENABLE);
  ADC_Value=ADC_GetConversionValue(ADC1);
  adc_channel++;
  
  	 // ADC_InitStructure.ADC_NbrOfChannel = 9;
	 // ADC_Init(ADC1, &ADC_InitStructure);
  if ( adc_channel % 6 == 0)
  {
	  DT1 = ADC_GetConversionValue(ADC1) >> 4;
	  DT = ADC_Value >> 4;

	  
	  
	  //DT = ADC_GetConversionValue(ADC1);
	  ADC_RegularChannelConfig(ADC1, ADC_Channel_8, 1, ADC_SampleTime_239Cycles5);
  }
  else if ( adc_channel % 6 == 2)
  {
	  Vl_3 = ADC_Value >> 4;
	  
  	 // ADC_InitStructure.ADC_NbrOfChannel = 1;
	 // ADC_Init(ADC1, &ADC_InitStructure);
	  
	  //Vl_3 = ADC_GetConversionValue(ADC1);
	  ADC_RegularChannelConfig(ADC1, ADC_Channel_2, 1, ADC_SampleTime_239Cycles5);
  }
  else if (adc_channel % 6 == 4)
  {
	  Gb_3 = ADC_Value >> 4;
	  ADC_RegularChannelConfig(ADC1, ADC_Channel_9, 1, ADC_SampleTime_239Cycles5);
  }
  

}
	


#else
void TIM1_UP_IRQHandler( void ) 
{
  int ADC_Value;
  int DACtmp;
  adc_channel=((adc_channel==11)?0:adc_channel+1);
  ADC_Value=ADC_GetConversionValue(ADC1);
  switch (adc_channel)
  {
    case 0:
		Temp_Value_int+=(((ADC_Value<<16)-Temp_Value_int)>>8);	
		Params.Temp_Int=(28000-(Temp_Value_int>>16))/8.539539393939394+250;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_3, 1, ADC_SampleTime_239Cycles5);
    break;
  	case 1:
		Eass_Value_int+=((((ADC_Value-32768)<<16)-Eass_Value_int)>>8);
		Params.Ass=32768-90*((Eass_Value_int>>16))/32768;					//Eass
		ADC_RegularChannelConfig(ADC1, ADC_Channel_4, 1, ADC_SampleTime_239Cycles5);//4
    break;
  	case 2:
		Ehv_Value_int+=(((ADC_Value<<16)-Ehv_Value_int)>>8);	
		Params.HV=3350*(Ehv_Value_int>>16)/13305; //Ehv		 
		ADC_RegularChannelConfig(ADC1, ADC_Channel_6, 1, ADC_SampleTime_239Cycles5); //5
    break;
  	case 3:
		Pos_Value_int+=(((ADC_Value<<16)-Pos_Value_int)>>Params.Filter_Level);
		if(ADC_Value)Pos_Value1=FLAG_FILTER?(Pos_Value_int>>16):ADC_Value;
		  else Pos_Value1=0;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_5, 1, ADC_SampleTime_239Cycles5);		   //6
    break;
  	case 4:
		Neg_Value_int+=(((ADC_Value<<16)-Neg_Value_int)>>Params.Filter_Level);
		if(ADC_Value)Neg_Value1=FLAG_FILTER?(Neg_Value_int>>16):ADC_Value;	
		else	Neg_Value1=0;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_0, 1, ADC_SampleTime_239Cycles5);
    break;
  	case 5:			
		if ((ADC_Value<DEV_ADC_LIMB+DELTA_ADC_DEV)&&(ADC_Value>DEV_ADC_LIMB-DELTA_ADC_DEV)) 
		  Dev_ID=DEV_LIMB;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_7, 1, ADC_SampleTime_239Cycles5);//1
    break;
	case 6:
	  	Pos_Value_int2+=(((ADC_Value<<16)-Pos_Value_int2)>>Params.Filter_Level);
		if(ADC_Value)Pos_Value2=FLAG_FILTER?(Pos_Value_int2>>16):ADC_Value;
	  	else Pos_Value2=0;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_1, 1, ADC_SampleTime_239Cycles5);//7
	break;	
	case 7:
		Neg_Value_int2+=(((ADC_Value<<16)-Neg_Value_int2)>>Params.Filter_Level);
		if(ADC_Value)Neg_Value2=FLAG_FILTER?(Neg_Value_int2>>16):ADC_Value;
	  	else Neg_Value2=0;
		ADC_RegularChannelConfig(ADC1, ADC_Channel_8, 1, ADC_SampleTime_239Cycles5);
	break;
	case 8:
		DtT = ADC_Value>>4;	
		ADC_RegularChannelConfig(ADC1, ADC_Channel_2, 1, ADC_SampleTime_239Cycles5);
		break;
	case 9:
	  Temp_Bat=ADC_Value>>4;
	  
	  if((DtT>0x0DB && Temp_Bat<0x4DA)||
			(DtT>0x0A2 && Temp_Bat<0x473)||
		   	(DtT>0x076 && Temp_Bat<0x40B)||
		    (DtT>0x055 && Temp_Bat<0x3A4)||
		    Temp_Bat<0x33C)
			mode = MODE_DISCHARGE;
		else
			ticks_off = 0;
		    ADC_RegularChannelConfig(ADC1, ADC_Channel_9, 1, ADC_SampleTime_239Cycles5);
	break;
	case 10:
	  DTI_1 = ADC_Value>>4;	 
	  ADC_RegularChannelConfig(ADC1, ADC_Channel_3, 1, ADC_SampleTime_239Cycles5);
	break;
	case 11:
	  DTI_2 = ADC_Value>>4;
	  //TODO: make condition full battery
	  
	  int temp = (GPIOA->IDR&GPIO_Pin_12);
	  if(!BS_ON && DtT > 0x32F && DtT < 0xE80 && Temp_Bat <2300)
	  //if(BS_ON && DtT > 0x32F && DtT < 0xE80 && Temp_Bat <0x678)
			CHRG_ON;
		else
		{
			CHRG_OFF;
		}
		ADC_RegularChannelConfig(ADC1, ADC_Channel_16, 1, ADC_SampleTime_239Cycles5);
	break;
  }
  ADC_SoftwareStartConvCmd(ADC1, ENABLE);
  
  if (mb_sl_timer)
  {
    if (--mb_sl_timer == 0)
    {
      if (mb_sl_mode == MB_SEND_MODE)
      {
	      DE_ON;
	      USART_ITConfig(USART1, USART_IT_TXE, ENABLE);
	      if ((MB_LSR & 0x80) && (mb_sl_bytecount < mb_sl_datasize))
	      {
	        RS485_THR = mb_slave_buf[mb_sl_bytecount++];
	      }
      }
      else
      {
	      DE_OFF;
	      RE_ON;
	      USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);
      }
    }
  }

  if (ready_timer<=(READY_TIMER_VALUE*5)) ready_timer+=2;
// if (Dev_ID==DEV_EDELWEIS)
// {
//   if (ready_timer>READY_TIMER_VALUE) 
// 	{
// 	 READY_ON;
// 	}
// }

//   if ((ready_timer>READY_TIMER_VALUE) && (!init))
// 	  READY_ON;	

//   if (Dev_ID==DEV_LIMB)
//   {
//     if (ready_timer<30000)
//       DACtmp=LIMB_TEST_VAL6;
//     else
//     {
//       switch (Params.Flags_Read&0x3)
//       {
//         case 0: DACtmp=LIMB_NONE_VAL; break;
//         case 1: DACtmp=LIMB_PI_VAL; break;
//         case 2: DACtmp=LIMB_PF_VAL; break;
//         case 3: DACtmp=LIMB_PI_PF_VAL; break;
//       }
//     }
//     if (ready_timer<25000) DACtmp=LIMB_TEST_VAL5;
//     if (ready_timer<20000) DACtmp=LIMB_TEST_VAL4;
//     if (ready_timer<15000) DACtmp=LIMB_TEST_VAL3;
//     if (ready_timer<10000) DACtmp=LIMB_TEST_VAL2;
//     if (ready_timer<5000) DACtmp=LIMB_TEST_VAL1;
//     TIM2->CCR2=DACtmp;
//   }
  if (LED_MODE_AUTO)
  {
		if(Params.Flags_Read&1) L_ON; else L_OFF;
		if(Params.Flags_Read&2) F_ON; else F_OFF;
		if(Params.Flags_Read&4) A_ON; else A_OFF;
		if(Params.Flags_Read&8) H_ON; else H_OFF;
		//if(Params.Flags_Read&16) CTRL_HL_ON; else CTRL_HL_OFF;
		if(Params.Flags_Read&32) RDY_ON; else RDY_OFF;
  }
	
  TIM_ClearITPendingBit(TIM1, TIM_IT_Update);
}
#endif



void SysTick_Handler( void ) {
	ticks++;
	beep_ticks++;
	ticks_off++;
	if(CTRL)
		button_ticks++;
	else
	{
		if(init==0 && button_ticks>200)
			OFF_ON;
		else if(init==0 && button_ticks<100&&button_ticks>30&& !(MODE_CONTROL_2))
		  {mode=MODE_CONTROL;ticks=0;MODE_CONTROL_2_ON;control=0;READY_OFF;BEEP_MODE_NORMAL;}
		else if(MODE_CONTROL_2 && control==0 &&init==0 && button_ticks<100&&button_ticks>10)
			{mode=MODE_READY;ticks=0;MODE_CONTROL_2_OFF;LED_MODE_AUTO_ON;}
		else if(init==1 && button_ticks>150)
		  {init=0;OFF_OFF;}
		button_ticks=0;
	}
  Beep();
}
//------------------------------------------------------------------------------

#ifdef USE_FULL_ASSERT

void assert_failed(uint8_t* file, uint32_t line) {
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
    ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
	while(1) {
		
	}
}

#endif