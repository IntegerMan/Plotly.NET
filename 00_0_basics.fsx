(**
[![Binder](https://plotly.net/img/badge-binder.svg)](https://mybinder.org/v2/gh/plotly/Plotly.NET/gh-pages?filepath=00_0_basics.ipynb)&emsp;
[![Script](https://plotly.net/img/badge-script.svg)](https://plotly.net/00_0_basics.fsx)&emsp;
[![Notebook](https://plotly.net/img/badge-notebook.svg)](https://plotly.net/00_0_basics.ipynb)

# Plotly.NET basics

**This section is WIP.**

### Table of contents

* Library design(#Library-design)

* [GenericChart](#GenericChart)

* [Working with GenericCharts](#Working-with-GenericCharts)

  * [Dynamic object style](#Dynamic-object-style)
  

## Library design

Plotly.NET is a .NET wrapper for creation of [plotly charts]() written in F#. This means that, under the hood, all functionality creates JSON objects that can be rendered by plotly.

A plotly.js chart consists of 3 objects:

* `data`, which is a collection of `traces` which represent the data and chart type used to visualize the data

* `layout`, which controls the general chart layout such as axis positions and styles

* `config` high level properties of the chart like making all chart elements editable or the tool bar on top

These are mirrored in Plotly.NET's central type, `GenericChart`:

## GenericChart

The central type that gets created by all Chart constructors is `GenericChart`, which itself represents either a single chart or a multi chart (as a Discriminate Union type). It looks like this:

```fsharp
type GenericChart_ =
    | Chart of Trace * Layout * Config * DisplayOptions
    | MultiChart of Trace list * Layout * Config * DisplayOptions

```

As you can see, a `GenericChart` consists of four top level objects - `Trace` (multiple of those in the case of a MultiChart) , `Layout`, `Config`, and `DisplayOptions`.

* `Trace` is in principle the representation of a dataset on a chart, including for example the data itself, color and shape of the visualization, etc.

* `Layout` is everything of the chart that is not dataset specifivc - e.g. the shape and style of axes, the chart title, etc.

* `Config` is an object that configures high level properties of the chart like making all chart elements editable or the tool bar on top

* `DisplayOptions` is an object that contains meta information about how the html document that contains the chart.

### Layers of abstraction

Plotly.NET uses multiple layers of abstractions to generate valid plotly.js JSON objects with different levels of control and complexity:

#### The Chart module

The `Chart` module provides the highest layer of abstraction. Here, plotly.js trace types are broken down to the most common and useful styling options, and combined with common layout settings.
It also provides composite charts which consist of multiple traces such as `Chart.Range`, which really is a combination of 3 scatter traces.

Here is an example on how to create a simple 2D point chart:

*)
let pointChart =
    Chart.Point([1,2; 3,4])(* output: 
No value returned by any evaluator*)
(**
#### The TraceStyle modules

The TraceStyle modules offer access to all parameters supported by plotly.js for the respective trace type. If you want to create a `scatter` trace, you can use the function
`Trace2D.initScatter`, which will initialize an empty trace of type `scatter` and apply a styling function to it. This function would be `Trace2DStyle.Scatter`, which can apply all scatter related parameters to a trace.
In contrast to the `Chart` module, the parameters are named exactly the same as in plotly.js (but in PascalCase).

To create a GenericChart from a `Trace` object, you can use `GenericChart.ofTraceObject`.
Compare how many more styling options you have compared to `Chart.Point` above, but also take a look at how more verbose you have to be.
You can clearly see the advantages and disadvantages of both approaches.

*)
let withTraceStyle =
    Trace2D.initScatter( 
        Trace2DStyle.Scatter(
            X = [1; 3],
            Y = [2; 4]
        )
    )
    |> GenericChart.ofTraceObject true(* output: 
No value returned by any evaluator*)
(**
#### Dynamic object

The prime directive for all functions provided by Plotly.NET is the construction of valid plotly JSON objects.
For this purpose, `Trace`, `Layout`, and `Config` (and many other internal objects) are inheriting from [`DynamicObj`](https://github.com/plotly/Plotly.NET/blob/dev/src/Plotly.NET/DynamicObj.fs),
an extension of `DynamicObject` which makes it possible to set arbitraryly named and typed properties of these objects via the `?` operator.

If you want to exactly mirror a plotly.js tutorial, or want to set properties that for any reason are not abstracted in Plotly.NET,
it can be useful to use the power of DynamicObj to set the parameters directly. Just make sure that the property name is exactly the same as in plotly.js (all lowercase)

So if you want to set any kind of property on one of these objects you can do it in a very declarative way like this:

*)
let myTrace = Trace("scatter") // create a scatter trace
myTrace?x <- [0;3] // set the x property (the x dimension of the data)
myTrace?y <- [2;4] // set the y property (the y dimension of the data)

let withDynObj = GenericChart.ofTraceObject true myTrace(* output: 
No value returned by any evaluator*)
(**
lets have a look at the trace object that will be created. The relevant section of the html generated with Chart.Show is the following:

```javascript
var data = [{"type":"scatter","x":[0,1,2],"y":[0,1,2]}];
```

*)

