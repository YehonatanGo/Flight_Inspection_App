using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        public double VM_PLAYSPEED
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
