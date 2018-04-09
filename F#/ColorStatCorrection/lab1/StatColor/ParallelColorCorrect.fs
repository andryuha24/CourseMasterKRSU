module ParallelColorCorrect
open ColorCorrect
open UtilityMatrixAndType
open System.Drawing
open MathNet.Numerics.LinearAlgebra
open System
open System.Drawing
open System.Drawing.Imaging

let chunksOfImage (bmp:Bitmap) = [|((0,bmp.Width/2-1),(0,bmp.Height/2-1));
                                    ((bmp.Width/2,bmp.Width-1),(0,bmp.Height/2-1));
                                    ((0,bmp.Width/2-1),(bmp.Height/2,bmp.Height-1));
                                    ((bmp.Width/2,bmp.Width-1),(bmp.Height/2,bmp.Height-1));|]


let labColorsParallel (bmp : Bitmap) (x,y) = 
    using (new LockContext(bmp))
        (fun lockContext ->
            [|for y' in fst y..snd y do
                for x' in fst x..snd x -> 
                    labMatrix (lockContext.GetPixel(x', y'))|] )

let sumlabColorsAsync (nameImg:string) (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let labColorsOfImg = labColorsParallel bmp (x,y)
        return  labColorsOfImg |>Array.fold(+) emptyMatrix
    }

let sumlabColorsParallel nameImg chunksImage  =
    chunksImage
    |> Seq.map (sumlabColorsAsync nameImg)  
    |> Async.Parallel   
    |> Async.RunSynchronously 

let sumSqrlabColorsAsync (nameImg:string) expectValOfImg (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let labColorsOfImg = labColorsParallel bmp (x,y)
        return   Array.fold(fun acc e1 ->
                             acc + ((e1-expectValOfImg)
                                    |> Matrix.map(fun e -> Math.Pow(e,2.0)))) emptyMatrix labColorsOfImg 
    }

let sumSqrlabColorsParallel nameImg expectValOfImg chunksImage  =
    chunksImage
    |> Seq.map (sumSqrlabColorsAsync nameImg expectValOfImg)  
    |> Async.Parallel   
    |> Async.RunSynchronously 


let lab2ColorsParallel (bmp : Bitmap) (x,y) contrast resE1 resDis1 resE2 resDis2 = 
    using (new LockContext(bmp))
        (fun lockContext ->
            [|for y' in fst y..snd y do
                for x' in fst x..snd x -> 
                    let newColor = lab2rgb (lockContext.GetPixel(x',y')) contrast resE1 resDis1 resE2 resDis2
                    x',y',newColor|])

let lab2ColorsAsync (nameImg:string)  contrast resE1 resDis1 resE2 resDis2 (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let res = lab2ColorsParallel bmp (x,y) contrast resE1 resDis1 resE2 resDis2
        return res
    }

let reslabColorsParallel nameImg contrast resE1 resDis1 resE2 resDis2 chunksImage  =
    chunksImage
    |> Seq.map (lab2ColorsAsync nameImg contrast resE1 resDis1 resE2 resDis2)  
    |> Async.Parallel   
    |> Async.RunSynchronously 

