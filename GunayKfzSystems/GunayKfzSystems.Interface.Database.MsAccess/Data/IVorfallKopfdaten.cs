using System;
using System.Collections.Generic;

namespace GunayKfzSystems.Interface.Database.MsAccess.Data
{
    public interface IVorfallKopfdaten 
    {
    
        int ID { get; set; }
        /// <summary>
        /// Id eines Fahrzeugs. Eindeutig
        /// </summary>
        int FahrzeugID { get; set; }
        /// <summary>
        /// Id des Kunden. Eindeutig
        /// </summary>
        int KundenID { get; set; }
        /// <summary>
        /// Timespamp 
        /// </summary>
        DateTime Zeitstempel { get; set; }
        /// <summary>
        /// RechnungsId. Eindeutig 
        /// </summary>
        int RecnungsID { get; set; }

        /// <summary>
        /// Kommentare zum Vorfall
        /// </summary>
        string Bemerkung { get; set; }

     
        List<IVorfall> VorfallCollection { get; set; }

        IFahrzeug ConnectedFahrzeug { get; set; }

        IKunden ConnectedKunden { get; set; }

    }

}
    