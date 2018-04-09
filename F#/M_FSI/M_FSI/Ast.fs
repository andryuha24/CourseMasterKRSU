module AST
open System
open System.Collections.Generic

type ValueType = 
    |Number of int
    |Var of string
    |Boolean of bool

type expr = 
    |Value of ValueType
    |Symbol of string
    |Fnc of expr list * expr list
    |DefFun of (Map<string,expr> list -> expr list -> Map<string,expr> list * expr)
    |List of expr list

let rec lookup (env:Map<string,expr> list) s =
    match env with
    | []        -> None
    | h::t      -> 
        match (h.TryFind s) with
        | Some(e)       -> Some(e)
        | None          -> lookup t s

let expandEnv (env:Map<string,expr> list) =
    List.append [Map.empty<string,expr>] env

let rec buildVarList exprs =
    match exprs with
    | []              -> []
    | Symbol(v) :: t  -> Value(Var(v)) :: buildVarList t
    | Value(v) :: t   -> Value(v) :: buildVarList t
    | _               -> failwith "invalid define arguments"

let setEnv str expr (env:Map<string,expr> list) =
      match env with
      | head::tail    -> List.append [head.Add(str, expr)] tail
      | []            -> failwith "empty environment"

let getNameFunc exprs =
    match exprs with
    | Symbol(n) -> n
    | Value(Var(n)) -> n
    | _   -> failwith "invalid define arguments"
    
let rec to_str result =
    match result with
    | Value(Number(n))  -> n.ToString()
    | Value(Boolean(b)) -> if b then "#t" else "#f"
    | Value(Var(s)) -> s
    | List (l) ->  List.map to_str l |> String.concat " " |> sprintf "(%s)"
    | _-> "null"


let rec eval (env:Map<string,expr> list) expr =
    match expr with
    | Value(v) -> (env, Value(v))
    | Symbol(s) ->
        match lookup env s with
            | Some(e) -> (env, e)
            | None -> failwith (sprintf "unbound symbol '%A'" s)
    | List(h::t) -> 
         match eval env h with
         | (env', DefFun(f)) -> f env t
         | (env', Fnc(args, code))  -> 
            let newEnv =
               try List.fold2 bindArg (expandEnv env') args t
               with ex -> failwith "invalid number of arguments"
            evalExprs newEnv code
         | (nenv, expr) -> (nenv, expr)
    | Fnc(args, code)  -> failwith "invalid function call"
    | _ -> failwith "invalid input type"

and  bindArg env arg expr = 
      match arg with
      | Value(Var(n))   -> setEnv n (snd(eval env expr)) env
      | _         -> failwith "invalid argument"

and evalExprs env exprs = 
      match exprs with
      | []        -> (env, List([]))
      | h :: t    ->
         let resEnv, res = eval env h
         match t.Length with
         | 0     -> (resEnv, res)
         | 1     -> eval resEnv t.Head
         | _     -> evalExprs resEnv t

let mapeval exps env =
    List.map (fun exp -> snd(eval env exp)) exps

let _not env args =
    match mapeval args env with
    | [Value(Boolean (b))] -> (env,Value(Boolean (not b)))
    | [e] -> (env,Value(Boolean (false)))
    | _ ->  failwith "not"

let compare op env args  =
    match mapeval args env with
    | [Value(Number (l)); Value(Number (r))] -> (env,Value(Boolean (op l r)))
    | _ -> failwith (op.ToString ())

let logicOp op env args  =
    match mapeval args env with
    | [Value(Boolean (l)); Value(Boolean (r))] -> (env,Value(Boolean (op l r)))
    | _ -> failwith (op.ToString ())

let if' env args =
    match args with
    | [cond; then'] -> 
        match snd(eval env cond) with
        | Value(Boolean (b)) -> if b then (eval  env then') else (env,List ([]))
        | _ -> eval env then'
    | [cond; then'; else'] ->
        match snd(eval env cond) with
        | Value(Boolean (b)) -> eval env (if b then then' else else') 
        | _ -> eval env then'
    | _ -> failwith "if"

let mathFun op env exps = 
    let math op env = function
        | []        -> failwith "arithmetric error"
        | e :: t    ->
            match snd(eval env e) with
            | Value(Number(v)) ->
                t |> List.fold(fun acc expr -> 
                    match snd(eval env expr) with 
                    | Value(Number(a))  -> op acc a
                    | _          -> failwith "arithmetric error") v
            | _ -> failwith "arithmetric error"
    (env, Value(Number(math op env exps)))

let _define env exprs = 
    match exprs with
    | Symbol(n) :: expr :: []     ->
        let env', res = eval env expr                           
        (setEnv n res env', List([]))
    | List(names) :: code ->
        let funcname = getNameFunc names.Head
        let args = buildVarList names.Tail
        (setEnv funcname (Fnc(args, code)) env, List([]))
    | _   -> failwith "invalid define arguments"

let globalEnv = 
    [ Map.ofList([  ("+", DefFun(mathFun (+)));
                    ("-", DefFun(mathFun (-)));
                    ("*", DefFun(mathFun (*)));
                    ("/", DefFun(mathFun (/)));
                    ("<", DefFun (compare (<)));
                    (">", DefFun (compare (>)));
                    ("=", DefFun (compare (=)));
                    ("not", DefFun (_not));
                    ("and", DefFun (logicOp (&&)));
                    ("or", DefFun (logicOp (||)));
                    ("if", DefFun (if'));
                    ("define",  DefFun(_define));
    ])]