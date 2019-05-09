
using System;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FW.Model
{

    /// <summary>
    /// A class which represents the Menu table.
    /// </summary>
	[Table("Menu")]
	public partial class Menu
	{  
	   /*  Id  ParentId  Title  img  href  seq  Target  IsDel  Level  Lead  TitleTag  */ 

	    
        #region 待写入字段集合 可抽象出来
		private bool _IsWriteFiled = true; // 默认记录赋值过的字段
        [WriteFiled]
        public System.Collections.Generic.List<System.Reflection.PropertyInfo> _wf 
						= new System.Collections.Generic.List<System.Reflection.PropertyInfo>();
						
        public void SetWriteFiled(bool ib = true) => this._IsWriteFiled = ib;
		#endregion
		 
        public Menu() {
            this._IsWriteFiled = false;
        }
        public Menu(bool isWrite) {
            this._IsWriteFiled = isWrite;
        }

        #region FieldName
		public static readonly string  Field_Id = "Id"; 
		public static readonly string  Field_ParentId = "ParentId"; 
		public static readonly string  Field_Title = "Title"; 
		public static readonly string  Field_img = "img"; 
		public static readonly string  Field_href = "href"; 
		public static readonly string  Field_seq = "seq"; 
		public static readonly string  Field_Target = "Target"; 
		public static readonly string  Field_IsDel = "IsDel"; 
		public static readonly string  Field_Level = "Level"; 
		public static readonly string  Field_Lead = "Lead"; 
		public static readonly string  Field_TitleTag = "TitleTag"; 
		#endregion

        #region Field
		private int _Id ; 
		private int _ParentId ; 
		private string _Title ; 
		private string _img ; 
		private string _href ; 
		private int? _seq ; 
		private int _Target ; 
		private int _IsDel ; 
		private int _Level ; 
		private int _Lead ; 
		private string _TitleTag ; 
        #endregion

		[Key]
		public virtual int Id { 
			set { _Id = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_Id) ); }
			get { return _Id; }
		}
		public virtual int ParentId { 
			set { _ParentId = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_ParentId) ); }
			get { return _ParentId; }
		}
		public virtual string Title { 
			set { _Title = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_Title) ); }
			get { return _Title; }
		}
		public virtual string img { 
			set { _img = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_img) ); }
			get { return _img; }
		}
		public virtual string href { 
			set { _href = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_href) ); }
			get { return _href; }
		}
		public virtual int? seq { 
			set { _seq = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_seq) ); }
			get { return _seq; }
		}
		public virtual int Target { 
			set { _Target = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_Target) ); }
			get { return _Target; }
		}
		public virtual int IsDel { 
			set { _IsDel = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_IsDel) ); }
			get { return _IsDel; }
		}
		public virtual int Level { 
			set { _Level = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_Level) ); }
			get { return _Level; }
		}
		public virtual int Lead { 
			set { _Lead = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_Lead) ); }
			get { return _Lead; }
		}
		public virtual string TitleTag { 
			set { _TitleTag = value; 
					if(_IsWriteFiled) _wf.Add(this.GetType().GetProperty(Field_TitleTag) ); }
			get { return _TitleTag; }
		}

	}

//	################
    /// <summary>
    /// 查询用 只存数据的实体  Menu_ table.
    /// </summary>
	[Table("Menu")]
	public partial class Menu_
	{  
	   /*  Id  ParentId  Title  img  href  seq  Target  IsDel  Level  Lead  TitleTag  */ 

	      
        #region Field
		private int _Id ;
		private int _ParentId ;
		private string _Title ;
		private string _img ;
		private string _href ;
		private int? _seq ;
		private int _Target ;
		private int _IsDel ;
		private int _Level ;
		private int _Lead ;
		private string _TitleTag ;
        #endregion

		[Key]
		public virtual int Id { 
			set { _Id = value; }
			get { return _Id; }
		}
		public virtual int ParentId { 
			set { _ParentId = value; }
			get { return _ParentId; }
		}
		public virtual string Title { 
			set { _Title = value; }
			get { return _Title; }
		}
		public virtual string img { 
			set { _img = value; }
			get { return _img; }
		}
		public virtual string href { 
			set { _href = value; }
			get { return _href; }
		}
		public virtual int? seq { 
			set { _seq = value; }
			get { return _seq; }
		}
		public virtual int Target { 
			set { _Target = value; }
			get { return _Target; }
		}
		public virtual int IsDel { 
			set { _IsDel = value; }
			get { return _IsDel; }
		}
		public virtual int Level { 
			set { _Level = value; }
			get { return _Level; }
		}
		public virtual int Lead { 
			set { _Lead = value; }
			get { return _Lead; }
		}
		public virtual string TitleTag { 
			set { _TitleTag = value; }
			get { return _TitleTag; }
		}

	}






} // namespace
