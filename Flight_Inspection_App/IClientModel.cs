using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App
{
    interface IClientModel : INotifyPropertyChanged
    {
        string Path { get; set; }

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

        // graphs controller
        List<DataPoint> DataPoints { get; set; }

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
