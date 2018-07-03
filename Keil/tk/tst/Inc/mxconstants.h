/**
  ******************************************************************************
  * File Name          : mxconstants.h
  * Description        : This file contains the common defines of the application
  ******************************************************************************
  *
  * COPYRIGHT(c) 2016 STMicroelectronics
  *
  * Redistribution and use in source and binary forms, with or without modification,
  * are permitted provided that the following conditions are met:
  *   1. Redistributions of source code must retain the above copyright notice,
  *      this list of conditions and the following disclaimer.
  *   2. Redistributions in binary form must reproduce the above copyright notice,
  *      this list of conditions and the following disclaimer in the documentation
  *      and/or other materials provided with the distribution.
  *   3. Neither the name of STMicroelectronics nor the names of its contributors
  *      may be used to endorse or promote products derived from this software
  *      without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MXCONSTANT_H
#define __MXCONSTANT_H
  /* Includes ------------------------------------------------------------------*/

/* USER CODE BEGIN Includes */

/* USER CODE END Includes */

/* Private define ------------------------------------------------------------*/

#define CH1_IN0_Pin GPIO_PIN_0
#define CH1_IN0_GPIO_Port GPIOC
#define CH1_IN1_Pin GPIO_PIN_1
#define CH1_IN1_GPIO_Port GPIOC
#define CH2_IN0_Pin GPIO_PIN_2
#define CH2_IN0_GPIO_Port GPIOC
#define CH2_IN1_Pin GPIO_PIN_3
#define CH2_IN1_GPIO_Port GPIOC
#define ADC_PRESSURE_Pin GPIO_PIN_0
#define ADC_PRESSURE_GPIO_Port GPIOA
#define ADC_VCC_SYS_Pin GPIO_PIN_1
#define ADC_VCC_SYS_GPIO_Port GPIOA
#define ADC_TEMP_Pin GPIO_PIN_2
#define ADC_TEMP_GPIO_Port GPIOA
#define ADC_HUM_Pin GPIO_PIN_3
#define ADC_HUM_GPIO_Port GPIOA
#define DBG_UART_TX_Pin GPIO_PIN_10
#define DBG_UART_TX_GPIO_Port GPIOB
#define DBG_UART_RX_Pin GPIO_PIN_11
#define DBG_UART_RX_GPIO_Port GPIOB
#define CH3_IN0_Pin GPIO_PIN_12
#define CH3_IN0_GPIO_Port GPIOB
#define CH3_IN1_Pin GPIO_PIN_13
#define CH3_IN1_GPIO_Port GPIOB
#define CH4_IN0_Pin GPIO_PIN_14
#define CH4_IN0_GPIO_Port GPIOB
#define CH4_IN1_Pin GPIO_PIN_15
#define CH4_IN1_GPIO_Port GPIOB
#define PWM_40k_Pin GPIO_PIN_6
#define PWM_40k_GPIO_Port GPIOC
#define US_TX_START_Pin GPIO_PIN_7
#define US_TX_START_GPIO_Port GPIOC
#define RX_CH_SEL1_Pin GPIO_PIN_8
#define RX_CH_SEL1_GPIO_Port GPIOC
#define RX_CH_SEL0_Pin GPIO_PIN_9
#define RX_CH_SEL0_GPIO_Port GPIOC
#define US_RX_TRIGGER_Pin GPIO_PIN_8
#define US_RX_TRIGGER_GPIO_Port GPIOA
#define RS485_HOST_TX_Pin GPIO_PIN_9
#define RS485_HOST_TX_GPIO_Port GPIOA
#define RS485_HOST_RX_Pin GPIO_PIN_10
#define RS485_HOST_RX_GPIO_Port GPIOA
#define RS485_HOST_TXEN_Pin GPIO_PIN_11
#define RS485_HOST_TXEN_GPIO_Port GPIOA
#define LATCH_Pin GPIO_PIN_2
#define LATCH_GPIO_Port GPIOD
#define FLASH_SCK_Pin GPIO_PIN_3
#define FLASH_SCK_GPIO_Port GPIOB
#define FLASH_MISO_Pin GPIO_PIN_4
#define FLASH_MISO_GPIO_Port GPIOB
#define FLASH_MOSI_Pin GPIO_PIN_5
#define FLASH_MOSI_GPIO_Port GPIOB
#define I2C_SCL_Pin GPIO_PIN_6
#define I2C_SCL_GPIO_Port GPIOB
#define I2C_SDA_Pin GPIO_PIN_7
#define I2C_SDA_GPIO_Port GPIOB
/* USER CODE BEGIN Private defines */

/* USER CODE END Private defines */

/**
  * @}
  */ 

/**
  * @}
*/ 

#endif /* __MXCONSTANT_H */
/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
