
using System;
using System.Data;
using System.Data.SQLite;

namespace DapperSqlMaker.DapperExt
{
    public partial class DapperFuncs : DapperFuncsBase
    {
        private DapperFuncs() { }

        public readonly static DapperFuncs New = new DapperFuncs();
        public override IDbConnection GetConn()
        {
            SQLiteConnection conn = new SQLiteConnection(DataBaseConfig.LockSqlLiteConnectionString);
            conn.Open();
            return conn;
        }
    }
    /// <summary>
    /// Sqlite库1
    /// </summary>
    public partial class LockSqlite : DapperSqlMaker
    {

        private LockSqlite() { }

        private readonly static LockSqlite _New = new LockSqlite();
        public static LockSqlite New()
        {
            return _New;
        }
        //public IDbConnection GetConnSign(bool isfirst) {
        //    return this.GetConn();
        //}

        public override IDbConnection GetConn()
        {
            SQLiteConnection conn = new SQLiteConnection(DataBaseConfig.LockSqlLiteConnectionString);
            conn.Open();
            return conn;
        }

    }

    public partial class LockSqlite<T> : DapperSqlMaker<T>
                                         where T : class, new()
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }

        public static DapperSqlMaker<T> Selec()
        {
            return new LockSqlite<T>().Select();
        }
        public static DapperSqlMaker<T> Inser()
        {
            return new LockSqlite<T>().Insert();
        }
        public static DapperSqlMaker<T> Updat()
        {
            return new LockSqlite<T>().Update();
        }
        public static DapperSqlMaker<T> Delet()
        {
            return new LockSqlite<T>().Delete();
        }

        /// <summary>
        /// 增删改
        /// </summary>
        public readonly static DapperSqlMaker<T> Cud = new LockSqlite<T>();

    }
    public partial class LockSqlite<T, Y> : DapperSqlMaker<T, Y>
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }

        // 不能用单例 单例后面的表别名字典会冲突
        public static DapperSqlMaker<T, Y> Selec()
        {
            return new LockSqlite<T, Y>().Select();
        }
    }

    public partial class LockSqlite<T, Y, Z> : DapperSqlMaker<T, Y, Z>
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }
        // 不能用单例 单例后面的表别名字典会冲突

        public static DapperSqlMaker<T, Y, Z> Selec()
        {
            return new LockSqlite<T, Y, Z>().Select();
        }

    }
    public partial class LockSqlite<T, Y, Z, O> : DapperSqlMaker<T, Y, Z, O>
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }
        // 不能用单例 单例后面的表别名字典会冲突

        public static DapperSqlMaker<T, Y, Z, O> Selec()
        {
            return new LockSqlite<T, Y, Z, O>().Select();
        }
    }
    public partial class LockSqlite<T, Y, Z, O, P> : DapperSqlMaker<T, Y, Z, O, P>
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }
        // 不能用单例 单例后面的表别名字典会冲突

        public static DapperSqlMaker<T, Y, Z, O, P> Selec()
        {
            return new LockSqlite<T, Y, Z, O, P>().Select();
        }
    }
    public partial class LockSqlite<T, Y, Z, O, P, Q> : DapperSqlMaker<T, Y, Z, O, P, Q>
    {
        public override IDbConnection GetConn()
        {
            return LockSqlite.New().GetConn();
        }
        // 不能用单例 单例后面的表别名字典会冲突

        public static DapperSqlMaker<T, Y, Z, O, P, Q> Selec()
        {
            return new LockSqlite<T, Y, Z, O, P, Q>().Select();
        }
    }



}
