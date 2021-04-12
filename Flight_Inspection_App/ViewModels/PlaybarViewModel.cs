using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class PlaybarViewModel : IViewModel
    {
        private IClientModel model;

        public PlaybarViewModel()
        {
            this.model = null;
        }

        public void setModel(IClientModel m)
        {
            this.model = m;
        }

    }
}
