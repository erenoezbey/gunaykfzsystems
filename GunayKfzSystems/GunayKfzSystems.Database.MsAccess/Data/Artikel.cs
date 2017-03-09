using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{


    public class Artikel : IArtikel
    {
        [PrimaryKey(true)]
        public int ArtikelNr { get; set; }
        public string Artikelname { get; set; }
        public string Artikelbeschreibung { get; set; }
        public string ArtikelNrbeiLieferant { get; set; }
        public double? Einzelpreis { get; set; }


        

    }
}
