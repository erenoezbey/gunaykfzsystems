using System;

namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IRechnung
    {
      
        int RechnungsNr { get; set; }
        int KundenNr { get; set; }
        string Status { get; set; }
        DateTime Rechnungsdatum { get; set; }
    }
}
