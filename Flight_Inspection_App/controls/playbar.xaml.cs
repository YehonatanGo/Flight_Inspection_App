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
    /// Interaction logic for playbar.xaml
    /// </summary>
    public partial class playbar : UserControl
    {
        internal ViewModels.PlaybarViewModel PlaybarViewModel;

        public playbar()
        {
            InitializeComponent();
        }

        void setModel(myClientModel model)
        {
            this.PlaybarViewModel.setModel(model);
        }

        private void Button_Play_Click(object sender, RoutedEventArgs e)
        {
            Slider_Time.Maximum = PlaybarViewModel.VM_NumOfLines;
            PlaybarViewModel.VM_play = true;
        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            PlaybarViewModel.VM_play = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PlaybarViewModel != null)
            {
                PlaybarViewModel.VM_Play_Speed = Slider_Speed.Value;
            }
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {

            PlaybarViewModel.stop();
        }

        private void Button_Forward_Click(object sender, RoutedEventArgs e)
        {
            PlaybarViewModel.VM_Running_Line += 150;
        }

        private void Button_Backward_Click(object sender, RoutedEventArgs e)
        {
            PlaybarViewModel.VM_Running_Line -= 150;
        }

        


    }
}
