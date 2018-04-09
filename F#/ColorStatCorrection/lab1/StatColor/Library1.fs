module ColorCorrect  
open System
open System.Drawing
open MathNet.Numerics.LinearAlgebra
open UtilityMatrixAndType

let rgbToFloat (c:Color) lab=
    let min = 3.0/255.0
    let colorToFloat (channelC:byte) = if float channelC/255.0 < min then min else float channelC/255.0
    let color = [|float c.R /255.0; float c.G /255.0; float c.B / 255.0|]
    match lab with
    |true -> matrix[[colorToFloat c.R]
                    [colorToFloat c.G]
                    [colorToFloat c.B]]
    | _ -> matrix[[color.[0]]
                  [color.[1]]
                  [color.[2]]]
   
let FloatToColor (colorMatrix:Mat) =
    let FloatToColor' chanellC = 
        match chanellC with
        |c when c > 1.0 -> 1.0
        |c when c < 0.0 -> 0.0
        |_ -> chanellC
    matrix[[FloatToColor' colorMatrix.[0,0]]
           [FloatToColor' colorMatrix.[1,0]]
           [FloatToColor' colorMatrix.[2,0]]]  


let lmsMatrix color = convertToLmsMatrix * rgbToFloat color true

let labMatrix color = convertLabMatrix1 * convertLabMatrix2 * Matrix.Log10 (lmsMatrix color)

let ColorMatrixWithContrast (colorMat:Mat) (contrast:bool) (expV1:Mat) (disV1:Mat) (expV2:Mat) (disV2:Mat)=
     let temp = (Array.map2(fun i j -> (i/j))  (disV1.ToColumnMajorArray()) (disV2.ToColumnMajorArray()))
     let k = DenseMatrix.raw temp.Length 1 temp
     match contrast with
     |true -> expV1 + ((colorMat - expV2)|>Matrix.mapi (fun i j value -> value*k.[i,j]))
     |false -> expV1 + (colorMat - expV2)

let lmsMatrix' color (contrast:bool) (expV1:Mat) (disV1:Mat) (expV2:Mat) (disV2:Mat)= 
    (convertLabMatrix2.Transpose()) * convertLabMatrix1 * (ColorMatrixWithContrast color contrast expV1 disV1 expV2 disV2)

let lab2rgb color (contrast:bool) (expV1:Mat) (disV1:Mat) (expV2:Mat) (disV2:Mat)= 
    let labColor = labMatrix color
    255.0 * FloatToColor (convertLabToRGBMatrix * 
        ((lmsMatrix' labColor contrast expV1 disV1 expV2 disV2)|> Matrix.map(fun e -> Math.Pow(10.0,e)))) 

let labColors (bmp : Bitmap) = 
    [|for y in 0..bmp.Height-1 do
        for x in 0..bmp.Width-1 -> labMatrix (bmp.GetPixel(x,y))|] 

let sumColors (bmp : Bitmap) (arrColors:Mat[]) = Array.fold(fun acc e->acc+e) emptyMatrix arrColors

let getExpectVal (bmp : Bitmap) (colorMatrix:Mat) = 
     colorMatrix|>  Matrix.map(fun e -> e / float (bmp.Width * bmp.Height))

let sumSqrColors (bmp : Bitmap) (arrColors:Mat[])  expectValOfImg = 
    Array.fold(fun acc e1 ->
        acc + ((e1-expectValOfImg)|> Matrix.map(fun e -> Math.Pow(e,2.0)))) emptyMatrix arrColors

let getDis (bmp : Bitmap) (colorMatrix:Mat) = 
    colorMatrix|>  Matrix.map(fun e -> Math.Sqrt(e / float (bmp.Width * bmp.Height)))

//Non-Parallel
let rgb2lab (bmp1 : Bitmap) (bmp2 : Bitmap) (contrast:bool) = 
    let resE1 = getExpectVal bmp1 (sumColors bmp1 (labColors bmp1))
    let resDis1 = getDis bmp1 (sumSqrColors bmp1 (labColors bmp1) resE1)

    let resE2 = getExpectVal bmp2 (sumColors bmp2 (labColors bmp2))
    let resDis2 = getDis bmp2 (sumSqrColors bmp2 (labColors bmp2) resE2)
    using (new LockContext(bmp2))
        (fun lockContext ->
            for y in 0..bmp2.Height-1 do
                for x in 0..bmp2.Width-1 do
                    let newColor = lab2rgb (lockContext.GetPixel(x,y)) contrast resE1 resDis1 resE2 resDis2
                    lockContext.SetPixel(x,y,Color.FromArgb((int)newColor.[0,0],(int)newColor.[1,0],(int)newColor.[2,0]))
                   )
    bmp2


let rgb2HSlColor color = 
    let floatColor = rgbToFloat color false
    let min = Math.Min(floatColor.[0,0],Math.Min(floatColor.[1,0],floatColor.[2,0]))
    let max = Math.Max(floatColor.[0,0],Math.Max(floatColor.[1,0],floatColor.[2,0]))
    let L = (max + min) / 2.0
    let dif = max-min

    let H (c:Mat) mx mn= 
        match mx,mn with
        |x,y when x=y -> 0.0
        |x,y when x = c.[0,0] && c.[1,0] >= c.[2,0] -> 60.0 * (c.[1,0] - c.[2,0]) / (x - y)
        |x,y when x = c.[0,0] && c.[1,0] < c.[2,0] -> 60.0 * (c.[1,0] - c.[2,0]) / (x - y) + 360.0
        |x,y when x = c.[1,0] -> 60.0 * (c.[2,0] - c.[0,0]) / (x - y) + 120.0      
        |x,y when x = c.[2,0] -> 60.0 * (c.[0,0] - c.[1,0]) / (x - y) + 240.0

    let S l mx mn =
        match l,mx,mn with
        |x,y,z when x= 0.0 || x=y -> 0.0
        |x,y,z when x > 0.0 && x <= 0.5 -> (y - z) / (y + z)
        |x,y,z when x > 0.5 -> (y - z) / (2.0 - (y + z))

    matrix[[H (floatColor) max min]
           [S L max min]
           [L]]

let hslColors (bmp : Bitmap) = 
    [|for y in 0..bmp.Height-1 do
        for x in 0..bmp.Width-1 -> rgb2HSlColor (bmp.GetPixel(x,y))|] 

let hsl2rgbColor (hsl:Mat) =
    match hsl.[1,0] with
    |0.0 -> 255.0*matrix[[hsl.[2,0]]
                         [hsl.[2,0]]
                         [hsl.[2,0]]]
    | _ -> 
         let Q = if hsl.[2,0] <0.5 then (hsl.[2,0]*(1.0+hsl.[1,0])) else (hsl.[2,0]+hsl.[1,0]-(hsl.[2,0]*hsl.[1,0]))
         let P = (2.0*hsl.[2,0]) - Q;
         let HK = hsl.[0,0] / 360.0;
         let T = [|HK+(1.0/3.0);HK;HK-(1.0/3.0)|]

         let res = T |> Array.map(fun e -> if e < 0.0 then e + 1.0 else e)
                     |> Array.map(fun e -> if e > 1.0 then e - 1.0 else e)
                     |> Array.map(fun e -> match e with
                                            | x when x*6.0 < 1.0 -> P+((Q-P)*6.0*x)
                                            | x when x*2.0 < 1.0 -> Q
                                            | x when x*3.0 < 2.0 -> P+(Q-P)*((2.0/3.0)-x)*6.0
                                            | _ -> P) 

         matrix[[res.[0]]
                [res.[1]]
                [res.[2]]]


let hsl2rgb color (contrast:bool) (expV1:Mat) (disV1:Mat) (expV2:Mat) (disV2:Mat)= 
    let hslColor = rgb2HSlColor color
    255.0 * FloatToColor (hsl2rgbColor (ColorMatrixWithContrast hslColor contrast expV1 disV1 expV2 disV2))

let rgb2hsl (bmp1 : Bitmap) (bmp2 : Bitmap) (contrast:bool) = 

    let resE1 = getExpectVal bmp1 (sumColors bmp1 (hslColors bmp1))
    let resDis1 = getDis bmp1 (sumSqrColors bmp1 (hslColors bmp1) resE1)
    let resE2 = getExpectVal bmp2 (sumColors bmp2 (hslColors bmp2))
    let resDis2 = getDis bmp2 (sumSqrColors bmp2 (hslColors bmp2) resE2)
    for y in 0..bmp2.Height-1 do
        for x in 0..bmp2.Width-1 do
            let newColor = hsl2rgb (bmp2.GetPixel(x,y)) contrast resE1 resDis1 resE2 resDis2
            bmp2.SetPixel(x,y,Color.FromArgb((int)newColor.[0,0],(int)newColor.[1,0],(int)newColor.[2,0]))
    bmp2