open System
open AST
open Lexer

let rec REPL (env:Map<string,expr> list, res) =
    Console.WriteLine(to_str res)
    printfn "> "
    match Console.ReadLine() with
    | "q" -> ()
    | input -> 
        try
            let newEnv, res = input|> parse |> eval env 
            REPL (newEnv,res)
        with e -> REPL (env, Value(Var(e.Message)))
       
[<EntryPoint>]
let main argv =
    printfn("Little Interpreter in F#")
    printfn("Press 'q' to quit...")
    REPL (globalEnv,Value(Var(" ")))  

    0 
