(**
# Sunburst charts

[![Binder](https://plotly.net/img/badge-binder.svg)](https://mybinder.org/v2/gh/plotly/Plotly.NET/gh-pages?filepath=09_5_sunburst.ipynb)&emsp;
[![Script](https://plotly.net/img/badge-script.svg)](https://plotly.net/09_5_sunburst.fsx)&emsp;
[![Notebook](https://plotly.net/img/badge-notebook.svg)](https://plotly.net/09_5_sunburst.ipynb)

**Summary:** This example shows how to create sunburst charts in F#.

Sunburst Chart � also known as Ring Chart, Multi-level Pie Chart, and Radial Treemap � is typically used to visualize hierarchical data structures.
A Sunburst Chart consists of an inner circle surrounded by rings of deeper hierarchy levels.
The angle of each segment is either proportional to a value or divided equally under its parent node.

## Simple sunburst plot

*)
open Plotly.NET 

let values = [19; 26; 55;]
let labels = ["Residential"; "Non-Residential"; "Utility"]

let sunburstChart =
    Chart.Sunburst(
        ["A";"B";"C";"D";"E"],
        ["";"";"B";"B";""],
        Values=[5.;0.;3.;2.;3.],
        MultiText=["At";"Bt";"Ct";"Dt";"Et"]
    )
(**
## More styled example

This example shows the usage of some of the styling possibility using `Chart.Sunburst`.
For even more styling control, use the respective TraceStyle function `TraceDomainStyle.Sunburst`

*)
let sunburstStyled = 
    let labelsParents = [
        ("A",""), 20
        ("B",""), 1
        ("C",""), 2
        ("D",""), 3
        ("E",""), 4

        ("AA","A"), 15
        ("AB","A"), 5

        ("BA","B"), 1

        ("AAA", "AA"), 10
        ("AAB", "AA"), 5
    ]

    Chart.Sunburst(
        labelsParents |> Seq.map fst,
        Values = (labelsParents |> Seq.map snd), 
        BranchValues = StyleParam.BranchValues.Total, // branch values are the total of their childrens values
        SectionColorScale = StyleParam.Colorscale.Viridis,
        ShowSectionColorScale = true,
        SectionOutlineColor = Color.fromKeyword Black,
        Rotation = 45,
        UseDefaults = false
    )(* output: 
<div id="020e0e5c-14e3-4cf4-bf09-95c446c000e2"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_020e0e5c14e34cf4bf0995c446c000e2 = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.6.3.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"sunburst","parents":["","","","","","A","A","B","AA","AA"],"values":[20,1,2,3,4,15,5,1,10,5],"labels":["A","B","C","D","E","AA","AB","BA","AAA","AAB"],"marker":{"colorscale":"Viridis","line":{"color":"rgba(0, 0, 0, 1.0)"},"showscale":true},"branchvalues":"total","rotation":45}];
            var layout = {};
            var config = {};
            Plotly.newPlot('020e0e5c-14e3-4cf4-bf09-95c446c000e2', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_020e0e5c14e34cf4bf0995c446c000e2();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_020e0e5c14e34cf4bf0995c446c000e2();
            }
</script>
*)

