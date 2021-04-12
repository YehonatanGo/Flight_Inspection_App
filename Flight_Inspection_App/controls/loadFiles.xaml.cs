using Microsoft.Win32;
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
    /// Interaction logic for loadFiles.xaml
    /// </summary>
    public partial class loadFiles : UserControl
    {
        internal ViewModels.FilesViewModel FilesViewModel;

        public loadFiles()
        {
            InitializeComponent();
        }

        private void Button_LoadFlightDataFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                FilesViewModel.VM_TestPath = openFileDialog.FileName;
                Button_ShowFlight.IsEnabled = true;
            }
        }

        private void Button_load_test_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                FilesViewModel.VM_TrainPath = openFileDialog.FileName;
                Button_ShowFlight.IsEnabled = true;
            }
        }

        private void Button_LoadAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll";
            if (openFileDialog.ShowDialog() == true)
            {
                FilesViewModel.VM_DllPath = openFileDialog.FileName;
                Button_ShowFlight.IsEnabled = true;
            }
        }

        private void Button_Play_Flight(object sender, RoutedEventArgs e)
        {
            Button_ShowFlight.IsEnabled = false;
            Button_OpenDataFile.IsEnabled = false;
            FilesViewModel.connectFlightGear();

            //Slider_Time.Maximum = vm.VM_NumOfLines;
            
        }
    }
}
