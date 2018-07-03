#include "gsa3m.h"
void Control(void)
{
  switch(control)
  {
    case 0:
      if(ticks < 500)
			{
				F_ON;L_ON;A_ON;RDY_ON;H_ON;CTRL_HL_ON;DCH_ON;
			}
			else
			{
				DCH_OFF;
				if (ticks>1500)
					control++;
			}
			break;
    case 1:
      if(ticks<100)
      {
        if(gsa3_flags&1) MODE_DISCHARGE;
      }
      else 
        control++;
      break;
    case 2:
      if(E11<P11 && E12>P12 && E21<P21 && E22>P22)
        control++;
      else mode = MODE_FAILURE;
      break;
    case 3:
      if(ticks>500) control++;
      break;
    case 4:
      if(ticks<700) 
      {
        if(!(E11<P11 && E12>P12 && E21>P21 && E22<P22))
          mode = MODE_FAILURE;
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
        if (ticks>6000) 
          mode = MODE_FAILURE;
      break;
    case 6:
      if (E12<P12)
      {
        mode = MODE_READY;
        RDY_ON;
        CTRL_HL_OFF;
      }
      else
        if (ticks>500)
          mode = MODE_FAILURE;
      break;
  }
}

void Beep(void)
{
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

void GSA3 (void)
{
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
      RDY_ON;
      if (Params.Flags_Read&15)
        BEEP_MODE_ALARM;
      else
        BEEP_MODE_NORMAL;
      break;
  }
}