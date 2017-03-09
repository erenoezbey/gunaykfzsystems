using GunayKfzSystems.Database.Base.Attribute;
using GunayKfzSystems.Interface.Database.MsAccess.Data;

namespace GunayKfzSystems.Database.MsAccess.Data
{

    public class Service : IService
    {
        [PrimaryKey(true)]

        public int Id { get; set; }
        public string AtionName { get; set; }
        public double? Aktionpreis { get; set; }
        public string Bemerkung { get; set; }
    }
}
