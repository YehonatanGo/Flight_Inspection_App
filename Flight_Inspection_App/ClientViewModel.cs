using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Flight_Inspection_App
{
    class ClientViewModel : INotifyPropertyChanged
    {
        /*public string path = @"C:\Users\Lenovo\source\repos\testdll\testdll\bin\Release\netcoreapp3.1\testdll.dll";*/

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


        // playeing controller
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


        
        // dashboard controller
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



        // graphs controller
        public List<string> VM_FeaturesList { get { return model.FeaturesList; } }

        public List<DataPoint> VM_Data_Points { get { return model.DataPoints; } }

        public string VM_DisplayedFeature { set { model.DisplayedFeature = value; } get { return model.DisplayedFeature; } }

        public string VM_CorrelatedFeature { set { model.CorrelatedFeature = value; } get { return model.CorrelatedFeature; } }

        public List<DataPoint> VM_CorrelatedDataPoints { get { return model.CorrelatedDataPoints; } }

        public float VM_LineRegA { get { return model.LineRegA; } }
        public float VM_LineRegB { get { return model.LineRegB; } }

        public List<DataPoint> VM_CFPoints { get { return model.CFPoints; } }

        public List<DataPoint> VM_LastPoints { get { return model.LastPoints; } }

        public Plot VM_AnomaliesPlot { get => model.AnomaliesPlot; set { model.AnomaliesPlot = value; } }

        public List<DataPoint> VM_AnomaliesPoints { get => model.AnomaliesPoints; set { model.AnomaliesPoints = value; } }



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
