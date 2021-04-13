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
        ImyFGModel model = new ImyFGModel();

        public MainWindow()
        {
            InitializeComponent();

            // set model to other VMs
            ViewModels.DashboardViewModel dashboardVM = new ViewModels.DashboardViewModel(model);
            DashboardView.DashboardViewModel = dashboardVM;
            DashboardView.DataContext = dashboardVM;

            ViewModels.GraphsViewModel graphVM = new ViewModels.GraphsViewModel(model);
            GraphsView.DataContext = graphVM;
            GraphsView.GraphsViewModel = graphVM;
            GraphsView.GraphsViewModel.VM_AnomaliesPlot = GraphsView.Linear_Regression;

            ViewModels.PlaybarViewModel playbarVM = new ViewModels.PlaybarViewModel(model);
            PlaybarView.DataContext = playbarVM;
            PlaybarView.PlaybarViewModel = playbarVM;

            ViewModels.FilesViewModel filesViewModel = new ViewModels.FilesViewModel(model);
            LoadFilesView.DataContext = filesViewModel;
            LoadFilesView.FilesViewModel = filesViewModel;
        }
        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GraphsView.GraphsViewModel.VM_DisplayedFeature = GraphsView.ListBox_Features_List.SelectedItem.ToString();
        }
    }
}
