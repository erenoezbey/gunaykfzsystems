using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{

    public class Kunden : IKunden
    {
        [PrimaryKey(true)]
        public int? KundenNr { get; set; }
        public string Firma { get; set; }
        public string KontaktVorname { get; set; }
        public string KontaktNachname { get; set; }
        public string Rechnungsadresse { get; set; }
        public string Ort { get; set; }
        public string Bundesland { get; set; }
        public string Postleitzahl { get; set; }
        public string Telefonnummer { get; set; }
        public string Durchwahl { get; set; }
        public string Faxnummer { get; set; }
        public string EmailAdresse { get; set; }
        public string Anmerkungen { get; set; }
    }
}
