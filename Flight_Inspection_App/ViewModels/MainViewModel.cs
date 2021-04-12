using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class MainViewModel
    {
        private IClientModel model;
        private IViewModel DashboardViewModel;
        private IViewModel GraphsViewModel;
        private IViewModel PlaybarViewModel;
        private IViewModel AlgorithmViewModel;
        private IViewModel FilesViewModel;

        public MainViewModel(IClientModel model)
        {
            this.model = model;
        }
    }
}
