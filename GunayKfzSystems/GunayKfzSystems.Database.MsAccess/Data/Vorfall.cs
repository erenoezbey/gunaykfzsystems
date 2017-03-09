using System;
using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{
   
    public class Vorfall : IVorfall
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int VorfallTyp { get; set; }
        public int VorfallTID { get; set; }
        public int Anztahl { get; set; }
        public int Sonderkondition { get; set; }
        public DateTime Zeitstempel { get; set; }
        public int VorfallID { get; set; }
        public string Bezeichnung { get; set; }
    }
}
