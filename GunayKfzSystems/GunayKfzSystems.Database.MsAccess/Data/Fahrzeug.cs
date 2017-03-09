using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{
 

    public class Fahrzeug : IFahrzeug
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Baujahr { get; set; }
        public string Kennzeichen { get; set; }
        public string Fabrikat { get; set; }
        public string Typ { get; set; }
        public string Fahrgestellnummer { get; set; }
        public int? Kilometerstand { get; set; }
        public string Bemerkung { get; set; }
    }
}
