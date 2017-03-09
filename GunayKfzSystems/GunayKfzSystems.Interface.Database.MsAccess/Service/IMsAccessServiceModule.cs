using System.Collections.Generic;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Interface.Database.MsAccess.Service
{
    public interface IMsAccessServiceModule
    {
        List<IService> GetAllServices();
        List<IArtikel> GetAllArtikel();
        List<IFahrzeug> GetAllFahrzeug(Dictionary<string, object> paramters = null);
        List<IKunden> GetAllKunden(Dictionary<string, object> paramters = null);
        List<IRechnung> GetAllRechnungen();
        List<IVorfall> GetAllVorfall(Dictionary<string, object> paramters = null);
        List<IVorfallKopfdaten> GetAllVorfallKopfdaten();
        void UpdateArtikel(IArtikel selectedArtikel);
        int InsertArtikel(IArtikel selectedArtikel);
        void DeleteArtikel(IArtikel selectedArtikel);
        void UpdateService(IService selectedArtikel);
        int InsertService(IService selectedArtikel);
        void DeleteService(IService selectedArtikel);
        void UpdateFahrzeug(IFahrzeug selectedFahrzeug);
        int InsertFahrzeug(IFahrzeug selectedFahrzeug);
        void DeleteFahrzeug(IFahrzeug selectedFahrzeug);
        void DeleteKunden(IKunden selectedKunden);
        void UpdateKunden(IKunden selectedKunden);
        int InsertKunden(IKunden selectedKunden);
        void UpdateWartung(IVorfallKopfdaten selectedWartung);
        int InsertWartung(IVorfallKopfdaten selectedWartung);
        void DeleteWartung(IVorfallKopfdaten selectedWartung);
    }
}
