using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OxyPlot;
using OxyPlot.Wpf;

namespace Flight_Inspection_App.ViewModels
{
    class PlaybarViewModel : IViewModel
    {
        private IFGModel model;

        public PlaybarViewModel(ImyFGModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        
        public void stop()
        {
            model.stop();
        }

        public bool VM_play
        {
            get
            {
                return model.Play;
            }
            set
            {
                model.Play = value;
            }
        }

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
