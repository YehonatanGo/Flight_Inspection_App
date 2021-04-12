using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class GraphsViewModel : IViewModel
    {
        private IClientModel model = null;

        public GraphsViewModel()
        {
        }

        public void setModel(IClientModel m)
        {
            this.model = m;
        }
    }
}
