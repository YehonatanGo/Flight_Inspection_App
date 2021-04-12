using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class GraphsViewModel : IViewModel
    {
        private IClientModel model = null;

        public GraphsViewModel(myClientModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public List<string> VM_FeaturesList { get { return model.FeaturesList; } }

        public List<int> VM_AnomaliesTSList { get => model.AnomaliesTSList; }

        public List<DataPoint> VM_Data_Points { get { return model.DataPoints; } }

        public string VM_DisplayedFeature { set { model.DisplayedFeature = value; } get { return model.DisplayedFeature; } }

        public string VM_CorrelatedFeature { set { model.CorrelatedFeature = value; } get { return model.CorrelatedFeature; } }

        public List<DataPoint> VM_CorrelatedDataPoints { get { return model.CorrelatedDataPoints; } }

        public float VM_LineRegA { get { return model.LineRegA; } }
        public float VM_LineRegB { get { return model.LineRegB; } }

        public List<DataPoint> VM_CFPoints { get { return model.CFPoints; } }

        public void jumpToAnomaly(int idx)
        {

            model.RunningLine = idx;
        }

        public List<DataPoint> VM_LastPoints { get { return model.LastPoints; } }

        public Plot VM_AnomaliesPlot { get => model.AnomaliesPlot; set { model.AnomaliesPlot = value; } }

        public List<DataPoint> VM_AnomaliesPoints { get => model.AnomaliesPoints; set { model.AnomaliesPoints = value; } }

    }
}
