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
    /// Interaction logic for graphs.xaml
    /// </summary>
    public partial class graphs : UserControl
    {

        internal ViewModels.GraphsViewModel GraphsViewModel;

        public graphs()
        {
            InitializeComponent();
        }

        void setModel(myFGModel model)
        {
            this.GraphsViewModel.setModel(model);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GraphsViewModel.VM_DisplayedFeature = ListBox_Features_List.SelectedItem.ToString();
        }

        private void AnomaliesTSLIST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_AnomaliesTS_List.SelectedItem!= null)
            {
            string indxStr = ListBox_AnomaliesTS_List.SelectedItem.ToString();
            int idx = Int32.Parse(indxStr);
            GraphsViewModel.jumpToAnomaly(idx);
            }
        }
    }
}
