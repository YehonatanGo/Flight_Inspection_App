using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class AlgorithmViewModel : IViewModel
    {
        private IClientModel model;

        public AlgorithmViewModel()
        {
            this.model = null;
        }

        public void setModel(IClientModel m)
        {
            this.model = m;
        }

    }

}
