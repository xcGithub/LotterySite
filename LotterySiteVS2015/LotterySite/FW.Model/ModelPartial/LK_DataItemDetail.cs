
using System;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FW.Model
{

    /// <summary>
    /// A class which represents the LK_DataItemDetail table.
    /// </summary>
	[Table("LK_DataItemDetail")]
	public partial class LKDataItemDetail
	{  
	   /*  F_ItemDetailId  F_ItemId  F_ParentId  F_ItemCode  F_ItemName  F_ItemValue  F_QuickQuery  F_SimpleSpelling  F_IsDefault  F_SortCode  F_DeleteMark  F_EnabledMark  F_Description  F_CreateDate  F_CreateUserId  F_CreateUserName  F_ModifyDate  F_ModifyUserId  F_ModifyUserName  F_Level  */ 

	    
        #region 待写入字段集合 可抽象出来
		private bool _IsWriteFiled = true; // 默认记录赋值过的字段
        [WriteFiled]
        public System.Collections.Generic.List<System.Reflection.PropertyInfo> _wf 
						= new System.Collections.Generic.List<System.Reflection.PropertyInfo>();
						
        public void SetWriteFiled(bool ib = true) => this._IsWriteFiled = ib;
		#endregion
		 
        public LKDataItemDetail() {
            this._IsWriteFiled = false;
        }
        public LKDataItemDetail(bool isWrite) {
            this._IsWriteFiled = isWrite;
        }

        #region FieldName
		public static readonly string  Field_F_ItemDetailId = "F_ItemDetailId"; 
		public static readonly string  Field_F_ItemId = "F_ItemId"; 
		public static readonly string  Field_F_ParentId = "F_ParentId"; 
		public static readonly string  Field_F_ItemCode = "F_ItemCode"; 
		public static readonly string  Field_F_ItemName = "F_ItemName"; 
		public static readonly string  Field_F_ItemValue = "F_ItemValue"; 
		public static readonly string  Field_F_QuickQuery = "F_QuickQuery"; 
		public static readonly string  Field_F_SimpleSpelling = "F_SimpleSpelling"; 
		public static readonly string  Field_F_IsDefault = "F_IsDefault"; 
		public static readonly string  Field_F_SortCode = "F_SortCode"; 
		public static readonly string  Field_F_DeleteMark = "F_DeleteMark"; 
		public static readonly string  Field_F_EnabledMark = "F_EnabledMark"; 
		public static readonly string  Field_F_Description = "F_Description"; 
		public static readonly string  Field_F_CreateDate = "F_CreateDate"; 
		public static readonly string  Field_F_CreateUserId = "F_CreateUserId"; 
		public static readonly string  Field_F_CreateUserName = "F_CreateUserName"; 
		public static readonly string  Field_F_ModifyDate = "F_ModifyDate"; 
		public static readonly string  Field_F_ModifyUserId = "F_ModifyUserId"; 
		public static readonly string  Field_F_ModifyUserName = "F_ModifyUserName"; 
		public static readonly string  Field_F_Level = "F_Level"; 
		#endregion

        #region Field
		private string _F_ItemDetailId ; 
		private string _F_ItemId ; 
		private string _F_ParentId ; 
		private string _F_ItemCode ; 
		private string _F_ItemName ; 
		private string _F_ItemValue ; 
		private string _F_QuickQuery ; 
		private string _F_SimpleSpelling ; 
		private int? _F_IsDefault ; 
		private int? _F_SortCode ; 
		private int? _F_DeleteMark ; 
		private int? _F_EnabledMark ; 
		private string _F_Description ; 
		private DateTime? _F_CreateDate ; 
		private string _F_CreateUserId ; 
		private string _F_CreateUserName ; 
		private DateTime? _F_ModifyDate ; 
		private string _F_ModifyUserId ; 
		private string _F_ModifyUserName ; 
		private int? _F_Level ; 
        #endregion

		[ExplicitKey] // 手动插入(主)键
		public virtual string F_ItemDetailId { 
			set { _F_ItemDetailId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ItemDetailId) ); }
			get { return _F_ItemDetailId; }
		}
		public virtual string F_ItemId { 
			set { _F_ItemId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ItemId) ); }
			get { return _F_ItemId; }
		}
		public virtual string F_ParentId { 
			set { _F_ParentId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ParentId) ); }
			get { return _F_ParentId; }
		}
		public virtual string F_ItemCode { 
			set { _F_ItemCode = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ItemCode) ); }
			get { return _F_ItemCode; }
		}
		public virtual string F_ItemName { 
			set { _F_ItemName = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ItemName) ); }
			get { return _F_ItemName; }
		}
		public virtual string F_ItemValue { 
			set { _F_ItemValue = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ItemValue) ); }
			get { return _F_ItemValue; }
		}
		public virtual string F_QuickQuery { 
			set { _F_QuickQuery = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_QuickQuery) ); }
			get { return _F_QuickQuery; }
		}
		public virtual string F_SimpleSpelling { 
			set { _F_SimpleSpelling = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_SimpleSpelling) ); }
			get { return _F_SimpleSpelling; }
		}
		public virtual int? F_IsDefault { 
			set { _F_IsDefault = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_IsDefault) ); }
			get { return _F_IsDefault; }
		}
		public virtual int? F_SortCode { 
			set { _F_SortCode = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_SortCode) ); }
			get { return _F_SortCode; }
		}
		public virtual int? F_DeleteMark { 
			set { _F_DeleteMark = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_DeleteMark) ); }
			get { return _F_DeleteMark; }
		}
		public virtual int? F_EnabledMark { 
			set { _F_EnabledMark = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_EnabledMark) ); }
			get { return _F_EnabledMark; }
		}
		public virtual string F_Description { 
			set { _F_Description = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_Description) ); }
			get { return _F_Description; }
		}
		public virtual DateTime? F_CreateDate { 
			set { _F_CreateDate = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_CreateDate) ); }
			get { return _F_CreateDate; }
		}
		public virtual string F_CreateUserId { 
			set { _F_CreateUserId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_CreateUserId) ); }
			get { return _F_CreateUserId; }
		}
		public virtual string F_CreateUserName { 
			set { _F_CreateUserName = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_CreateUserName) ); }
			get { return _F_CreateUserName; }
		}
		public virtual DateTime? F_ModifyDate { 
			set { _F_ModifyDate = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ModifyDate) ); }
			get { return _F_ModifyDate; }
		}
		public virtual string F_ModifyUserId { 
			set { _F_ModifyUserId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ModifyUserId) ); }
			get { return _F_ModifyUserId; }
		}
		public virtual string F_ModifyUserName { 
			set { _F_ModifyUserName = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_ModifyUserName) ); }
			get { return _F_ModifyUserName; }
		}
		public virtual int? F_Level { 
			set { _F_Level = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_F_Level) ); }
			get { return _F_Level; }
		}

	}

