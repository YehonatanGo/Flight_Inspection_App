# Flight_Inspection_App
### General Description
Desktop app for analyzing flights. The app connects to FlightGear simulator (download at https://www.flightgear.org), shows graphically all the data of the flight, and in accordance to the chosen anomaly detectoion algorithm shows the anomalies found during the flight. 

### Collaborators
This program was developed by Yehonatan Goldfarb, Itamar Fisch and Neriya Fisch, CS students from Bar-Ilan university, Israel.

### Prerequisites
* install flightgear, and make sure your dierctories hierarachy is `C:\Program Files\FlightGear`
* move "playback_small.xml" to `C:\Program Files\FlightGear\data\Protocol`
* if you are interested in adding a new anomaly detection algorithm, please read under "Instructions" for more information. 

### Code Design and Architechture:
The app has been programmed by the MVVM architecture and WPF data binding mechanism. As well, the app supports loading dll files dynamically at runtime.

### Instructions
When opening the app, the client needs to load a train and current csv flights file (with features at the first line), choose algorithm of anomaly detection from the "plugin" folder and then press on "open FlightGear" button.
If the client wants to use his own anomaly detection algorithm, he must works as follows:
* puts the C# dll file under "plugins" file.
* The namespace of the DLL will be called "Anomaly_Detecton_Algorithm".
* The class of will be called "AnomalyDetector".


```C#
public void learnAndDetect(string path){}
public Annotation GetAnnotation(string cfKey){}
public List<Point> getAnomalies(string cfKey){}
```

* The function that learns and detect all the data will be called "learnAndDetect" and it recieves the path of the train and current csv flights files. return value is void.
* The function that in charge of the current shape to be drawn will be called "GetAnnotation" and it recieves a string of the current correlated features that chosen. return value is Annotation type.
* The function that in charge of the anomaly points to be drawn will be called "getAnomalies" and it recieves a string of the current correlated features that chosen. return value is a list of points.

### Further Documentation
See UML diagrams under "UMLS" directory.

### Video Demo
Link to the video: https://youtu.be/C6J0noC6K6k
