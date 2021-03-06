using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App
{
    interface IFGModel : INotifyPropertyChanged
    {
        string TestPath { get; set; }
        string TrainPath { get; set; }

        string DllPath { get; set; }

        // playing controller
        bool Play { get; set; }
        double PlaySpeed { get; set; }
        int RunningLine { get; set; }
        int NumOfLines { get; set; }

        // dashborad controller
        double Elevator { get; set; }
        double Aileron { get; set; }
        double Rudder { get; set; }
        double Throttle { get; set; }
        double Airspeed { get; set; }
        double Heading { get; set; }
        double Altitude_hundreds { get; set; }
        double Altitude_thousands { get; set; }
        double Altitude_dozens { get; set; }
        double Yaw { get; set; }
        double Roll { get; set; }
        double Pitch { get; set; }

        // graphs controller
        List<string> FeaturesList { get; set; }
        List<DataPoint> DataPoints { get; set; }
        string DisplayedFeature { get; set; }   

        string CorrelatedFeature { get; set; }
        List<DataPoint> CorrelatedDataPoints { get; set; }

        float LineRegA { get;}
        float LineRegB { get;}
        List<DataPoint> CFPoints { get; set; }
        List<DataPoint> LastPoints { get; set; }

        public Plot AnomaliesPlot { get; set; }
        List<DataPoint> AnomaliesPoints { get; set; }
        List<int> AnomaliesTSList { get; set; }
        string Time { get; set; }


        // read the csv file into CsvHanlder, open FG and open TCP connection 
        public void connectFlightGear();

        // create a thread, start sending data in a loop
        public void start();

        // send a line
        public void sendLines();

        // stop button - pause and go back to the begiging
        public void stop();

        public void disconnect();

        public void NotifyPropertyChanged(string propName);
    }

}
