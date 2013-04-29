### Overview

I had not heard of tail call optimisation until I read "Real-World Functional Programming" by
Tomas Petricek and Jon Skeet. Having read a bit about it in the context of F#, I was curious about
what the deal was with other languages like C# and C++. So I decided to experiment a bit.

### Links

* http://en.wikipedia.org/wiki/Tail_call
* [Real-World Functional Programming](http://www.manning.com/petricek/)

### C++

By enabling the "Assembly With Source Code" option for the "Assembler Output" setting on the Output Files tab,
we can view the generated assembly together with the source code. It looks like the recursion inside FactorialHelper
has been replaced with a loop (see the jg SHORT $LL4@FactorialH).

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

Interestingly, the Factorial method invokes the FactorialHelper method by jumping to it instead of calling it
(see jmp ?FactorialHelper@@YAHHH@Z).

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

Looking at the IL for the FactorialHelper method, it can clearly be seen that there is a call instruction.

```
.method private hidebysig static 
    int32 FactorialHelper (
        int32 x,
        int32 acc
    ) cil managed 
{
    IL_0000: ldarg.0
    IL_0001: ldc.i4.1
    IL_0002: bgt.s IL_0006

    IL_0004: ldarg.1
    IL_0005: ret

    IL_0006: ldarg.0
    IL_0007: ldc.i4.1
    IL_0008: sub
    IL_0009: ldarg.0
    IL_000a: ldarg.1
    IL_000b: mul
    IL_000c: call int32 TailRecursion.Program::FactorialHelper(int32,  int32)
    IL_0011: ret
}
```

### F&#35;

Using JustDecompile to examine the F# assembly in C#, it can be seen that the
recursion has been replaced with a loop.

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
