namespace FSharp.Plotly

open System


/// Line type inherits from dynamic object
type Line () =
    inherit DynamicObj ()


[<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
module Line =

    let init (applyStyle:Line->Line) = 
        Line () |> applyStyle 


    type LineStyle () =

        // Applies the styles to Line()
        static member Apply
            (
                ?Width,
                ?Color,
                ?Shape:StyleParam.Shape,
                ?Dash,
                ?Smoothing,
                ?ColorScale:StyleParam.ColorScale
            ) =
                (fun (line:Line) -> 
                    Color      |> DynObj.setValueOpt line "color"
                    Width      |> DynObj.setValueOpt line "width"
                    Shape      |> DynObj.setValueOptBy line "shape" StyleParam.Shape.convert
                    Smoothing  |> DynObj.setValueOpt line "smoothing"
                    Dash       |> DynObj.setValueOptBy line "dash" StyleParam.DrawingStyle.toString
                    ColorScale |> DynObj.setValueOptBy line "colorscale"StyleParam.ColorScale.convert
                    
                    // out -> 
                    line
                )

    