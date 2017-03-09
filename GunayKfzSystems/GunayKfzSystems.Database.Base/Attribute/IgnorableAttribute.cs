using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunayKfzSystems.Database.Base.Attribute
{
    public class IgnorableAttribute : System.Attribute
    {
        private bool _isIgnorable;
        public bool IsIgnorable
        {
            get { return this._isIgnorable; }
            set { this._isIgnorable = value; }
        }



        public IgnorableAttribute(bool isIgnorable)
        {
            this.IsIgnorable = isIgnorable;
        }
    }
}
