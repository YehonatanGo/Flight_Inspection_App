using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    public class DashboardViewModel : IViewModel
    {
        private IClientModel model;

        public DashboardViewModel()
        {
            this.model = null;
        }

        public DashboardViewModel(myClientModel model)
        {
            this.model = model;
        }
    }
}
