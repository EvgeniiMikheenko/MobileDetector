
typedef void (* SetBit)(bool value);
typedef void (* UartInit)(uint16_t baudrate);
typedef void (* UartSend)(uint8_t data);
typedef void (* TimerCmd)( void );
typedef uint8_t (* UartRecv)( void );
typedef uint8_t (* IRRType)( void );



typedef struct {
	uint8_t 	address;						/* ����� ���������� */
	uint8_t		function;						/* ������� modbus */
	uint16_t 	num_regs;						/* ���������� ��������� */
	uint8_t 	stopBits;						/* ���������� ���� ��� */
	uint32_t 	serialBaudrate;					/* �������� ����� */

	//
	uint8_t 	*rxBuf;							/* ��������� �� ����� ������ */
	uint32_t	rxBufSize;						/* ������ ������ ������ */
	uint32_t	rxCount;						/* ���������� �������� ������ */
	
	uint8_t 	*txBuf;							/* ��������� �� ����� �������� */
	uint32_t	txBufSize;						/* ������ ������ �������� */
	uint32_t	txCount;						/* ���������� ���� ��� �������� */
	uint32_t	txIndex;						/* ����� ����������� ����� */
	//
	uint32_t	timeLeft;						/* ������ ������� � ������� ������ ���������� ����� */
	uint32_t	timeOut;						/* �������� �������� ����� ��������� ���������  */
	uint8_t		timer_set;						/* �������� ������� ������� � ����� �������� (1) */
	//		
//	uint32_t	hostTimeOut;					/* �������� ��������� �������� ����� � ������ */
//	uint32_t	hostTimeLeft;					/* ������ ������� � ������� ��������� ���������� */	
	
	UartInit	lpUartInit;						/* ��������� �� ������� ������������� ����� */
	UartSend	lpUartSend;						/* ��������� �� ������� �������� ������ ����� */
	UartRecv	lpUartRecv;						/* ��������� �� ������� ������ ������ ����� */
	//GetDeviceInfo lpGetDeviceInfo;				/* ��������� �� ������� ��������� ���������� �� ���������� */
	
	SetBit		lpSetTxEn;						/* ��������� �� ������� ���/���� ������ TXEN RS485 */
	SetBit		lpSetRxEn;						/* ��������� �� ������� ���/���� ������ RXEN RS485 */
	TimerCmd		lpTimerStart;					/* ��������� �� ������� ������� ������� */
	TimerCmd		lpTimerReset;					/* ��������� �� ������� ������ �������� � ��������� ������� */
	IRRType		GetIntSourse;					/* �������� ����������� ����� ����� ��� ��������   Tx -- 010   Rx -- 101 */
	
	
	int			passwordRegNum;					/* ����� ���������� �������� ��� ������ */
	int			deviceInfoSizeRegNum;
	int			deviceInfoRegNum;				/* ����� ���������� �������� ��� ���������� �� ���������� */
	
	const char* password;
	int			passwordLen;
	

	
	uint32_t	errorCount;						/* ���������� ������ ���������*/
	
	uint16_t	*lpRegisters;					/* ��������� �� ����� ��������� */
	uint16_t	registersCount;					/* ���������� ��������� */
	

	 
	union {
		uint32_t value;
		struct {
			unsigned enableTimer 			: 1;	/* ���� ���������� ������� */
			unsigned enableParseRxData 		: 1;	/* ���� ���������� ��������� ��������� ������ */
			unsigned isParsedStart 			: 1;	/* ����, �����������, ��� ��������� �������� ������ ������ */
			unsigned isRxDataMain 			: 1;	/* ���� ���������� �������� ������ */
			unsigned isHostTimeout 			: 1;	/* ���� �������� ����� � ������ */
			unsigned isConfigChangeLock 	: 1;	/* ���� ���������� ��������� ������������ */
			unsigned eepromNeedUpdate 		: 1;	/* ���� ������������� ���������� ����������������� ������ */
			unsigned isPasswordValid		: 1;   	/* ���� ����������� ������ */
			unsigned isPacket						: 1;		/*������, ����� � �������� */
		};
	} Flags;
	
} MbParam_t, *lpMbParam_t;
