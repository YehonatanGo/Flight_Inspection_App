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



        public void connect(string ip, int port);
        public void start();

        public void loadFileToMap();

        public void disconnect();



        public void connectFlightGear();
        public void NotifyPropertyChanged(string propName);
    }


}
