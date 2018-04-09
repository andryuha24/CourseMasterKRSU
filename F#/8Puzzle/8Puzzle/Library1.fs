module Puzzle
open System
open System.Numerics
open System.Collections.Generic


let goalState = [| 1; 2; 3; 4; 5; 6; 7; 8; 0 |]

let rows = 3
let columns = 3

let column j = j % columns
let row i = i / columns
let index (c,r) = r * rows + c

type field = int list

type State =
    { Parent: State option;
      field: int list;
      Depth: int;
      Score: int; }

let isAdjacentTuple (x1, y1) (x2, y2) =
    let diff a b = abs(a - b)
    let (s1, s2) = (diff x1 x2, diff y1 y2)
    (s1 + s2 = 1) && (s1 * s2 = 0)

let isAdjacent i j = i <> j && isAdjacentTuple (column i, row i) (column j, row j)


let move (field:int array) tileId =
    let j = Seq.findIndex ((=) 0) field
    let elem = Seq.item tileId field
    if isAdjacent tileId j then 
        let res = List.map (fun i -> match i with 
                                        | 0 -> elem
                                        | x when x = elem -> 0
                                        | _ -> i) (Array.toList field)
        List.toArray res
    else
        field 

let children_nodes field = 
    let i = Seq.findIndex ((=) 0) field
    let x, y = column i, row i
    let swap b x y = b y x

    [(x-1, y); (x+1, y); (x, y-1); (x, y+1)]
        |> Seq.filter (fun (c, r) -> c >= 0 && r >= 0 && c < columns && r < rows)
        |> Seq.map (index >> swap Seq.item field) 
        |> Seq.map (fun n -> List.map (fun i -> match i with
                                                    | 0 -> n
                                                    | x when x = n -> 0
                                                    | _ -> i) field)
        |> Seq.toList

let rec shuffle (field:int array) n =
    let rnd = Random()
    let res = Seq.fold (fun f _ -> let c = children_nodes f
                                   Seq.item (rnd.Next(0, Seq.length c)) c) (Array.toList field) (seq{1..n})
    List.toArray res


let manhattanDistance a b = a |> Seq.mapi (fun i v -> (i, Seq.findIndex ((=) v) b))
                              |> Seq.map (fun (i, j) -> abs (column i - column j) + abs (row i - row j))
                              |> Seq.sum

let solve (start:int array) (goal:int array) =
    let closedSet = List<field>()
    let openSet = List<State>()
    let s = { Parent = None; field = (Array.toList start); Depth = 0; Score = manhattanDistance start goal; }
    openSet.Add(s)
    let rec search() =
        openSet.Sort({new IComparer<State> with member x.Compare(a, b) = a.Score.CompareTo(b.Score)})
        let candidate = openSet.Item(0)
        closedSet.Add(candidate.field)
        openSet.RemoveAt(0)
        if candidate.field = (Array.toList goal) then
            candidate
        else
            let a = children_nodes candidate.field
            let children =
                    children_nodes candidate.field
                    |> Seq.filter (fun f -> closedSet.Contains(f) |> not)
                    |> Seq.map (fun f -> {  Parent = Some candidate;
                                            field = f;
                                            Depth = candidate.Depth + 1;
                                            Score = (candidate.Depth + 1) + (manhattanDistance f goal); })
            for c in children do
                openSet.Add(c)
            search()

    let res = search()|>Some
                      |> Seq.unfold (fun s ->  match s with
                                                | None    -> None
                                                | Some s' -> Some (List.toArray s'.field, s'.Parent))
                      |> Seq.toArray |> Array.rev
    res