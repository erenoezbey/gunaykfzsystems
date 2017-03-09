using System.Windows;
using log4net.Config;

namespace GunayKfzSystems.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            // Configure Log4Net
            XmlConfigurator.Configure();

            base.OnStartup(e);

            AppBootstrapper bs = new AppBootstrapper();
            bs.Run();

        }


    }
}
