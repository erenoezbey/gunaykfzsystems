using System;
using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{


    public class Rechnung : IRechnung
    {
        [PrimaryKey(true)]
        public int RechnungsNr { get; set; }
        public int KundenNr { get; set; }
        public string Status { get; set; }
        public DateTime Rechnungsdatum { get; set; }

    }
}
