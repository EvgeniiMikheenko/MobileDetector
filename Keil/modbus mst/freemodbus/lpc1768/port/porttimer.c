/*
 * FreeModbus Libary: BARE Port
 * Copyright (C) 2006 Christian Walter <wolti@sil.at>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 *
 * File: $Id: porttimer.c,v 1.1 2006/08/22 21:35:13 wolti Exp $
 */

/* ----------------------- Platform includes --------------------------------*/
#include "port.h"
#include <lpc17xx_timer.h>
/* ----------------------- Modbus includes ----------------------------------*/
#include "mb.h"
#include "mbport.h"

/* ----------------------- static functions ---------------------------------*/
static void prvvTIMERExpiredISR( void );

/* ----------------------- Start implementation -----------------------------*/


TIM_TIMERCFG_Type	TIM0CFG;
TIM_MATCHCFG_Type TIM0MATCH;


BOOL
xMBPortTimersInit( USHORT usTim1Timerout50us )
{
	
	TIM0CFG.PrescaleOption = TIM_PRESCALE_USVAL;
	TIM0CFG.PrescaleValue = 50;
	
	
	TIM0MATCH.ExtMatchOutputType = TIM_EXTMATCH_NOTHING;
	TIM0MATCH.IntOnMatch = ENABLE;
	TIM0MATCH.MatchChannel = 0;
	TIM0MATCH.MatchValue = 1;
	TIM0MATCH.ResetOnMatch = ENABLE;
	TIM0MATCH.StopOnMatch = ENABLE;
	
	
	TIM_Init(LPC_TIM0, TIM_TIMER_MODE, &TIM0CFG);
	
	
//	TIM_ConfigMatch(LPC_TIM0, &TIM0MATCH);
	NVIC_EnableIRQ(TIMER0_IRQn);
		
	
    return TRUE1;
}


inline void
vMBPortTimersEnable(  )
{
	
	TIM_ResetCounter(LPC_TIM0);
	TIM_Cmd(LPC_TIM0, ENABLE);
	
    /* Enable the timer with the timeout passed to xMBPortTimersInit( ) */
}

inline void
vMBPortTimersDisable(  )
{
	
		TIM_Cmd(LPC_TIM0, DISABLE);
    /* Disable any pending timers. */
}

/* Create an ISR which is called whenever the timer has expired. This function
 * must then call pxMBPortCBTimerExpired( ) to notify the protocol stack that
 * the timer has expired.
 */
//static void prvvTIMERExpiredISR( void )
static void TIMER1_IRQHandler( void )
{
	
		TIM_ClearIntPending(LPC_TIM0, TIM_MR0_INT);
	
    ( void )pxMBPortCBTimerExpired(  );
}

