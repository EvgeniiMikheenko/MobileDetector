/**
* \file
*
* \brief Empty user application template
*
*/

/**
* \mainpage User Application template doxygen documentation
*
* \par Empty user application template
*
* Bare minimum empty user application template
*
* \par Content
*
* -# Include the ASF header files (through asf.h)
* -# "Insert system clock initialization code here" comment
* -# Minimal main function that starts with a call to board_init()
* -# "Insert application code here" comment
*
*/

/*
* Include header files for all drivers that have been imported from
* Atmel Software Framework (ASF).
*/
/*
* Support and FAQ: visit <a href="http://www.atmel.com/design-support/">Atmel Support</a>
*/

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
#include <eeprom_store.h>




#define AREF 4
#define length 16
#define pwm_min 0x02
#define pwm_max 0xFA
#define pwm_step 0x19
#define	SHOW_TIME_COUNTER		30
#define resempling_degree	5
//#define F_CPU 2000000






void set_level(double val);
void adc_init( uint8_t channel );
void pwm_init( uint8_t tim1_val );
void overload( void );
void timer1_init( void );
void uart_init( void );
void set_dac_uf(uint16_t val);



uint16_t adc_data;
volatile uint16_t counter = 0;
volatile int flag = 0;
uint8_t pwm = pwm_min;
uint8_t txflag = 0;
uint8_t txbuf[2];
uint8_t adc_select = 0;
uint8_t adc_select_flag;
uint16_t ref_divider;
uint8_t timer_counter=0;
float value1 = 0;
double value;
uint16_t step;
uint16_t median_adc_data;
uint32_t resempling;	
uint32_t resempling_val;
uint8_t	 resempling_flag;	


uint8_t SHOW_TIME;
static bool isInit = false;


int main (void)
{
		
	
 	RingBuffer_InitBuffer(&Buffer, BufferData, sizeof(BufferData));
 	uart_init();
	
	while(!read_eeprom()){};
	



	cli();
	
	DDRA |= (1 << 3);

	PORTD = 0;//(1 << 6) ;
	PORTD |= (1 << 2);
	
	DDRC |= 0xFF;
	DDRB |= (1 << 1)|(1 << 3)|(1 << 0);
	
	DDRD |= (1 << 6)|(1 << 1);
	PORTC = 0xE7;
	set_dac_uf(0xFFF);				// Reduce flow sensor voltage supply
	adc_init(0x05);	
	
	ADCSRA |= (1 << ADSC);					//Get value of  reduced flow sensor voltage supply
	delay_ms(100);
	uint16_t b1 = ADCL;
	uint16_t b2 = ADCH & 0x03;	
	ref_divider = (b2 << 8) + b1;

	adc_init(0x06);

	
	
	sei();

	pwm_init(pwm);
	timer1_init();


	isInit = true;

	while(1)
	{

		if(flag > 400)						//Button PWM control
		{
			cli();
			
			PORTC |= (1 << 6);
			
			if(pwm >= pwm_max)
			{
				pwm = pwm_min;
				PORTC |= (1 << 5);
				delay_s(1);
				PORTC &= ~(1 << 5);
			};
			pwm_init( pwm );
			PORTC &= ~(1 << 6);
			pwm += pwm_step;
			flag=0;
			sei();
		}
		if(resempling_flag)							// Data is collected, processing,  output 
		{
			cli();
			resempling_val = (resempling >> resempling_degree)>>5;
			resempling = 0;
			resempling_flag = 0;
			step = 0;
			value1 = resempling_val*4.1/1024;
			value = kA * pow(value1,3) + kB * pow(value1,2) + kC * value1 + kD;
			set_level(value);
			sei();
		}
	};
}

void set_level( double val)
{

	if( val > 1000 )			// Check borders
	{
		overload();
	};

	if ( val < 50 )
	{
		val = 50;
	};

	int num = (int)((val+25)/50)-1;
	PORTC = 0xE0 + num;			//Set val on port C
};


void adc_init( uint8_t channel )
{
	ADMUX = channel;	
	ADCSRA = 0xCF;		//Enable, Start conversion, interrupt enable, xtal/128
	sei();
}




