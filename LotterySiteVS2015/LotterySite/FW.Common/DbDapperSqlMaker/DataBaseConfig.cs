using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;

namespace DapperSqlMaker
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DataBaseConfig
    {
        #region SqlServer链接配置
        /// <summary>
        /// 默认的Sql Server的链接字符串
        /// </summary>
        public static string DefaultSqlConnectionString = @"server=127.0.0.1,1433;database=LockTest;uid=sa;pwd=sa123;";
        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }
        #endregion

        #region SqlLite链接配置

        public static readonly string LockSqlLiteConnectionString =
            "Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + //"db/Lock.db";

        System.Configuration.ConfigurationManager.ConnectionStrings["LockSqlite"].ConnectionString;
        public static IDbConnection GetSqliteConnection(string sqliteConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqliteConnectionString))
            {
                sqliteConnectionString = LockSqlLiteConnectionString;
            }
            SQLiteConnection conn = new SQLiteConnection(sqliteConnectionString);
            conn.Open();
            return conn;
        }

        #endregion
    }
}

//namespace DapperSqlMaker.DapperExt {
//    public partial class DapperFuncs {


//    }
//}
