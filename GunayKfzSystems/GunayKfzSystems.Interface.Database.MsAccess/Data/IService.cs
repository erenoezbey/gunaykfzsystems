namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IService
    {
      
        int Id { get; set; }
        string AtionName { get; set; }
        double? Aktionpreis { get; set; }
        string Bemerkung { get; set; }
    }

}
