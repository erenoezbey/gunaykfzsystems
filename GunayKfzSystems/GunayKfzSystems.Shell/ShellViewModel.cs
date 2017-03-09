using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace GunayKfzSystems.Shell
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel: BindableBase
    {

    }
}
