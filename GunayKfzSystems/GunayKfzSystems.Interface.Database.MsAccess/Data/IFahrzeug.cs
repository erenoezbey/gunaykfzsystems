namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IFahrzeug
    {
     
        int ID { get; set; }
        string Baujahr { get; set; }
        string Kennzeichen { get; set; }
        string Fabrikat { get; set; }
        string Typ { get; set; }
        string Fahrgestellnummer { get; set; }
        int? Kilometerstand { get; set; }
        string Bemerkung { get; set; }
    }
}
