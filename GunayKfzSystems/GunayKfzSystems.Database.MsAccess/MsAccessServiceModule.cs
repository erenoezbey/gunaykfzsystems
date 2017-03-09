using Prism.Mef.Modularity;
using Prism.Modularity;

namespace GunayKfzSystems.Database.MsAccess
{
    [Module(ModuleName = "GunayKfzSystems.Database.MsAccess")]
    [ModuleExport(typeof(MsAccessServiceModule))]
    public class MsAccessServiceModule: IModule
    {
        #region Implementation of IModule

        public void Initialize()
        {
            
        }

        #endregion
    }
}
