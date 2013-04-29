### Overview

I had not heard of tail call optimisation until I read "Real-World Functional Programming" by
Tomas Petricek and Jon Skeet. Having read a bit about it in the context of F#, I was curious about
what the deal was with other languages like C# and C++. So I decided to experiment a bit.

### Links

* http://en.wikipedia.org/wiki/Tail_call
* [Real-World Functional Programming](http://www.manning.com/petricek/)

### C++

``` assembly
;  COMDAT ?FactorialHelper@@YAHHH@Z
_TEXT	SEGMENT
?FactorialHelper@@YAHHH@Z PROC				; FactorialHelper, COMDAT
; _x$ = ecx
; _acc$ = eax

; 21   : 	if (x <= 1) {

	cmp	ecx, 1
	jle	SHORT $LN7@FactorialH
$LL4@FactorialH:

; 22   : 		return acc;
; 23   : 	}
; 24   : 	return FactorialHelper(x - 1, x * acc);

	imul	eax, ecx
	dec	ecx
	cmp	ecx, 1
	jg	SHORT $LL4@FactorialH
$LN7@FactorialH:

; 25   : }

	ret	0
?FactorialHelper@@YAHHH@Z ENDP				; FactorialHelper
; Function compile flags: /Ogtp
_TEXT	ENDS
```

``` assembly
;	COMDAT ?Factorial@@YAHH@Z
_TEXT	SEGMENT
?Factorial@@YAHH@Z PROC					; Factorial, COMDAT
; _x$ = eax

; 17   : 	return FactorialHelper(x, 1);

	cmp	eax, 1
	jg	SHORT $LN3@Factorial
	mov	eax, 1

; 18   : }

	ret	0

; 17   : 	return FactorialHelper(x, 1);

$LN3@Factorial:
	lea	ecx, DWORD PTR [eax-1]
	jmp	?FactorialHelper@@YAHHH@Z		; FactorialHelper
?Factorial@@YAHH@Z ENDP					; Factorial
_TEXT	ENDS
```

### C&#35;

```
```

### F&#35;

``` csharp
internal static int factorialHelper@2(int x, int acc)
{
    while (x > 1)
    {
        acc = x * acc;
        x--;
    }
    return acc;
}
```