ISR(ADC_vect, ISR_NOBLOCK)
{
cli();


	ADMUX = 0x06;		// FLOW channel
	uint16_t buf1 = ADCL;
	uint16_t buf2 = ADCH & 0x03;
	counter > 0xFFFD? counter = 0: counter++;	 // Global counter
	
		step++;				//Resempling counter
		adc_data = (buf2 << 8) + buf1;
		resempling += adc_data;
		if(step == 1023)
		{
			resempling_flag = 1;
		}
		
sei();
}



void overload( void )		//FLOW >= 1000 CM3/MIN
{
	SREG &= 0x7F;			//==cli();
	for(int i = 0; i < 1; i++)
	{
		PORTC = 0x60; // blue + buzzer on
		delay_ms(200);
		PORTC = 0xC0; // red on
		delay_ms(200);
	};
	sei();
};



void pwm_init( uint8_t tim1_val  )
{
	DDRD |= (1 << 7)|(1 << 4)|(1 << 3)|(1 << 5); //set PD7 as output
	PORTD |= (1 << 3); //enable DD2 driver
	TCCR1A |= (1<<WGM20)|(1<<WGM21); //select Fast PWM mode
	TCCR1A |= (1 << COM1A1)|(1 << COM1B1); //clear OC2 on compare match
	TCCR1B =  (1<<CS10); //Clock Prescaler is 1024
	OCR1AH = tim1_val;
	OCR1AL = tim1_val;	
	OCR1BH = tim1_val;
	OCR1BL = tim1_val;
	TCCR2A |= (1<<WGM20)|(1<<WGM21); //select Fast PWM mode XS5
	TCCR2A |= (1 << COM2A1); //clear OC2 on compare match XS5
	TCCR2B |=(1<<CS20); //Clock Prescaler is 1 XS5
	OCR2A = tim1_val; // Set Dutycycle XS5
}



void timer1_init( void )
{

	TIMSK0=(1<<TOIE0);
	// set timer0 counter initial value to 0
	TCNT0=0x00;
	// start timer0 with /64 prescaler
	TCCR0B = (1 << CS01)|(1 << CS00);
}


ISR(TIMER0_OVF_vect) {
	timer_counter++;
	ADCSRA |= (1 << ADSC);

	if(!(PIND & 4))  //Button
	{
		flag++;

	}
}

void uart_init( void )
{
	UCSR0A = 0x02;
	UCSR0B = 0x98;   //Rx interrupt enable
	UCSR0C = 0x06;
	UBRR0L = 0x15;
	UBRR0H = 0x00;
};



ISR(USART0_RX_vect)
{
	
	int rx_data_byte;
	
	if(UCSR0A & (1 << 7))		// USART Receive Complete
	{
		rx_data_byte = UDR0;
		delay_ms(5);
		UDR0 = rx_data_byte;		// Send back to terminal
			
		if( rx_data_byte == 13)		// Carriage return
		{
			rx_data_buf[rx_num] = '\0';
			rx_parse();
			rx_data_buf[0] = '\0';
			rx_num = 0;	
		}
		else if(rx_data_byte == 127)	// Del (backspace)
		{
			rx_num--;
		}
		else
		{
			rx_data_buf[rx_num] = rx_data_byte;	
		
			if( rx_num < RX_BUF_SIZE )
				rx_num++;
		
		}
	};
}



ISR(USART0_TX_vect)
{

	uint16_t BufferCount = RingBuffer_GetCount(&Buffer);
	if(BufferCount == 0)
	{
		UDR0 = 0x00;
		UCSR0B &= ~(1 << 6);		//TX Complete Interrupt Disable
	}
	else
	{
		UDR0 = RingBuffer_Remove( &Buffer );
	}
};



void set_dac_uf(uint16_t val)		// Reduce flow sensor voltage supply
{
	
	PORTB &= ~(1 << 0);
	
	for(int i =0; i < 16; i++)
	{
		PORTB |= (1 << 1);
		if(val & (0x8000 >> i)){
			PORTB |= (1 << 3);}
		else{
			PORTB &= ~(1 << 3);}
		PORTB &= ~(1 << 1);
		
	}
	
	PORTB |= (1 << 0);
}