let rgb2labParallel (bmp1 : BMP) (bmp2 : BMP) (contrast:bool) = 
    
    let res1 = sumlabColorsParallel bmp1.Path (chunksOfImage bmp1.Img)
    let resPllExpect1 = getExpectVal bmp1.Img ((res1) |>Array.fold(+) emptyMatrix)
    let res'1 =  sumSqrlabColorsParallel bmp1.Path resPllExpect1 (chunksOfImage bmp1.Img)
    let resPllDisp1 = getDis bmp1.Img ((res'1) |>Array.fold(+) emptyMatrix)


    let res2 = sumlabColorsParallel bmp2.Path (chunksOfImage bmp2.Img)
    let resPllExpect2 = getExpectVal bmp2.Img ((res2) |>Array.fold(+) emptyMatrix)
    let res'2 =  sumSqrlabColorsParallel bmp2.Path resPllExpect2 (chunksOfImage bmp2.Img)
    let resPllDisp2 = getDis bmp2.Img ((res'2) |>Array.fold(+) emptyMatrix)
    
    let reslab2Color = 
        reslabColorsParallel bmp2.Path contrast resPllExpect1 resPllDisp1 resPllExpect2 resPllDisp2 (chunksOfImage bmp2.Img)
    let resBmp = new Bitmap(bmp2.Img.Width, bmp2.Img.Height,PixelFormat.Format24bppRgb)
    let resData = Array.fold(fun acc e -> Array.concat[acc;e] ) [||] reslab2Color
    using (new LockContext(resBmp)) 
        (fun lockContext ->
            resData|> Seq.iter (fun (x,y,c) -> 
                            lockContext.SetPixel(x, y, Color.FromArgb((int)c.[0,0],(int)c.[1,0],(int)c.[2,0]))))
    resBmp


let hslColorsParallel (bmp : Bitmap) (x,y) = 
    using (new LockContext(bmp))
        (fun lockContext ->
            [|for y' in fst y..snd y do
                for x' in fst x..snd x -> 
                    rgb2HSlColor (lockContext.GetPixel(x', y'))|] )

let sumHslColorsAsync (nameImg:string) (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let hlsColorsOfImg = hslColorsParallel bmp (x,y)
        return  hlsColorsOfImg |>Array.fold(+) emptyMatrix
    }

let sumHslColorsParallel nameImg chunksImage  =
    chunksImage
    |> Seq.map (sumHslColorsAsync nameImg)  
    |> Async.Parallel   
    |> Async.RunSynchronously 

let sumSqrHSLColorsAsync (nameImg:string) expectValOfImg (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let hslColorsOfImg = hslColorsParallel bmp (x,y)
        return   Array.fold(fun acc e1 ->
                             acc + ((e1-expectValOfImg)
                                    |> Matrix.map(fun e -> Math.Pow(e,2.0)))) emptyMatrix hslColorsOfImg 
    }

let sumSqrHSLColorsParallel nameImg expectValOfImg chunksImage  =
    chunksImage
    |> Seq.map (sumSqrHSLColorsAsync nameImg expectValOfImg)  
    |> Async.Parallel   
    |> Async.RunSynchronously 

let hsl2ColorsParallel (bmp : Bitmap) (x,y) contrast resE1 resDis1 resE2 resDis2 = 
    using (new LockContext(bmp))
        (fun lockContext ->
            [|for y' in fst y..snd y do
                for x' in fst x..snd x -> 
                    let newColor = hsl2rgb (lockContext.GetPixel(x',y')) contrast resE1 resDis1 resE2 resDis2
                    x',y',newColor|])

let hsl2ColorsAsync (nameImg:string)  contrast resE1 resDis1 resE2 resDis2 (x,y)  = 
    async {
        let  bmp = new Bitmap(nameImg)
        let res = hsl2ColorsParallel bmp (x,y) contrast resE1 resDis1 resE2 resDis2
        return res
    }

let resHslColorsParallel nameImg contrast resE1 resDis1 resE2 resDis2 chunksImage  =
    chunksImage
    |> Seq.map (hsl2ColorsAsync nameImg contrast resE1 resDis1 resE2 resDis2)  
    |> Async.Parallel   
    |> Async.RunSynchronously


let rgb2hslParallel (bmp1 : BMP) (bmp2 : BMP) (contrast:bool) = 
    
    let res1 = sumHslColorsParallel bmp1.Path (chunksOfImage bmp1.Img)
    let resPllExpect1 = getExpectVal bmp1.Img ((res1) |>Array.fold(+) emptyMatrix)
    let res'1 =  sumSqrHSLColorsParallel bmp1.Path resPllExpect1 (chunksOfImage bmp1.Img)
    let resPllDisp1 = getDis bmp1.Img ((res'1) |>Array.fold(+) emptyMatrix)


    let res2 = sumHslColorsParallel bmp2.Path (chunksOfImage bmp2.Img)
    let resPllExpect2 = getExpectVal bmp2.Img ((res2) |>Array.fold(+) emptyMatrix)
    let res'2 =  sumSqrHSLColorsParallel bmp2.Path resPllExpect2 (chunksOfImage bmp2.Img)
    let resPllDisp2 = getDis bmp2.Img ((res'2) |>Array.fold(+) emptyMatrix)
    
    let resHsl2Color = 
        resHslColorsParallel bmp2.Path contrast resPllExpect1 resPllDisp1 resPllExpect2 resPllDisp2 (chunksOfImage bmp2.Img)
    let resBmp = new Bitmap(bmp2.Img.Width, bmp2.Img.Height,PixelFormat.Format24bppRgb)
    let resData = Array.fold(fun acc e -> Array.concat[acc;e] ) [||] resHsl2Color
    using (new LockContext(resBmp)) 
        (fun lockContext ->
            resData|> Seq.iter (fun (x,y,c) -> 
                            lockContext.SetPixel(x, y, Color.FromArgb((int)c.[0,0],(int)c.[1,0],(int)c.[2,0]))))
    resBmp
