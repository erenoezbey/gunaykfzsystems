using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.Constants;
using GunayKfzSystems.Wartung.View;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace GunayKfzSystems.Wartung
{


    [Module(ModuleName = "GunayKfzSystems.Wartung.WartungModule")]
    [ModuleExport(typeof(WartungModule))]
    public class WartungModule : IModule
    {

        private IRegionManager regionManager;

        [ImportingConstructor]
        public WartungModule(IRegionManager regionManager)
        {
            // ToDo: Get your IEventAggregator, IRegionManager, IUnityContainer here
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.Wartung, typeof(WartungView));
        }
    }
}
