# Flight_Inspection_App
### General Description
Desktop app for analyzing flights. The app connects to FlightGear simulator (download at https://www.flightgear.org), shows graphically all the data of the flight, and in accordance to the chosen anomaly detectoion algorithm shows the anomalies found during the flight. 

### Collaborators
This program was developed by Yehonatan Goldfarb, Itamar Fisch and Neriya Fisch, CS students from Bar-Ilan university, Israel.

### Prerequisites
* Install flightgear, and make sure your dierctories hierarachy is `C:\Program Files\FlightGear`
* Move `playback_small.xml` to `C:\Program Files\FlightGear\data\Protocol`
* Wev'e been using the following packages:
  * Microsoft.Toolkit.Uwp.UI.Controls version 7.0.1
  * OxyPlot.Wpf version 2.0.0 
  *  Syncfusion.SfGauge.WPF version 19.1.0.54
  When opening the project on Visual Studio they should be installed automaticall. Download manually if they don't.

* If you are interested in adding a new anomaly detection algorithm, please read under "Instructions" for more information. 

### Code Design and Architechture:
The app has been programmed by the MVVM architecture and WPF data binding mechanism. As well, the app supports loading dll files dynamically at runtime.

### Instructions
When opening the app, the client needs to load a train and current csv flights file (with features at the first line), choose algorithm of anomaly detection from the "plugin" folder and then press on "open FlightGear" button.

If the client wants to use his own anomaly detection algorithm, he must works as follows:
* Put the dll file under "plugins" file.
* The namespace of the DLL will be called "Anomaly_Detecton_Algorithm".
* The class of will be called "AnomalyDetector".
* Must follow the following signatures:


```C#
public void learnAndDetect(string trainPath, string testPath){}
public Annotation GetAnnotation(string cfKey){}
public List<Point> getAnomalies(string cfKey){}
```
* learnAndDetect - gets paths of train and test csv files, learns the normal model, and detects anomalies in the test file.
* GetAnnotation - gets a string description of correlated features returns ("a+b"). Returns Annotation (oxyplot interface) describing the normal model (linear regression, minimal circle etc.).
* getAnomalies - gets a string description of correlated features returns ("a+b"). Returns List of Points (you can use our Point class or equivalent) that was found anomalous.

### Further Documentation
See UML diagrams under "UMLS" directory.

### Video Demo
Link to the video: https://youtu.be/C6J0noC6K6k
