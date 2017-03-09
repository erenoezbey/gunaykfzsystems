using System;

namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IVorfall
    {
        int ID { get; set; }
        int VorfallTyp { get; set; }
        int VorfallTID { get; set; }
        int Anztahl { get; set; }
        int Sonderkondition { get; set; }
        DateTime Zeitstempel { get; set; }
        int VorfallID { get; set; }
        string Bezeichnung { get; set; }
    }

}
