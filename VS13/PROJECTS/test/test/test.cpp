// test.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <Windows.h>
#include <stdio.h>

int _tmain(int argc, _TCHAR* argv[])
{
	char data_bytes[8];

	for(int i=0; i<= 7; i++){
		data_bytes[i]=0x41+i;
	};





	return 0;
}

