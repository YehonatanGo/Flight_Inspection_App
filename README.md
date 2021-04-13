# Flight_Inspection_App
### General Description
Desktop app for analyzing flights. the app connects to FlightGear simulator(https://www.flightgear.org/), shows graphically all the data of the flight and in accordiance to the chosen detector algorithm it's shows the anomalies of the flight. 

### Collaborators
This program was developed by Yehonatan Goldfarb, Itamar Fisch and Neriya Fisch, CS students from Bar-Ilan university, Israel.

### Prerequisites
* install flightgear 20.3.6, and make sure your dierctories hierarachy is "C:\Program Files\FlightGear"
* move "playback_small.xml" to "C:\Program Files\FlightGear\data\Protocol"
* if you are interested in new anomaly detector algorithm, please read under ### Instructions for more information. 

### Code Design:
The app has been programmed by the MVVM design and C# data binding mechanism and. as well, the app supports loading dll files at runtime.

### Instructions
When opening the app, the client needs to load a train and current csv flights file (with features at the first line), choose algorithm of anomaly detection from the "plugin" folder and then press on "open FlightGear" button.
If the client wants to use his own anomaly detection algorithm, he must works as follows:
* puts the C# dll file under "plugins" file.
* The namespace of the DLL will be called "Anomaly_Detecton_Algorithm".
* The class of will be called "AnomalyDetector".
* The function that learns and detect all the data will be called "learnAndDetect" and it recieves the path of the train and current csv flights files. return value is void.
* The function that in charge of the current shape to be drawn will be called "GetAnnotation" and it recieves a string of the current correlated features that chosen. return value is Annotation type.
* The function that in charge of the anomaly points to be drawn will be called "getAnomalies" and it recieves a string of the current correlated features that chosen. return value is a list of points.

### Further Documentation

### Video Demo
