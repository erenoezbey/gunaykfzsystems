using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using GunayKfzSystems.Database.Base.Helper;

using GunayKfzSystems.Database.MsAccess.Helper;
using GunayKfzSystems.Database.MsAccess.Properties;
using GunayKfzSystems.Interface.Database.MsAccess.Data;
using GunayKfzSystems.Interface.Database.MsAccess.Service;

using Prism.Logging;

namespace GunayKfzSystems.Database.MsAccess.Service
{

    /// <summary>
    /// The database design is really old fashioned, but nevertheless I leave the db model as it is. 
    /// </summary>
    [Export(typeof(IMsAccessServiceModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MsAccessService : IMsAccessServiceModule
    {
        [Import]
        private ILoggerFacade _logger;

        private OleDbConnection GetConnection()
        {
            var connectionString = Settings.Default.MsAccessConnectionString;
            return new OleDbConnection(connectionString);
        }

        private DataTable GetTableFromQuery(string query, Dictionary<string, object> parameters, CommandType commandType)
        {
            DataTable dataTable = new DataTable();

            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, db))
                {
                    cmd.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in parameters)
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        var i = cmd.CommandText;
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }


        public List<IService> GetAllServices()
        {
            List<IService> result = new List<IService>();
            try
            {
                var myTable = this.GetTableFromQuery("SELECT * FROM Aktion", null, CommandType.Text);

                myTable.DataTableToList<Data.Service>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }

            return result;

        }







        public List<IArtikel> GetAllArtikel()
        {
            List<IArtikel> result = new List<IArtikel>();
            try
            {
                var myTable = this.GetTableFromQuery("SELECT * FROM Artikel", null, CommandType.Text);

                myTable.DataTableToList<Data.Artikel>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }


            //try
            //{


            //    var firstArt = result.FirstOrDefault();
            //    PropertyInfo primaryKey = typeof(IArtikel).GetProperties(BindingFlags.Public | BindingFlags.Instance) // add other bindings if needed
            //                     .FirstOrDefault(x => x.GetCustomAttribute<PrimaryKeyAttribute>() != null
            //                                        && x.GetCustomAttribute<PrimaryKeyAttribute>().IsKey == true);

            //    int name = (int)primaryKey.GetValue(firstArt);


            //}
            //catch (Exception ex)
            //{


            //}


            return result;

        }





        public List<IFahrzeug> GetAllFahrzeug(Dictionary<string, object> parameters = null)
        {
            List<IFahrzeug> result = new List<IFahrzeug>();
            try
            {

                string sql = parameters != null
                                 ? "SELECT * FROM Fahrzeug where ID = ?"
                                 : "SELECT * FROM Fahrzeug";

                var myTable = this.GetTableFromQuery(sql, parameters, CommandType.Text);

                myTable.DataTableToList<Data.Fahrzeug>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }

            return result;

        }



        public List<IKunden> GetAllKunden(Dictionary<string, object> parameters = null)
        {
            List<IKunden> result = new List<IKunden>();
            try
            {

                string sql = parameters != null
                                 ? "SELECT * FROM Kunden where KundenNr = ?"
                                 : "SELECT * FROM Kunden";

                var myTable = this.GetTableFromQuery(sql, parameters, CommandType.Text);

                myTable.DataTableToList<Data.Kunden>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }

            return result;

        }


        public List<IRechnung> GetAllRechnungen()
        {
            List<IRechnung> result = new List<IRechnung>();
            try
            {
                var myTable = this.GetTableFromQuery("SELECT * FROM Rechnungen", null, CommandType.Text);

                myTable.DataTableToList<Data.Rechnung>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }
            return result;
        }


        public List<IVorfall> GetAllVorfall(Dictionary<string, object> parameters = null)
        {
            List<IVorfall> result = new List<IVorfall>();
            try
            {
                string sql = parameters != null
                                 ? "SELECT * FROM Vorfall where VorfallID = ?"
                                 : "SELECT * FROM Vorfall";


                var myTable = this.GetTableFromQuery(sql, parameters, CommandType.Text);

                myTable.DataTableToList<Data.Vorfall>(this._logger).ForEach(m => result.Add(m));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }
            return result;
        }

        public List<IVorfallKopfdaten> GetAllVorfallKopfdaten()
        {
            var result = new ConcurrentBag<IVorfallKopfdaten>();
            try
            {
                var myTable = this.GetTableFromQuery("SELECT * FROM VorfallKopfdaten ", null, CommandType.Text);

                Parallel.ForEach(myTable.DataTableToList<Data.VorfallKopfdaten>(this._logger), m =>
                                                                                     {
                                                                                         var dic = new Dictionary<string, object> { ["ID"] = m.FahrzeugID };

                                                                                         m.ConnectedFahrzeug = this.GetAllFahrzeug(dic).FirstOrDefault();


                                                                                         dic.Clear();
                                                                                         dic["KundenNr"] = m.KundenID;
                                                                                         m.ConnectedKunden = this.GetAllKunden(dic).FirstOrDefault();


                                                                                         result.Add(m);
                                                                                     });

            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, Category.Exception, Priority.High);
            }
            return result.OrderByDescending(m => m.ID).ToList();
        }

        public void UpdateArtikel(IArtikel selectedArtikel)
        {


            var updateSql = string.Format("UPDATE Artikel SET " +
                                          " ArtikelNrbeiLieferant='{1}' ," + Environment.NewLine +
                                          " Artikelbeschreibung='{2}' ," + Environment.NewLine +
                                          " Artikelname='{3}' ," + Environment.NewLine +
                                          " Einzelpreis={4} " + Environment.NewLine +
                                          " WHERE ArtikelNr={0} ",
                                          selectedArtikel.ArtikelNr,
                                          !string.IsNullOrEmpty(selectedArtikel.ArtikelNrbeiLieferant)
                                              ? selectedArtikel.ArtikelNrbeiLieferant
                                              : " ",
                                          !string.IsNullOrEmpty(selectedArtikel.Artikelbeschreibung)
                                              ? selectedArtikel.Artikelbeschreibung
                                              : " ",
                                          !string.IsNullOrEmpty(selectedArtikel.Artikelname)
                                              ? selectedArtikel.Artikelname
                                              : " ",
                                          selectedArtikel.Einzelpreis.GetValueOrDefault()
                );
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(updateSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }

        public int InsertArtikel(IArtikel selectedArtikel)
        {

            int newId;
            var insertSql =
                string.Format(
                              "INSERT INTO ARTIKEL (ArtikelNrbeiLieferant, Artikelbeschreibung, Artikelname, Einzelpreis) VALUES " +
                              "('{0}' ,'{1}' ,'{2}' ,{3} )",
                              !string.IsNullOrEmpty(selectedArtikel.ArtikelNrbeiLieferant)
                                  ? selectedArtikel.ArtikelNrbeiLieferant
                                  : " ",
                              !string.IsNullOrEmpty(selectedArtikel.Artikelbeschreibung)
                                  ? selectedArtikel.Artikelbeschreibung
                                  : " ",
                              !string.IsNullOrEmpty(selectedArtikel.Artikelname) ? selectedArtikel.Artikelname : " ",
                              selectedArtikel.Einzelpreis.GetValueOrDefault()
                    );
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(insertSql, db))
                {
                    cmd.ExecuteNonQuery();
                    newId = (int)db.GetLatestAutonumber();
                }
                db.Close();
            }

            return newId;


        }

