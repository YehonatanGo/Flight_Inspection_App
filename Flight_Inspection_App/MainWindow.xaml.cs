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

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            /*openFileDialog.Filter = "XML files (*.xml)|*.xml";*/
            /*openFileDialog.Filter = "CSV files (*.csv)|*.csv";*/
            if (openFileDialog.ShowDialog() == true)
            {
                textbox.Text = openFileDialog.FileName;
                vm.VM_Path = textbox.Text;
            }
        }

        private void Button_Play(object sender, RoutedEventArgs e)
        {
            vm.start();
            /*IClientModel c = new myClientModel();
            *//*c.Path = path;*//*
            c.connectFlightGear();*/


        }
    }
}
