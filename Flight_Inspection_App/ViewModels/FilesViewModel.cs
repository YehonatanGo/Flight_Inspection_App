using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App.ViewModels
{
    class FilesViewModel : IViewModel
    {
        private IClientModel model = null;

        public FilesViewModel()
        {
        }

        public void setModel(IClientModel m)
        {
            this.model = m;
        }
    }
}
