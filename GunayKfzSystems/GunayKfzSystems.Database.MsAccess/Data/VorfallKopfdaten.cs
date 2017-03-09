using System;
using System.Collections.Generic;
using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{

    public class VorfallKopfdaten : IVorfallKopfdaten
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int FahrzeugID { get; set; }
        public int KundenID { get; set; }
        public DateTime Zeitstempel { get; set; }
        public int RecnungsID { get; set; }
        public string Bemerkung { get; set; }

        [Ignorable(true)]
        public List<IVorfall> VorfallCollection { get; set; }
        [Ignorable(true)]
        public IFahrzeug ConnectedFahrzeug { get; set; }
        [Ignorable(true)]
        public IKunden ConnectedKunden { get; set; }
    }
}
