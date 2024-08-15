using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteARP.ViewModels
{
    internal class LoadAnimationViewModel : BindableBase
    {

        private string _strtext;
        public string Strtext
        {
            get { return _strtext; }
            set { SetProperty(ref _strtext, value); }
        }

        public LoadAnimationViewModel()
        {



        }
    }
}
