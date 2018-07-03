
typedef void (* SetBit)(bool value);
typedef void (* UartInit)(uint16_t baudrate);
typedef void (* UartSend)(uint8_t data);
typedef void (* TimerCmd)( void );
typedef uint8_t (* UartRecv)( void );
typedef uint8_t (* IRRType)( void );



typedef struct {
	uint8_t 	address;						/* Адрес устройства */
	uint8_t		function;						/* Функция modbus */
	uint16_t 	num_regs;						/* Количество регистров */
	uint8_t 	stopBits;						/* Количество стоп бит */
	uint32_t 	serialBaudrate;					/* Скорость порта */

	//
	uint8_t 	*rxBuf;							/* Указатель на буфер приема */
	uint32_t	rxBufSize;						/* Размер буфера приема */
	uint32_t	rxCount;						/* Количество принятых данных */
	
	uint8_t 	*txBuf;							/* Указатель на буфер передачи */
	uint32_t	txBufSize;						/* Размер буфера передачи */
	uint32_t	txCount;						/* Количество байт для передачи */
	uint32_t	txIndex;						/* Номер переданного байта */
	//
	uint32_t	timeLeft;						/* Прошло времени с момента приема последнего байта */
	uint32_t	timeOut;						/* Значение таймаута после окончания тразакции  */
	uint8_t		timer_set;						/* Значение переода таймера в микро секундах (1) */
	//		
//	uint32_t	hostTimeOut;					/* Значение защитного таймаута связи с хостом */
//	uint32_t	hostTimeLeft;					/* Прошло времени с момента последней транзакции */	
	
	UartInit	lpUartInit;						/* Указатель на функцию инициализации порта */
	UartSend	lpUartSend;						/* Указатель на функцию отправки одного байта */
	UartRecv	lpUartRecv;						/* Указатель на функцию приема одного байта */
	//GetDeviceInfo lpGetDeviceInfo;				/* Указатель на функцию получения информации об устройстве */
	
	SetBit		lpSetTxEn;						/* Указатель на функцию вкл/выкл вывода TXEN RS485 */
	SetBit		lpSetRxEn;						/* Указатель на функцию вкл/выкл вывода RXEN RS485 */
	TimerCmd		lpTimerStart;					/* Указатель на функцию запуска таймера */
	TimerCmd		lpTimerReset;					/* Указатель на функцию сброса значения и остоновки таймера */
	IRRType		GetIntSourse;					/* Источник прервывания порта прием или передача   Tx -- 010   Rx -- 101 */
	
	
	int			passwordRegNum;					/* номер начального регистра для пароля */
	int			deviceInfoSizeRegNum;
	int			deviceInfoRegNum;				/* номер начального регистра для информации об устройстве */
	
	const char* password;
	int			passwordLen;
	

	
	uint32_t	errorCount;						/* количество ошибок протокола*/
	
	uint16_t	*lpRegisters;					/* Указатель на буфер регистров */
	uint16_t	registersCount;					/* количество регистров */
	

	 
	union {
		uint32_t value;
		struct {
			unsigned enableTimer 			: 1;	/* Флаг активности таймера */
			unsigned enableParseRxData 		: 1;	/* Флаг разрешения обработки принятого пакета */
			unsigned isParsedStart 			: 1;	/* Флаг, указывающий, что обработка текущего пакета начата */
			unsigned isRxDataMain 			: 1;	/* Флаг готовности принятых данных */
			unsigned isHostTimeout 			: 1;	/* Флаг таймаута связи с хостом */
			unsigned isConfigChangeLock 	: 1;	/* Флаг блокировки изменения конфигурации */
			unsigned eepromNeedUpdate 		: 1;	/* Флаг необходимости обновления энергонезависимых данных */
			unsigned isPasswordValid		: 1;   	/* Флаг правильного пароля */
			unsigned isPacket						: 1;		/*собран, готов к отправке */
		};
	} Flags;
	
} MbParam_t, *lpMbParam_t;
