using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GunayKfzSystems.Fahrzeug.ViewModel;

namespace GunayKfzSystems.Fahrzeug.View
{
    /// <summary>
    /// Interaction logic for FahrzeugView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FahrzeugView : UserControl
    {
        [Import]
        public FahrzeugViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        public FahrzeugView()
        {
            InitializeComponent();
        }
    }

}
