using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    public class DashboardViewModel : IViewModel
    {
        private IFGModel model;

        public DashboardViewModel(myFGModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public double VM_Elevator
        {
            get
            {
                return model.Elevator;
            }
        }

        public double VM_Aileron
        {
            get
            {
                return model.Aileron;
            }
        }

        public double VM_Rudder
        {
            get
            {
                return model.Rudder;
            }
        }

        public double VM_Throttle
        {
            get
            {
                return model.Throttle;
            }
        }

        public double VM_Airspeed
        {
            get
            {
                return model.Airspeed;
            }
        }

        public double VM_Heading
        {
            get
            {
                return model.Heading;
            }
        }

        public double VM_Altitude_hundreds
        {
            get
            {
                return model.Altitude_hundreds;
            }
        }

        public double VM_Altitude_thousands
        {
            get
            {
                return model.Altitude_thousands;
            }
        }

        public double VM_Altitude_dozens
        {
            get
            {
                return model.Altitude_dozens;
            }
        }

        public double VM_Yaw
        {
            get
            {
                return model.Yaw;
            }
        }

        public double VM_Roll
        {
            get
            {
                return model.Roll;
            }
        }

        public double VM_Pitch
        {
            get
            {
                return model.Pitch;
            }
        }


    }
}
