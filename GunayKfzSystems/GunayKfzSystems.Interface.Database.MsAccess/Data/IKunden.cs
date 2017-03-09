namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IKunden
    {
     
        int? KundenNr { get; set; }
        string Firma { get; set; }
        string KontaktVorname { get; set; }
        string KontaktNachname { get; set; }
        string Rechnungsadresse { get; set; }
        string Ort { get; set; }
        string Bundesland { get; set; }
        string Postleitzahl { get; set; }
        string Telefonnummer { get; set; }
        string Durchwahl { get; set; }
        string Faxnummer { get; set; }
        string EmailAdresse { get; set; }
        string Anmerkungen { get; set; }
    }

}
