module MyFractal
open System
open System.Numerics
open System.Drawing


let iterations = 64
let epsilon = 0.0000001

let f x = x*x*x - Complex(1.0, 0.0)

let fd x =  Complex(3.0, 0.0) * x*x

let compute_pixel (z:Complex) =
  let rec loop z n =
    let z1 = z - (f z) / (fd z)
    if (z1 - z).Magnitude < epsilon || n > iterations then (z1,n) else loop z1 (n+1)
  loop z 0

let center = new Complex(0.0, 0.0)

let tc x w scale = (float x - 0.5*float w) / scale
let transform (x,y) width height scale = center + Complex(tc x width scale, tc y height scale)

let grayScale l =
  let toIntColor f = 255. * f |> int |> min 255 |> max 0
  (l|> toIntColor, l |> toIntColor, l|> toIntColor)

let complex_to_color (z:Complex,n) =
  let (r,g,b) = grayScale (0.8 / (1.+0.18*float n))
  Color.FromArgb(r, g, b)

let newton z = compute_pixel z
let newton_pixel x y w h s = transform (x,y) w h s |> newton |> complex_to_color

let zig (x1,y1) (x2,y2) = (x1+x2+y1-y2)/2, (x2-x1+y1+y2)/2
let zag (x1,y1) (x2,y2) = (x1+x2-y1+y2)/2, (x1-x2+y1+y2)/2
 
let rec dragon p1 p2 p3 n =
   if n = 0 then [p1;p2] else
   (dragon p1 (zig p1 p2) p2 (n-1)) @ (dragon p2 (zag p2 p3) p3 (n-1))