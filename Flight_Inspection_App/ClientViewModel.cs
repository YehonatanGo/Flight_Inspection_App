using System;

using System.ComponentModel;

namespace Flight_Inspection_App
{
    class ClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IClientModel model;
        public ClientViewModel(IClientModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string vm_path;
        public string VM_Path
        {
            get
            {
                return model.Path;
            }

            set
            {
                model.Path = value;
            }
        }

        private bool play;
        public bool VM_play {
            get
            {
                return play;
            }
            set
            {
                model.Play = value;
            }
        }

        private double VM_playSpeed;
        public double VM_Play_Speed
        {
            get
            {
                return model.PlaySpeed;
            }
            set
            {
                model.PlaySpeed = value;
            }
        }

        private int VM_runningLine;
        public int VM_Running_Line
        {
            get
            {
                return model.RunningLine;
            }
            set
            {
                model.RunningLine = value;
            }
        }

        private int VM_numOfLines;
        public int VM_NumOfLines
        {
            get
            {
                return model.NumOfLines;
            }
            set
            {
                model.NumOfLines = value;
            }
        }

        public void connectFlightGear()
        {
            model.connectFlightGear();
        }

        public void stop()
        {
            model.stop();
        }

    }
}
