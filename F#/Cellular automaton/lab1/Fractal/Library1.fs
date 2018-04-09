module CellAutomata
open System
open System.Numerics
open System.Drawing

type state = A | B | C | E | F | J
type dir = North | East | South | West

let turn_left = function
  | North -> West
  | East  -> North
  | South -> East
  | West  -> South
 
let turn_right = function
  | North -> East
  | East  -> South
  | South -> West
  | West  -> North
 
let move (x, y) = function
  | North -> x, y - 1
  | East  -> x + 1, y
  | South -> x, y + 1
  | West  -> x - 1, y


let turmite s c (x,y) dir=
        match s,c with
        | A, ConsoleColor.Black -> (B, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | B, ConsoleColor.Black  -> (C, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | C, ConsoleColor.Black  -> (E, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | E, ConsoleColor.Black  -> (F, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | F, ConsoleColor.Black  -> (J, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | J, ConsoleColor.Black  -> 
            let newdir = turn_left dir  in
            (A, ConsoleColor.Magenta, (move (x,y) newdir), newdir)
        | A, ConsoleColor.Yellow -> (A, ConsoleColor.Magenta, (move (x,y) dir), dir)
        | A, ConsoleColor.Magenta -> (A, ConsoleColor.Yellow, (move (x,y) dir), dir)
        | J, ConsoleColor.Yellow -> (A, ConsoleColor.Magenta, (move (x,y) dir), dir)
        | J, ConsoleColor.Magenta  -> 
            let newdir = turn_right dir  in
            (A, ConsoleColor.Yellow, (move (x,y) newdir), newdir)
        |_-> (s,c, (x,y), dir)