module UtilityMatrixAndType 
open System
open System.Net
open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.Statistics
open Microsoft.FSharp.NativeInterop
open System.Drawing
open System.Drawing.Imaging
#nowarn "9"

type LockContext(bitmap:Bitmap) =
     let pixelSize = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8
     let data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                                ImageLockMode.ReadOnly, 
                                bitmap.PixelFormat)

     let formatNotSupportedMessage = "Pixel format not supported."

     let getPixelAddress = 
        match data.PixelFormat with
        | PixelFormat.Format24bppRgb | PixelFormat.Format32bppArgb -> (fun x y -> NativePtr.add<byte> 
                                                                                    (NativePtr.ofNativeInt data.Scan0) 
                                                                                    ((y * data.Stride) + (x * pixelSize)))
        | _ -> failwith formatNotSupportedMessage
     
     let getPixel x y = 
        let address = getPixelAddress x y
        match data.PixelFormat with
        | PixelFormat.Format24bppRgb | PixelFormat.Format32bppArgb -> Color.FromArgb(NativePtr.get address 2 |> int, 
                                                                                    NativePtr.get address 1 |> int, 
                                                                                    NativePtr.read address |> int)
        | _ -> failwith formatNotSupportedMessage

     let setPixel x y (r,g,b) = 
        let address = getPixelAddress x y
        match data.PixelFormat with
        | PixelFormat.Format24bppRgb | PixelFormat.Format32bppArgb ->   NativePtr.set address 2 r
                                                                        NativePtr.set address 1 g
                                                                        NativePtr.write address b
        | _ -> failwith formatNotSupportedMessage

     member this.SetPixel(x,y,color:Color) = setPixel x y (color.R, color.G, color.B)
     member this.GetPixel(x,y) = getPixel x y

     interface IDisposable with
        member this.Dispose() =
            bitmap.UnlockBits(data)



type Mat = Matrix<float>
let convertToLmsMatrix = 0.92157 * matrix [[0.3811; 0.5783; 0.0402]
                                           [0.1967; 0.7244; 0.0782]
                                           [0.0241; 0.1288; 0.8444]]

let convertLabToRGBMatrix = Math.Pow(0.92157,-1.0) * matrix [[4.4679; -3.5873; 0.1193]
                                                             [-1.2186; 2.3809; -0.1624]
                                                             [0.0497; -0.2439; 1.2045]]
let convertLabMatrix1 = matrix [[0.5774; 0.0; 0.0]
                                [0.0; 0.4082; 0.0]
                                [0.0;0.0; 0.7071]]

let convertLabMatrix2 = matrix [[1.0; 1.0; 1.0]
                                [1.0;1.0; -2.0]
                                [1.0;-1.0; 0.0]]

let emptyMatrix = DenseMatrix.zero<float> 3 1

type BMP =
    struct
        val Img : Bitmap;
        val Path : string;
        new (img, path) = {Img = img; Path = path}
    end