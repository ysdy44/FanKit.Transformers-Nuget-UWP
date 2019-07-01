# FanKit.Transformers

 Drag and drop nodes and manipulate transformer libraries. 

![](https://github.com/ysdy44/FanKit.Transformers-Nuget-UWP/blob/master/ScreenShot/ScreenShot001.jpg)


## Getting Started

|Key|Value|
|:-|:-|
|System requirements| Windows10 Creators Update or upper|
|Development tool|Visual Studio 2017|
|Programing language|C#|
|Display language|English|

  ![](https://github.com/ysdy44/FanKit.Transformers-Nuget-UWP/blob/master/ScreenShot/logo.png)


Search 'FanKit.Transformers
' in Nuget and download it.
  ![](https://github.com/ysdy44/FanKit.Transformers-Nuget-UWP/blob/master/ScreenShot/Thumbnails000.jpg)


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

You can learn more from the demo application:
https://www.microsoft.com/store/productId/9PD2JJZQF524


1.Click on item "Transformers" in the top bar.

![](https://github.com/ysdy44/FanKit.Transformers-Nuget-UWP/blob/master/ScreenShot/Thumbnails001.jpg)

2.Look for simple examples.
![](https://github.com/ysdy44/FanKit.Transformers-Nuget-UWP/blob/master/ScreenShot/Thumbnails002.jpg)


Enjoy it..
