using Dapper.Contrib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FW.Model
{
    public partial class Menu
    {


        [Write(false)]
        public List<Menu> Children  { get;set; }

        public string TargetMenuClass
        {
            get
            {
                return this.Target == 0 ? "J_menuItem": "";
            }

            private set { }
        }
          
    }

    public partial class Menu_ 
    {

         
    }
}
