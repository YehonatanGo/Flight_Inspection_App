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

### Further Documentation

### Video Demo
