//==============================================================================
//
//==============================================================================

#ifndef _MEDIAN_FILTER_H_
#define _MEDIAN_FILTER_H_

#include <stdint.h>
#include <stdlib.h>
#include <stdbool.h>

typedef struct PAIR_STRUCT_U16 {
	void *lpNext;
	uint16_t value;
} PairU16_t;

typedef struct MEDIAN_FILTER_U16_DATA_T {
	
	PairU16_t *lpBuf;
	uint32_t bufSize;
	
	PairU16_t *lpDataPoint;
	PairU16_t small;
	PairU16_t big;	
	
	uint16_t stopValue;
	uint16_t value;
	bool isUsingAddCriticalSection;
	bool isInit;
} MedianFltrU16Data_t, *lpMedianU16FltrData_t;

void u16_med_fltr_init( lpMedianU16FltrData_t lpData, PairU16_t *lpBuf, uint32_t bufSize );

void u16_med_fltr_clear( lpMedianU16FltrData_t lpData );

uint16_t u16_med_fltr_add( lpMedianU16FltrData_t lpData, uint16_t value );

//------------------------------------------------------------------------------
// Медианный фильтр, работающий по алгоритму сортировки

typedef struct { // MedianFltrSrtU16Data_t
	uint16_t *lpBuf;
	uint16_t *lpWrPtr;
	uint32_t bufSize;	
	
	uint16_t stopValue;
	uint16_t value;
	bool isUsingAddCriticalSection;
	bool isInit;
} MedianFltrSrtU16Data_t, *lpMedianStrU16FltrData_t;

void u16_med_fltr_srt_init( lpMedianStrU16FltrData_t lpData, uint16_t *lpBuf, uint32_t bufSize );

void u16_med_fltr_srt_clear( lpMedianStrU16FltrData_t lpData );

uint16_t u16_med_fltr_srt_add( lpMedianStrU16FltrData_t lpData, uint16_t value );

#endif // _MEDIAN_FILTER_H_