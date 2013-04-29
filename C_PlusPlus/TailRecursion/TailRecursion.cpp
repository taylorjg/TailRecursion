#include "stdafx.h"

static int Factorial(int x);
static int FactorialHelper(int x, int acc);

int _tmain(int argc, _TCHAR* argv[])
{
	(void)_tprintf(_T("TailRecursion (C++)\n"));
	for (int x = 1; x <= 10; x++) {
		(void)_tprintf(_T("Factorial(%d): %d\n"), x, Factorial(x));
	}

	return 0;
}

static int Factorial(int x) {
	return FactorialHelper(x, 1);
}

static int FactorialHelper(int x, int acc) {
	if (x <= 1) {
		return acc;
	}
	return FactorialHelper(x - 1, x * acc);
}
