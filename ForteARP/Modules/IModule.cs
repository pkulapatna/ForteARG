using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ForteARP.Modules
{
    public interface IModule
    {
        int Index { get; set; }
        string MName { get; set; }
        UserControl UserInterface { get; set; }
        bool BActive { get; set; }
    }


}

    