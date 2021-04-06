using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
            Slider_Time.Maximum = vm.VM_NumOfLines;
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
            vm.VM_Play_Speed = Slider_Speed.Value;
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            vm.stop();
        }

        private void Button_Forward_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Running_Line += 150;
        }

        private void Button_Backward_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Running_Line -= 150;//
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.VM_DisplayedFeature = ListBox_Features_List.SelectedItem.ToString();
        }
    }
}
