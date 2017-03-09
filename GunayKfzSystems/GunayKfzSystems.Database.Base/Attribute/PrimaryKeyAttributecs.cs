using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunayKfzSystems.Database.Base.Attribute
{
    public class PrimaryKeyAttribute : System.Attribute
    {
        private bool _isKey;
        public bool IsKey
        {
            get { return this._isKey; }
            set { this._isKey = value; }
        }


        public PrimaryKeyAttribute(bool isKey)
        {
            this.IsKey = isKey;
        }

    }
}
