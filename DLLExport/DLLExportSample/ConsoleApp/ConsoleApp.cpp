// ConsoleApp.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <iostream>

using namespace std;

extern "C" __declspec(dllimport) int __cdecl Add(int a, int b);
extern "C" __declspec(dllimport) int __cdecl Subtract(int a, int b);
extern "C" __declspec(dllimport) char * __cdecl FormatString(char *pszFormatString, int value);


int _tmain(int argc, _TCHAR* argv[])
{
	int addResult = Add(5, 6);

	addResult = Subtract(addResult, 1);

	cout << FormatString("The value is {0}", addResult) << "\n";

	getchar();

	return 0;
}

