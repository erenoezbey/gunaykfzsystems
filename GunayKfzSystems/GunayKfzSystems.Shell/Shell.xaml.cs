using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using MahApps.Metro.Controls;

namespace GunayKfzSystems.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(Shell))]
    public partial class Shell : MetroWindow
    {

        public Shell()
        {
            this.InitializeComponent();
            this.Closing += this.Shell_Closing;

        }

        private void Shell_Closing(object sender, CancelEventArgs e)
        {
            // Faster Shutdown
            var p = Process.GetCurrentProcess();
            p.Kill();
        }


        [Import]
        private ShellViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
