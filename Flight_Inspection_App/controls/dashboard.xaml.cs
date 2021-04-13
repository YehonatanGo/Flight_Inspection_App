using Flight_Inspection_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flight_Inspection_App.controls

{
    /// <summary>
    /// Interaction logic for dashboard.xaml
    /// </summary>
    public partial class dashboard : UserControl
    {
        internal ViewModels.DashboardViewModel DashboardViewModel;

        public dashboard()
        {
            InitializeComponent();
        }

        void setModel(myFGModel model)
        {
            this.DashboardViewModel.setModel(model);
        }

        private void compass_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