//	################
    /// <summary>
    /// 查询用 只存数据的实体  LK_DataItemDetail_ table.
    /// </summary>
	[Table("LK_DataItemDetail")]
	public partial class LKDataItemDetail_
	{  
	   /*  F_ItemDetailId  F_ItemId  F_ParentId  F_ItemCode  F_ItemName  F_ItemValue  F_QuickQuery  F_SimpleSpelling  F_IsDefault  F_SortCode  F_DeleteMark  F_EnabledMark  F_Description  F_CreateDate  F_CreateUserId  F_CreateUserName  F_ModifyDate  F_ModifyUserId  F_ModifyUserName  F_Level  */ 

	      
        #region Field
		private string _F_ItemDetailId ;
		private string _F_ItemId ;
		private string _F_ParentId ;
		private string _F_ItemCode ;
		private string _F_ItemName ;
		private string _F_ItemValue ;
		private string _F_QuickQuery ;
		private string _F_SimpleSpelling ;
		private int? _F_IsDefault ;
		private int? _F_SortCode ;
		private int? _F_DeleteMark ;
		private int? _F_EnabledMark ;
		private string _F_Description ;
		private DateTime? _F_CreateDate ;
		private string _F_CreateUserId ;
		private string _F_CreateUserName ;
		private DateTime? _F_ModifyDate ;
		private string _F_ModifyUserId ;
		private string _F_ModifyUserName ;
		private int? _F_Level ;
        #endregion

		[ExplicitKey] // 手动插入(主)键
		public virtual string F_ItemDetailId { 
			set { _F_ItemDetailId = value; }
			get { return _F_ItemDetailId; }
		}
		public virtual string F_ItemId { 
			set { _F_ItemId = value; }
			get { return _F_ItemId; }
		}
		public virtual string F_ParentId { 
			set { _F_ParentId = value; }
			get { return _F_ParentId; }
		}
		public virtual string F_ItemCode { 
			set { _F_ItemCode = value; }
			get { return _F_ItemCode; }
		}
		public virtual string F_ItemName { 
			set { _F_ItemName = value; }
			get { return _F_ItemName; }
		}
		public virtual string F_ItemValue { 
			set { _F_ItemValue = value; }
			get { return _F_ItemValue; }
		}
		public virtual string F_QuickQuery { 
			set { _F_QuickQuery = value; }
			get { return _F_QuickQuery; }
		}
		public virtual string F_SimpleSpelling { 
			set { _F_SimpleSpelling = value; }
			get { return _F_SimpleSpelling; }
		}
		public virtual int? F_IsDefault { 
			set { _F_IsDefault = value; }
			get { return _F_IsDefault; }
		}
		public virtual int? F_SortCode { 
			set { _F_SortCode = value; }
			get { return _F_SortCode; }
		}
		public virtual int? F_DeleteMark { 
			set { _F_DeleteMark = value; }
			get { return _F_DeleteMark; }
		}
		public virtual int? F_EnabledMark { 
			set { _F_EnabledMark = value; }
			get { return _F_EnabledMark; }
		}
		public virtual string F_Description { 
			set { _F_Description = value; }
			get { return _F_Description; }
		}
		public virtual DateTime? F_CreateDate { 
			set { _F_CreateDate = value; }
			get { return _F_CreateDate; }
		}
		public virtual string F_CreateUserId { 
			set { _F_CreateUserId = value; }
			get { return _F_CreateUserId; }
		}
		public virtual string F_CreateUserName { 
			set { _F_CreateUserName = value; }
			get { return _F_CreateUserName; }
		}
		public virtual DateTime? F_ModifyDate { 
			set { _F_ModifyDate = value; }
			get { return _F_ModifyDate; }
		}
		public virtual string F_ModifyUserId { 
			set { _F_ModifyUserId = value; }
			get { return _F_ModifyUserId; }
		}
		public virtual string F_ModifyUserName { 
			set { _F_ModifyUserName = value; }
			get { return _F_ModifyUserName; }
		}
		public virtual int? F_Level { 
			set { _F_Level = value; }
			get { return _F_Level; }
		}

	}


} // namespace
