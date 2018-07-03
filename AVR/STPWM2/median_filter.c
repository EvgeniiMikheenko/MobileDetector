//==============================================================================
//
//==============================================================================

#include "median_filter.h"
#include "..\utils.h"

static inline bool u16_med_fltr_check_valid_data( lpMedianU16FltrData_t lpData ) {
	if(lpData == (lpMedianU16FltrData_t)NULL)
		return false;
	
	if((lpData->bufSize == 0) || (lpData->lpBuf == NULL))
		return false;
	
	if(!lpData->isInit)
		return false;
	
	return true;
}

void u16_med_fltr_init( lpMedianU16FltrData_t lpData, PairU16_t *lpBuf, uint32_t bufSize ) {
	if(lpData == NULL)
		return;
	
	lpData->isInit = false;
	
	if((bufSize == 0) || (lpBuf == NULL))
		return;
	
	lpData->lpBuf = lpBuf;
	lpData->bufSize = bufSize;
	//
	u16_med_fltr_clear( lpData );
	
	lpData->value = 0;
	lpData->isInit = true;
}

void u16_med_fltr_clear( lpMedianU16FltrData_t lpData ) {
	if(lpData == (lpMedianU16FltrData_t)NULL)
		return;
	
	if((lpData->bufSize == 0) || (lpData->lpBuf == NULL)) {
		lpData->isInit = false;
		return;
	}
	
	if( lpData->isUsingAddCriticalSection )
		EnterCritSection();
	
	for(uint32_t i = 0; i < lpData->bufSize; i++) {
		lpData->lpBuf[i].value = 0;
		lpData->lpBuf[i].lpNext = (void*)NULL;
	}	
	
	lpData->lpDataPoint = lpData->lpBuf;
					
	lpData->small.lpNext = NULL;
	lpData->small.value = 0;
						
	lpData->big.lpNext = &lpData->small;
	lpData->big.value = 0;
	
	lpData->value = 0;
	
	if( lpData->isUsingAddCriticalSection )
		ExitCritSection();
}

uint16_t u16_med_fltr_add( lpMedianU16FltrData_t lpData, uint16_t value ) {
	
	if(!u16_med_fltr_check_valid_data(lpData))
		return 0;
	
	PairU16_t *successor;
	PairU16_t *scan;
	PairU16_t *scanOld;
	PairU16_t *median;
	
	if( lpData->isUsingAddCriticalSection )
		EnterCritSection();
	
	if( value == lpData->stopValue)
		value++;
	
	lpData->lpDataPoint++;
	
	if( lpData->lpDataPoint >= (lpData->lpBuf + lpData->bufSize)) 
		lpData->lpDataPoint = lpData->lpBuf;
				
	lpData->lpDataPoint->value = value;
	
	successor = lpData->lpDataPoint->lpNext;
	median = &lpData->big;
	scanOld = NULL;
	scan = &lpData->big;
	
	if( scan->lpNext == lpData->lpDataPoint )
		scan->lpNext = successor;
	
	scanOld = scan;
	scan = scan->lpNext;
	
	for(uint32_t i = 0; i < lpData->bufSize; i++) {
					
		if( scan->lpNext == lpData->lpDataPoint )
			scan->lpNext = successor;
						
		if( scan->value < value ) {
			lpData->lpDataPoint->lpNext = scanOld->lpNext;
			scanOld->lpNext = lpData->lpDataPoint;
			value = lpData->stopValue;
		}
						
		median = median->lpNext;
		if(scan == &lpData->small)
			break;
			
		scanOld = scan;
		scan = scan->lpNext;
						
		if( scan->lpNext == lpData->lpDataPoint )
			scan->lpNext = successor;
					
		if( scan->value < value ) {
			lpData->lpDataPoint->lpNext = scanOld->lpNext;
			scanOld->lpNext = lpData->lpDataPoint;
			value = lpData->stopValue;
		}
						
		if(scan == &lpData->small)
			break;
						
		scanOld = scan;
		scan = scan->lpNext;
	}
	lpData->value = median->value;
	
	if( lpData->isUsingAddCriticalSection )
		ExitCritSection();
	
	return lpData->value;
}

//------------------------------------------------------------------------------
// Медианный фильтр, работающий по алгоритму сортировки

static inline bool u16_med_fltr_srt_check_valid_data( lpMedianStrU16FltrData_t lpData ) {
	if(lpData == (lpMedianStrU16FltrData_t)NULL)
		return false;
	
	if((lpData->bufSize == 0) || (lpData->lpBuf == NULL))
		return false;
	
	if(!lpData->isInit)
		return false;
	
	return true;
}

void u16_med_fltr_srt_init( lpMedianStrU16FltrData_t lpData, uint16_t *lpBuf, uint32_t bufSize ) {
	if(lpData == NULL)
		return;
	
	lpData->isInit = false;
	
	if((bufSize == 0) || (lpBuf == NULL))
		return;
	
	lpData->lpBuf = lpBuf;
	lpData->bufSize = bufSize;
	//
	u16_med_fltr_srt_clear( lpData );
	
	lpData->value = 0;
	lpData->isInit = true;
}

void u16_med_fltr_srt_clear( lpMedianStrU16FltrData_t lpData ) {
	if(lpData == (lpMedianStrU16FltrData_t)NULL)
		return;
	
	if((lpData->bufSize == 0) || (lpData->lpBuf == NULL)) {
		lpData->isInit = false;
		return;
	}
	
	uint16_t *ptr = lpData->lpBuf;
	uint16_t *ptrEnd = lpData->lpBuf + lpData->bufSize;
	
	if( lpData->isUsingAddCriticalSection )
		EnterCritSection();
	
	for( ; ptr < ptrEnd; ptr++) {
		*ptr = 0;
	}	
	
	lpData->lpWrPtr = lpData->lpBuf;
	lpData->value = 0;
	
	if( lpData->isUsingAddCriticalSection )
		ExitCritSection();
}

uint16_t u16_med_fltr_srt_add( lpMedianStrU16FltrData_t lpData, uint16_t value ) {
	if( !u16_med_fltr_srt_check_valid_data( lpData ) )
		return 0;
	
	uint16_t tmp;
	uint16_t *ptr1 = lpData->lpBuf;
	uint16_t *ptr2;
	uint16_t *endPtr = lpData->lpBuf + lpData->bufSize;
	
	if( lpData->isUsingAddCriticalSection )
		EnterCritSection();
	
	*lpData->lpWrPtr = value;
	lpData->lpWrPtr++;
	if(lpData->lpWrPtr >= (lpData->lpBuf + lpData->bufSize))
		lpData->lpWrPtr = lpData->lpBuf;
	
	for( ; ptr1 < endPtr; ptr1++) {
		for(ptr2 = lpData->lpBuf; ptr2 < endPtr; ptr2++) {
			if( (*ptr2) > (*ptr1))
				continue;
			//
			tmp = *ptr2;
			*ptr2 = *ptr1;
			*ptr1 = tmp;
		}
	}
	
	lpData->value = lpData->lpBuf[lpData->bufSize >> 1];
	
	if( lpData->isUsingAddCriticalSection )
		ExitCritSection();
	
	return lpData->value;
}