        public void DeleteArtikel(IArtikel selectedArtikel)
        {
            var deleteSql = string.Format("DELETE FROM ARTIKEL WHERE ARTIKELNR = {0} ", selectedArtikel.ArtikelNr);
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(deleteSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }


        }

        public void UpdateService(IService selectedService)
        {
            var updateSql = string.Format(
                    "UPDATE AKTION " +
                    "SET AtionName = '{0}', Aktionpreis= {1}, Bemerkung='{2}' " +
                    "WHERE ID = {3}",
                    !string.IsNullOrEmpty(selectedService.AtionName)
                        ? selectedService.AtionName
                        : " ",
                     selectedService.Aktionpreis.GetValueOrDefault(),
                    !string.IsNullOrEmpty(selectedService.Bemerkung)
                        ? selectedService.Bemerkung
                        : " ",
                    selectedService.Id);

            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(updateSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }

        public int InsertService(IService selectedService)
        {
            int newId;
            var insertSql =
                string.Format(
                              "INSERT INTO AKTION (AtionName, Aktionpreis, Bemerkung) VALUES " +
                              "('{0}' ,{1} ,'{2}' )",
                              !string.IsNullOrEmpty(selectedService.AtionName)
                                  ? selectedService.AtionName
                                  : " ",
                               selectedService.Aktionpreis.GetValueOrDefault(),
                              !string.IsNullOrEmpty(selectedService.Bemerkung)
                                  ? selectedService.Bemerkung
                                  : " ");

            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(insertSql, db))
                {
                    cmd.ExecuteNonQuery();
                    newId = (int)db.GetLatestAutonumber();
                }
                db.Close();
            }

