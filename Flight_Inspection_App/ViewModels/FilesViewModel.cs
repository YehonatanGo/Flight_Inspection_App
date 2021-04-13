using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class FilesViewModel : IViewModel
    {
        private IFGModel model = null;

        public FilesViewModel(myFGModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void connectFlightGear()
        {
            model.connectFlightGear();
        }

        public string VM_TestPath
        {
            get
            {
                return model.TestPath;
            }

            set
            {
                model.TestPath = value;
            }
        }

        public string VM_TrainPath
        {
            get { return model.TrainPath; }
            set { model.TrainPath = value; }
        }

        public string VM_DllPath { get => model.DllPath; set { model.DllPath = value; } }

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
    }
}
