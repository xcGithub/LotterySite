
using System;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FW.Model
{

    /// <summary>
    /// A class which represents the LockPers table.
    /// </summary>
	public partial class LockPers
	{
        /*  Name  Content  Prompt  Id  InsertTime  IsDel  DelTime  */

        //public virtual string Name { get; set; }
        //public virtual string Content { get; set; }
        //      // this.OnPropertyValueChange()  
        //      public virtual string Prompt { get; set; }
        //      字段名为id会默认当作主键   [ExplicitKey]  // 显示键 代码中赋值
        //public virtual string Id { get; set; }
        //public virtual DateTime? InsertTime { get; set; }
        //public virtual string IsDel { get; set; }
        //public virtual DateTime? DelTime { get; set; }
        //      public virtual DateTime? UpdateTime { get; set; }
         
        //public LockPers() {
        //    this._IsWriteFiled = false; // 默认不记录赋值字段
        //}  //this._IsWriteFiled = false; }  

        private int _rowNum;
        [Write(false)]
        public virtual int rowNum
        {
            get { return _rowNum; }
            set { _rowNum = value; }
        }

        private string _InsertTimestr;
        [Write(false)]
        public virtual string InsertTimestr
        {
            get { return _InsertTimestr; }
            set { _InsertTimestr = value; }
        }

        private string _ContentOld;
        [Write(false)]
        public virtual string ContentOld {
            get { return _ContentOld; }
            set {
                _ContentOld = value;
                // _WriteFiled.Add(this.GetType().GetProperty("ContentOld") );  // 非库字段无需记录
            }
        }


    }
     
} // namespace
