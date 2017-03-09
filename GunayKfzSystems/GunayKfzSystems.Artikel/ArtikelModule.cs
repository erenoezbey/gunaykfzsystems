using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Artikel.View;
using GunayKfzSystems.Core.Constants;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace GunayKfzSystems.Artikel
{
    [Module(ModuleName = "GunayKfzSystems.Artikel.ArtikelModule")]
    [ModuleExport(typeof(ArtikelModule))]
    public class ArtikelModule : IModule
    {

        private IRegionManager regionManager;

        [ImportingConstructor]
        public ArtikelModule(IRegionManager regionManager)
        {
            // ToDo: Get your IEventAggregator, IRegionManager, IUnityContainer here
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.Artikel, typeof(ArtikelView));
        }
    }
}
