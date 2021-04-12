using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Wpf;
using System.ComponentModel;


namespace Flight_Inspection_App.ViewModels
{
    public abstract class IViewModel : INotifyPropertyChanged
    {
        private IClientModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        // get model and set it
        public void setModel(myClientModel m)
        {
            this.model = m;
        }

    }
}
