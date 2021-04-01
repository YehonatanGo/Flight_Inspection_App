using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App
{
    interface IClientModel : INotifyPropertyChanged
    {
        string Path { get; set; }
        bool Play { get; set; }
        double PlaySpeed { get; set; }
        int RunningLine { get; set; }





        // open FG and open TCP connection 
        public void connectFlightGear(string ip, int port);

        // create a thread, start sending data in a loop
        public void start();

        // send a line
        public void sendLine();

        public void loadFileToMap();

        public void disconnect();



        public void connectFlightGear();
        public void NotifyPropertyChanged(string propName);
    }


}