            return newId;
        }

        public void DeleteService(IService selectedService)
        {
            {
                var deleteSql = string.Format("DELETE FROM AKTION WHERE ID = {0} ", selectedService.Id);
                using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
                {
                    db.Open();
                    using (OleDbCommand cmd = new OleDbCommand(deleteSql, db))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    db.Close();
                }
            }
        }

        public void UpdateFahrzeug(IFahrzeug selectedFahrzeug)
        {
            var updateSql = string.Format(
                     "UPDATE FAHRZEUG " +
                     "SET Baujahr = '{0}', Kennzeichen= '{1}', Fabrikat='{2}', Typ = '{3}', Fahrgestellnummer= '{4}', Kilometerstand={5}, Bemerkung='{6}' " +
                     "WHERE ID = {7}",
                              !string.IsNullOrEmpty(selectedFahrzeug.Baujahr)
                                  ? selectedFahrzeug.Baujahr
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Kennzeichen)
                                  ? selectedFahrzeug.Kennzeichen
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Fabrikat)
                                  ? selectedFahrzeug.Fabrikat
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Typ)
                                  ? selectedFahrzeug.Typ
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Fahrgestellnummer)
                                  ? selectedFahrzeug.Fahrgestellnummer
                                  : " ",
                              selectedFahrzeug.Kilometerstand.GetValueOrDefault(),

                              !string.IsNullOrEmpty(selectedFahrzeug.Bemerkung)
                                  ? selectedFahrzeug.Bemerkung
                                  : " ",
                              selectedFahrzeug.ID
                                  );

            ExecuteUpdateSql(updateSql);
        }

        private static void ExecuteUpdateSql(string updateSql)
        {
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(updateSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }

        public int InsertFahrzeug(IFahrzeug selectedFahrzeug)
        {
            int newId;
            var insertSql =
                string.Format(
                              "INSERT INTO FAHRZEUG (Baujahr, Kennzeichen, Fabrikat, Typ,  Fahrgestellnummer, Kilometerstand, Bemerkung) VALUES " +
                              "('{0}' ,'{1}' ,'{2}', '{3}' ,'{4}' ,{5}, '{6}' )",

                              !string.IsNullOrEmpty(selectedFahrzeug.Baujahr)
                                  ? selectedFahrzeug.Baujahr
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Kennzeichen)
                                  ? selectedFahrzeug.Kennzeichen
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Fabrikat)
                                  ? selectedFahrzeug.Fabrikat
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Typ)
                                  ? selectedFahrzeug.Typ
                                  : " ",
                              !string.IsNullOrEmpty(selectedFahrzeug.Fahrgestellnummer)
                                  ? selectedFahrzeug.Fahrgestellnummer
                                  : " ",
                              selectedFahrzeug.Kilometerstand.GetValueOrDefault(),

                              !string.IsNullOrEmpty(selectedFahrzeug.Bemerkung)
                                  ? selectedFahrzeug.Bemerkung
                                  : " ");

            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(insertSql, db))
                {
                    cmd.ExecuteNonQuery();
                    newId = (int)db.GetLatestAutonumber();
                }
                db.Close();
            }

            return newId;
        }

        public void DeleteFahrzeug(IFahrzeug selectedFahrzeug)
        {

            var deleteSql = string.Format("DELETE FROM FAHRZEUG WHERE ID = {0} ", selectedFahrzeug.ID);
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(deleteSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }

        }

        public void DeleteKunden(IKunden selectedKunden)
        {

            var deleteSql = string.Format("DELETE FROM KUNDEN WHERE KundenNr = {0} ", selectedKunden.KundenNr);
            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(deleteSql, db))
                {
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }

        public void UpdateKunden(IKunden selectedKunden)
        {
            var updateSql = string.Format(
          "UPDATE KUNDEN " +
          "SET Firma = '{0}', KontaktVorname= '{1}', KontaktNachname='{2}', Rechnungsadresse = '{3}', Ort= '{4}', Bundesland='{5}', Postleitzahl='{6}', Telefonnummer = '{7}', Durchwahl= '{8}', Faxnummer='{9}', EmailAdresse = '{10}', Anmerkungen= '{11}' "
          + "WHERE KUNDENNR = {12}",
                   !string.IsNullOrEmpty(selectedKunden.Firma)
                       ? selectedKunden.Firma
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.KontaktVorname)
                       ? selectedKunden.KontaktVorname
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.KontaktNachname)
                       ? selectedKunden.KontaktNachname
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Rechnungsadresse)
                       ? selectedKunden.Rechnungsadresse
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Ort)
                       ? selectedKunden.Ort
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Bundesland)
                       ? selectedKunden.Bundesland
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Postleitzahl)
                       ? selectedKunden.Postleitzahl
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Telefonnummer)
                       ? selectedKunden.Telefonnummer
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Durchwahl)
                       ? selectedKunden.Durchwahl
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Faxnummer)
                       ? selectedKunden.Faxnummer
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.EmailAdresse)
                       ? selectedKunden.EmailAdresse
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Anmerkungen)
                       ? selectedKunden.Anmerkungen
                       : " ",
                  selectedKunden.KundenNr.GetValueOrDefault()
                       );

            ExecuteUpdateSql(updateSql);
        }

        public int InsertKunden(IKunden selectedKunden)
        {
            int newId;
            var insertSql =
                string.Format(
                              "INSERT INTO KUNDEN (Firma, KontaktVorname, KontaktNachname, Rechnungsadresse, Ort, Bundesland, Postleitzahl, Telefonnummer , Durchwahl, Faxnummer, EmailAdresse , Anmerkungen)  VALUES " +
                              "('{0}' ,'{1}' ,'{2}', '{3}' ,'{4}' ,'{5}', '{6}', '{7}' ,'{8}' ,'{9}', '{10}' ,'{11}')",

                                 !string.IsNullOrEmpty(selectedKunden.Firma)
                       ? selectedKunden.Firma
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.KontaktVorname)
                       ? selectedKunden.KontaktVorname
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.KontaktNachname)
                       ? selectedKunden.KontaktNachname
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Rechnungsadresse)
                       ? selectedKunden.Rechnungsadresse
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Ort)
                       ? selectedKunden.Ort
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Bundesland)
                       ? selectedKunden.Bundesland
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Postleitzahl)
                       ? selectedKunden.Postleitzahl
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Telefonnummer)
                       ? selectedKunden.Telefonnummer
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Durchwahl)
                       ? selectedKunden.Durchwahl
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Faxnummer)
                       ? selectedKunden.Faxnummer
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.EmailAdresse)
                       ? selectedKunden.EmailAdresse
                       : " ",
                   !string.IsNullOrEmpty(selectedKunden.Anmerkungen)
                       ? selectedKunden.Anmerkungen
                       : " "

                              );

            using (OleDbConnection db = new OleDbConnection(Settings.Default.MsAccessConnectionString))
            {
                db.Open();
                using (OleDbCommand cmd = new OleDbCommand(insertSql, db))
                {
                    cmd.ExecuteNonQuery();
                    newId = (int)db.GetLatestAutonumber();
                }
                db.Close();
            }

            return newId;
        }

        public void UpdateWartung(IVorfallKopfdaten selectedWartung)
        {
            throw new NotImplementedException();
        }

        public int InsertWartung(IVorfallKopfdaten selectedWartung)
        {
            throw new NotImplementedException();
        }

        public void DeleteWartung(IVorfallKopfdaten selectedWartung)
        {
            throw new NotImplementedException();
        }
    }
}
