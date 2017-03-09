using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.Constants;
using GunayKfzSystems.Fahrzeug.View;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace GunayKfzSystems.Fahrzeug
{


    [Module(ModuleName = "GunayKfzSystems.Fahrzeug.FahrzeugModule")]
    [ModuleExport(typeof(FahrzeugModule))]
    public class FahrzeugModule : IModule
    {

        private IRegionManager regionManager;

        [ImportingConstructor]
        public FahrzeugModule(IRegionManager regionManager)
        {
            // ToDo: Get your IEventAggregator, IRegionManager, IUnityContainer here
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.Fahrzeuge, typeof(FahrzeugView));
        }
    }
}
