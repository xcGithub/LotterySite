using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FW.Model
{
    public partial class Users
    {
        //public Users() { this.LockPerss = new List<LockPers>(); }

        [Write(false)]
        public List<LockPers> LockPerss { get; set; }


        [Write(false)]
        public virtual string RoleName { get; set; }
        [Write(false)]
        public virtual string bgurl { get; set; }
    }
}
