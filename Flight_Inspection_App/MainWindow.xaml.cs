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
        ClientViewModel vm;
        public MainWindow()
        {

            InitializeComponent();
            vm = new ClientViewModel(new myClientModel());
            DataContext = vm;
            path = "";
        }

        private void Button_OpenFlightDataFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            /*openFileDialog.Filter = "XML files (*.xml)|*.xml";*/
            /*openFileDialog.Filter = "CSV files (*.csv)|*.csv";*/
            if (openFileDialog.ShowDialog() == true)
            {
                vm.VM_Path = openFileDialog.FileName;
            }
        }

        private void Button_Play_Flight(object sender, RoutedEventArgs e)
        {
            vm.start();
            /*IClientModel c = new myClientModel();
            *//*c.Path = path;*//*
            c.connectFlightGear();*/


        }
    }
}
