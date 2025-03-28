# FanKit.Transformers

 Drag and drop nodes and manipulate transformer libraries. 

![](ScreenShot/ScreenShot001.jpg)


## Development environment

|Key|Value|
|:-|:-|
|System requirements| Windows10 Creators Update or upper|
|Development tool|Visual Studio 2017|
|Programing language|C#|
|Display language|English|

![](ScreenShot/logo.png)

  
## Nuget

1. Access [Nuget Gallery | FanKit.Transformers](https://www.nuget.org/packages/FanKit.Transformers)
2. Search "**FanKit.Transformers**" in **Nuget Packages Manager** and download it.  
![](ScreenShot/Thumbnails000.jpg)


### Example

Run the "TestApp".

```xaml
    xmlns:transformers="using:FanKit.Transformers"
    xmlns:canvas="using:Microsoft.Graphics.Canvas.UI.Xaml"

    <Page.Resources>
        <transformers:CanvasOperator x:Name="CanvasOperator" DestinationControl="{x:Bind CanvasControl}" Single_Start="CanvasOperator_Single_Start" Single_Delta="CanvasOperator_Single_Delta" Single_Complete="CanvasOperator_Single_Complete"/>
    </Page.Resources>

   <canvas:CanvasControl x:Name="CanvasControl" Draw="CanvasControl_Draw" CreateResources="CanvasControl_CreateResources"/>   
"
```
and

```csharp
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System.Numerics;
...
    //Controller
    Transformer transformer = Transformer.Controller(this.TransformerMode, startingPoint, point, this.Layer.TransformerMatrix.OldDestination, isRatio, isCenter);

    this.Layer.TransformerMatrix.Destination = transformer;
...
```


## Learn More

You can learn more from the demo application in windows 10 store:<br/>
[FanKit](https://apps.microsoft.com/detail/9pd2jjzqf524)


1.Click on item "Transformers" in the top bar.  
![](ScreenShot/Thumbnails001.jpg)

2.Look for simple examples.  
![](ScreenShot/Thumbnails002.jpg)


Enjoy it..
