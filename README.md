# BPMN View
A simple free tool to view and print business process diagrams in a popular BPMN format. 

* Full conformance to the latest version of [BPMN 2.0 specification by OMG](http://www.bpmn.org/).
* Import models from all [major BPM vendors](http://bpmn-miwg.github.io/bpmn-miwg-tools/)
* Strict validation of the model according to BPMN specification.
* Arbitrary scaling and zooming view of most complex diagrams.
* Support of raster and vector image output.
* Batch processing of multiple BPMN files.
* 100% Microsoft .NET managed solution.
* Loyal open source license for private and commercial use.
 
Try it yourself!
* Clone project repository and build on Microsoft Visual Studio.
* Or download pre-built binary package.
* Test it on files from [offcial BPMN test suite](https://github.com/bpmn-miwg/bpmn-miwg-test-suite).
* Build your own open source or commercial solution based on this code.

 
![bzinchenko](Images/BPMN_View.png)

##Code example
Jump start your BPMN capable solution in minutes!

Sample code to read BPMN file and save it as an image:

```csharp
Model model = BPMN.Model.Read("B.2.0.bpmn");
Image img = model.GetImage(0, 2.0f);
img.Save("B.2.0.png", ImageFormat.Png);
```

Below is the result:

![bzinchenko](Images/B.2.0.png)

