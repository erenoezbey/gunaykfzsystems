using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.Constants;
using GunayKfzSystems.Services.View;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace GunayKfzSystems.Services
{

    [Module(ModuleName = "GunayKfzSystems.Services.ServicesModule")]
    [ModuleExport(typeof(ServicesModule))]
    public class ServicesModule : IModule
    {

        private IRegionManager regionManager;

        [ImportingConstructor]
        public ServicesModule(IRegionManager regionManager)
        {
            // ToDo: Get your IEventAggregator, IRegionManager, IUnityContainer here
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.Services, typeof(ServicesView));
        }
    }
}
