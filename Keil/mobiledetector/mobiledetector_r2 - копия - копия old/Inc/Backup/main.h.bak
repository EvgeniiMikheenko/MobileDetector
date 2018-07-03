/**
  ******************************************************************************
  * File Name          : main.h
  * Description        : This file contains the common defines of the application
  ******************************************************************************
  *
  * COPYRIGHT(c) 2017 STMicroelectronics
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
#ifndef __MAIN_H
#define __MAIN_H
  /* Includes ------------------------------------------------------------------*/

/* USER CODE BEGIN Includes */

/* USER CODE END Includes */

/* Private define ------------------------------------------------------------*/

#define SHUNT_ON_Pin GPIO_PIN_14
#define SHUNT_ON_GPIO_Port GPIOC
#define RS485_DE_Pin GPIO_PIN_15
#define RS485_DE_GPIO_Port GPIOC
#define LED1_Pin GPIO_PIN_1
#define LED1_GPIO_Port GPIOA
#define ADC_IN2_Pin GPIO_PIN_2
#define ADC_IN2_GPIO_Port GPIOA
#define I2C2_SCL_Pin GPIO_PIN_10
#define I2C2_SCL_GPIO_Port GPIOB
#define I2C2_SDA_Pin GPIO_PIN_11
#define I2C2_SDA_GPIO_Port GPIOB
#define LED5_Pin GPIO_PIN_13
#define LED5_GPIO_Port GPIOB
#define LED3_Pin GPIO_PIN_14
#define LED3_GPIO_Port GPIOB
#define RS485_TX_Pin GPIO_PIN_9
#define RS485_TX_GPIO_Port GPIOA
#define RS485_RX_Pin GPIO_PIN_10
#define RS485_RX_GPIO_Port GPIOA
#define USB_DM_Pin GPIO_PIN_11
#define USB_DM_GPIO_Port GPIOA
#define USB_DP_Pin GPIO_PIN_12
#define USB_DP_GPIO_Port GPIOA
#define ZUMMER_Pin GPIO_PIN_15
#define ZUMMER_GPIO_Port GPIOA
#define BTN1_Pin GPIO_PIN_3
#define BTN1_GPIO_Port GPIOB
#define VAR_RES_UD_Pin GPIO_PIN_4
#define VAR_RES_UD_GPIO_Port GPIOB
#define VAR_RES_NCS_Pin GPIO_PIN_5
#define VAR_RES_NCS_GPIO_Port GPIOB
#define VAR_RES_SCLK_Pin GPIO_PIN_6
#define VAR_RES_SCLK_GPIO_Port GPIOB
#define LED2_Pin GPIO_PIN_8
#define LED2_GPIO_Port GPIOB
#define LED4_Pin GPIO_PIN_9
#define LED4_GPIO_Port GPIOB
/* USER CODE BEGIN Private defines */

#define FLASH_LOCATION			((uint32_t)0x2000000-FLASH_PAGE_SIZE)			//FLASH_BANK1_END
#define EndAddr					((uint32_t)0x2000000)
#define FLASH_SIZE				(((unsigned short *)&Params.crc - (unsigned short *) & Params.var_res_level)/2+1)
#define CONFIG_REGISTERS_COUNT 	((unsigned short *)&Params.crc - (unsigned short *) & Params.var_res_level)

#define MODBUS_DEVICE_INFO 			"Mobile Detector"
#define	InfoSizeRegNum						60000
#define	InfoRegNum								60001


/* USER CODE END Private defines */

/**
  * @}
  */ 

/**
  * @}
*/ 

#endif /* __MAIN_H */
/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
