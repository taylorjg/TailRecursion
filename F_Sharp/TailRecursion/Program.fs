let factorial x =
    let rec factorialHelper x acc = 
        if x <= 1 then
            acc
        else
            factorialHelper (x - 1) (x * acc)
    factorialHelper x 1

printfn "TailRecursion (F#)"

for x in [1..10] do
    printfn "factorial(%d): %d" x (factorial x)
