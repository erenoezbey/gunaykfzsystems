using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using GunayKfzSystems.Shell.LogHelper;
using Prism.Logging;
using Prism.Mef;
using Prism.Regions;

namespace GunayKfzSystems.Shell
{
    internal class AppBootstrapper : MefBootstrapper
    {
        //TODO: 02. Override the CreateShell returning an instance of your shell.
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }

 


        //TODO: 03. Override the InitializeShell setting the MainWindow on the application and showing the shell.
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        //TODO: 04. Override the ConfigureAggregateCatalog.  Add the new assembly catalogs which will register all
        //          classes with [Export] attributes
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

    //        AggregateCatalog.Catalogs.Add(new DirectoryCatalog(@".\"));

            var directories = new List<string>
            {
                "",
                "Services",
                "MainRegions",
                "Views",
                "Modules"
            };

            var asble = directories.Select(ModuleHandler.GetNotLoadedFiles).SelectMany(files => files);

            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AppBootstrapper).Assembly));

            foreach (var file in asble)
            {
                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(file)));
#if DEBUG
             //   this.Logger.Log(string.Format("Loaded {0}", file), Category.Debug, Priority.None);
#endif 
            }

           
        }

        /// <summary>
        /// Create the <see cref="T:Prism.Logging.ILoggerFacade" /> used by the bootstrapper.
        /// </summary>
        /// <remarks>The base implementation returns a new TextLogger.</remarks>
        protected override ILoggerFacade CreateLogger()
        {
            //var writer = new StreamWriter(@"c:\\Logger\Log.txt");
            //return new TextLogger(writer);

            return new Log4NetLogger();
        }
    }
}
