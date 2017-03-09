namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IArtikel
    {
      
        int ArtikelNr { get; set; }
        string Artikelname { get; set; }
        string Artikelbeschreibung { get; set; }
        string ArtikelNrbeiLieferant { get; set; }
        double? Einzelpreis { get; set; }
    }
}
