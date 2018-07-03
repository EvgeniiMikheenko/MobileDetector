#include <LPC17xx.H>
//#include <system_LPC17xx.h>
//#include <lpc17xx_pinsel.h>
//#include <lpc17xx_gpio.h>
//#include <lpc17xx_systick.h>







void pins_led_init( void );

int main() {
	//	int i;
	SystemInit (); 
	LPC_SC -> PCONP |= (1<<15);
	//pins_led_init();
	
	//LPC_GPIO1->FIODIR =0;
	// LPC_GPIO0->FIODIR |= (1 << 4);
	//	  LPC_GPIO0->FIODIR |= (1 << 22);
	LPC_GPIO0->FIODIR |= (1 << 26);

	//	LPC_GPIO0->FIOPIN |= (1 << 4);
	//		LPC_GPIO0->FIOPIN |= (1 << 22);
	//SysTick_Config(SystemCoreClock/15);
    //LPC_GPIO0->FIOCLR |= (1 << 26);
	SysTick_Config(SystemCoreClock / 10);

	while(1){
	     //GPIO_SetValue(0, 1 << 26); //LPC_GPIO0->FIOSET |= (1 << 26);
		//for(volatile int i = 0; i < 1000000; i++) {}
	    
		
		
		
		
		//LPC_GPIO0->FIOCLR |= (1 << 26);
		
		
		
		
		
		//GPIO_ClearValue(0, 1 << 26); //LPC_GPIO0->FIOCLR |= (1 << 26);
		
			    

		//for(volatile int i = 0; i < 100000; i++) {}
		//GPIO_ClearValue(0, 1 << 26); //LPC_GPIO0->FIOCLR |= (1 << 26);
		//LPC_GPIO0->FIOPIN |= (1 << 4);
		//	i =10000;
		//while ( i > 0){i--;}
		//LPC_GPIO0->FIOPIN &= ~(1 << 4) ;
		//i =10000;
		//	while ( i > 0){i--;}
			
		
	}
//SYSTICK_InternalInit(1);

}

/*
void pins_led_init( void ) {
	PINSEL_CFG_Type pinCfg;
	
	pinCfg.Portnum = 0;
	pinCfg.Pinnum = 26;
	pinCfg.Pinmode = PINSEL_PINMODE_PULLDOWN;
	pinCfg.Funcnum = PINSEL_FUNC_0;
	PINSEL_ConfigPin(&pinCfg);
	GPIO_SetDir(0, 1 << 26, 1);
	GPIO_ClearValue(0, 1 << 26);
}
*/

uint32_t led1_value = 0;
uint32_t led1_mask = 0;
#define COUNTER_LED_MAX			10

void SysTick_Handler(void) {
	static uint32_t counter = 0;
			//GPIO_ClearValue(0, 1 << 26);
		//LPC_GPIO0->FIOSET |= (1 << 26);
		//for(volatile int i = 0; i < 10000; i++) {}
	
	if(led1_value & 0x01)
		LPC_GPIO0->FIOSET |= (1 << 26);
	else
		LPC_GPIO0->FIOCLR |= (1 << 26);
	
	led1_value >>= 1;
	
	if((++counter) >= COUNTER_LED_MAX) {
		counter = 0;
		led1_value = led1_mask;
	}
}

