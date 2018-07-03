
#ifndef _RFM12B_H_
#define _RFM12B_H_

// Configuration setting command

#define CSC		0X8008

#define EL_ON		CSC |= (1 << 7)   // enable internal data register
#define EL_OFF	CSC |= (0 << 7)

#define EF_ON		CSC |= (1 << 6) 	//enable fifof mode
#define EF_OFF 	CSC |= (0 << 6)

#define	Mhz_433		CSC |= (1 << 4)&(0 << 5)  // freq selection
#define Mhz_868		CSC |= (1 << 5)&(0 << 4)
#define Mhz_915		CSC |= (1 << 5)&(1 << 4)
////////////////////////////////////////////




// Power managementt command

#define	PMC		0x8208

#define	ER_ON		PMC |= (1 << 7)  // enable receiver
#define ER_OFF	PMC |= (0 << 7)

#define EBB_ON 	PMC |= (1 << 6)  // recaiver baseband
#define EBB_OFF	PMC |= (0 << 6)

#define ET_ON		PMC |= (1 << 5)  // PLL ON, START TRANSMISSION
#define ET_OFF  PMC |= (0 << 5)

#define ES_ON		PMC |= (1 << 4)  // turn on the synthesizer
#define ES_OFF	PMC |= (0 << 4)

#define EX_ON		PMC |= (1 << 3)  // crystal oscillator on
#define EX_OFF	PMC |= (0 << 3)

#define EB_ON		PMC |= (1 << 2)  // enable low battery detector
#define EB_OFF	PMC |= (0 << 2)

#define EW_ON		PMC |= (1 << 1) // enable wake-up timer
#define EW_OFF	PMC |= (0 << 1)

#define DC_ON		PMC |= (1 << 0) // disable clock output in pin8
#define DC_OFF	PMC |= (0 << 0)
///////////////////////////////////////////////////////




// Frequency setting command

#define FSC		0XA640   // 430.8 MHz
///////////////////////////////////////////////


// Data Rate Command

#define DRC  0xC647		// 4.8 Kbps
/////////////////////////////////////////////////

// Receiver control command

#define  RCC		0x9080

#define  II_ON	  RCC |= (0 << 10) // interrupt input enable
#define  VDI_ON 	RCC |= (1 << 10) // VDI output (valid data indicator)


#define VDI_FAST		RCC |= (0 << 9)&(0 << 8)	// SIGNAL RESPONSE TIME SETTING
#define VDI_MEDIUM	RCC |= (0 << 9)&(1 << 8)
#define VDI_SLOW		RCC |= (1 << 9)&(0 << 8)
#define VDI_ALWAYS_ON		RCC |= (1 << 9)&(1 << 8)



#define	BW_SELECT_400		RCC += 0x20  // receiver baseband bandwidth
#define	BW_SELECT_340		RCC += 0x40
#define	BW_SELECT_270		RCC += 0x60
#define	BW_SELECT_200		RCC += 0x80
#define	BW_SELECT_134		RCC += 0xA0
#define	BW_SELECT_67		RCC += 0xC0

#define LNA_SELECT_0		RCC |= (0 << 4)&(0 << 3)  // gain relative to maximum dB
#define LNA_SELECT_6		RCC |= (0 << 4)&(1 << 3)
#define LNA_SELECT_14		RCC |= (1 << 4)&(0 << 3)
#define LNA_SELECT_20		RCC |= (1 << 4)&(1 << 3)


#define  RSSI_103			RCC += 0		// RSSI detector threshold
#define  RSSI_97			RCC += 1
#define  RSSI_91			RCC += 2
#define  RSSI_85			RCC += 3
#define  RSSI_79			RCC += 4
#define  RSSI_73			RCC += 5
///////////////////////////////////////////////////////////////////////////////





// Data filter command

#define DFC  0xC22C

#define  AL_ON		DFC |= (1 << 7)  // clock recovery auto mode
#define  AL_OFF		DFC |= (0 << 7)  // manual mode

#define	 ML_ON		DFC |= (1 << 6)  // clock recovery lock control, fast mode
#define	 ML_OFF		DFC |= (0 << 6)  // slow mode


#define S_DIGITAL_FILTER		DFC |= (0 << 4)		//data filter
#define S_ANALOG_RC					DFC |= (1 << 4)  // If analog RC filter is selected the internal clock recovery circuit and the FIFO cannot be used. 
//////////////////////////////////////////////////////////////////////////





//FIFO and Reset mode command

#define  FRMC		0xCA80

#define		SP_2DD4		FRMC |= (0 << 3) //2 byte synchron pattern
#define		SP_D4			FRMC |= (1 << 3) //1 byte synchron pattern


#define 	AL_FIFO_ON			FRMC |= (1 << 2) //	 input of the FIFO fill start condition: Always fill
#define		AL_FIFO_OFF		FRMC |= (0 << 2) //	 input of the FIFO fill start condition: synchron pattern


#define  DR_ON		FRMC |= (0 << 0)  //  Disables the highly sensitive RESET mode: Sensitive reset
#define  DR_OFF		FRMC |= (1 << 0)  //  Disables the highly sensitive RESET mode: Non-sensitive reset
/////////////////////////////////////////////////////////////////////////////


//Synchron pattern command

#define		SPC		0xCED4
////////////////////////////////////////////////////////////////////////////

typedef union {
	uint16_t value;
	struct {
		unsigned		OFFS3 : 3;
		unsigned		OFFS6 : 1;
		unsigned		ATGL	: 1;
		unsigned		CRL		: 1;
		unsigned		DQD		: 1;
		unsigned		RSSI	: 1;
		unsigned		FFEM	: 1;
		unsigned		LBD		: 1;
		unsigned		EXT		: 1;
		unsigned		WKUP	: 1;
		unsigned		FFOV	: 1;
		unsigned		POR		: 1;
		unsigned		FFIT	: 1;
		
	} Bits;
} RFM_Status;

#endif // _RFM12B_H_
