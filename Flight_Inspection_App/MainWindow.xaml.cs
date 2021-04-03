using System;
using System.Windows;
using Microsoft.Win32;

namespace Flight_Inspection_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String path;
        private bool isAlreadyPlayed;
        ClientViewModel vm;
        public MainWindow()
        {
            vm = new ClientViewModel(new myClientModel());
            InitializeComponent();
            DataContext = vm;
            path = "";
            isAlreadyPlayed = false;
        }

        private void Button_OpenFlightDataFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                vm.VM_Path = openFileDialog.FileName;
                Button_ShowFlight.IsEnabled = true;
            }
        }

        private void Button_Play_Flight(object sender, RoutedEventArgs e)
        {
            Button_ShowFlight.IsEnabled = false;
            Button_OpenDataFile.IsEnabled = false;
            vm.connectFlightGear();
        }

        private void Button_Play_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_play = true;
        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_play = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vm.VM_PLAYSPEED = Slider_Speed.Value;
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            vm.stop();
        }
    }
}
