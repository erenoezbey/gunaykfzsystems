using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.Constants;
using GunayKfzSystems.Kunden.View;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace GunayKfzSystems.Kunden
{


    [Module(ModuleName = "GunayKfzSystems.Kunden.KundenModule")]
    [ModuleExport(typeof(KundenModule))]
    public class KundenModule : IModule
    {

        private IRegionManager regionManager;

        [ImportingConstructor]
        public KundenModule(IRegionManager regionManager)
        {
            // ToDo: Get your IEventAggregator, IRegionManager, IUnityContainer here
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.Kunden, typeof(KundenView));
        }
    }
}
