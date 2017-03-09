using System.Data.OleDb;

namespace GunayKfzSystems.Database.MsAccess.Helper
{
    public static class OleDbExtention
    {
        public static int GetLatestAutonumber(this OleDbConnection connection)
        {
            using (OleDbCommand command = new OleDbCommand("SELECT @@IDENTITY;", connection))
            {
                return (int)command.ExecuteScalar();
            }
        }

    }
}
