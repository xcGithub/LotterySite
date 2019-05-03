using Dapper;
using DapperSqlMaker.DapperExt;
using FW.Common;
using FW.Model;
using Newtonsoft.Json;
using QnCmsData.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using xxoo.Common;

namespace LotteryWeb.Controllers
{
    public class LockController : Controller
    {
        //
        // GET: /Lock/
        // 0 false 1 true 

        public ActionResult Index()
        {
            
            var query = LockDapperUtilsqlite<LockPers>
                .Selec().Column().From().Where(m => m.IsDel != true).Order(m => new { m.Name });
            //Tuple<StringBuilder, Dapper.DynamicParameters> ru = query.RawSqlParams();
            var list =  query.ExecuteQuery<LockPers>(); 
            ViewData.Model = list;

            //string sql = string.Format("SELECT * FROM LockPers where isDel != 1  order by Name ");
            //DataTable dt = LockSQLitehelper.ExecuteTable(sql);
            //ViewData.Model = DataToModelHelper.RefDataTableToList<LockPers>(dt);
            //var str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            return View();
        }

        public ActionResult Check(LockPers p)
        {
            var query = LockDapperUtilsqlite<LockPers>
                        .Selec().Column(c => new { c.Content, c.CheckCount }).From().Where(m => m.Id == p.Id);
            LockPers oldlp = query.ExecuteQuery<LockPers>().FirstOrDefault();

            //string sql = string.Format("SELECT Content FROM LockPers where Id = @Id");
            //var content = LockSQLitehelper.ExecuteScalar(sql, new SQLiteParameter("@Id", DbType.String) { Value = p.Id }) + "";

            // 更新 核验次数
            oldlp._IsWriteFiled = true;
            oldlp.CheckCount = oldlp.CheckCount + 1;
            var content = oldlp.Content;
            var encstr = LockEncrypt.StringToMD5(p.Content);
            DapperFuncs .Update<LockPers>(oldlp, w => w.Id == p.Id);
            return Content(Convert.ToInt32(content.Equals(encstr)).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> -1 旧内容不对</returns>
        public ActionResult AddUpdate(LockPers p)
        {
            if (p.Id == "-1")
            { //添加的字段 Id,Name,Content,Prompt,InsertTime
                LockPers add = new LockPers(true);
                add.Id = Guid.NewGuid().ToString();
                add.Name = p.Name;
                add.Content = LockEncrypt.StringToMD5(p.Content);
                add.Prompt = p.Prompt;
                add.InsertTime = DateTime.Now;
                
                //string sql = "insert into LockPers(Id,Name,Content,Prompt,InsertTime) values(@Id,@Name,@Content,@Prompt,@InsertTime)";
                //efrwos = LockDapperUtilsqlite.New().Execute(sql, p);
                //using (IDbConnection conn = DataBaseConfig.GetSqliteConnection(DataBaseConfig.LockSqlLiteConnectionString))
                //{
                //    // efrwos = conn.Execute(sql, p);
                //}

                var efrwos = 0;
                efrwos = DapperFuncs .Insert<LockPers>(add);
                return Content(efrwos.ToString());
            }
            else
            { // 修改
                //var old =  LockDapperUtil<LockPers>.New.Get(p.Id);
                //var query = LockDapperUtilsqlite<LockPers>.Selec().Column().From().Where(lp => lp.Id == p.Id);
                //var old = query.ExecuteQuery<LockPers>().FirstOrDefault();
                //if (old.Content != LockEncrypt.StringToMD5(p.ContentOld))
                //{ // 旧内容不一致
                //    return Content("-1");  // 旧内容不一致
                //}

                var query = LockDapperUtilsqlite<LockPers>
                            .Selec().Column(c => new { c.Content,c.EditCount }).From().Where(m => m.Id == p.Id);
                //Tuple<StringBuilder, Dapper.DynamicParameters> ru = query.RawSqlParams();
                var edit = query.ExecuteQuery<LockPers>().FirstOrDefault();
                if (edit.Content != LockEncrypt.StringToMD5(p.ContentOld))  return Content("-1");  // 旧内容不一致

                edit._IsWriteFiled = true; // 标记开始记录赋值字段 注意上面查询LockPers 要再默认构造函数里把 标识改为false 查出的数据不要记录赋值字段 
                edit.Name = p.Name;
                edit.Content = LockEncrypt.StringToMD5(p.Content);
                edit.Prompt = p.Prompt;
                edit.UpdateTime = DateTime.Now;
                edit.EditCount = edit.EditCount + 1;
                var t = DapperFuncs .Update<LockPers>(edit, w => w.Id == p.Id );  
                
                //var t = LockDapperUtil<LockPers>.New.Update(old);
                return Content(Convert.ToInt32(t).ToString());
            }
            // 
        }

        public ActionResult Delete(string Id)
        {
            //var old = LockDapperUtilsqlite<LockPers>.Selec().Column().From().Where(lp => lp.Id == Id).ExecuteQuery<LockPers>().FirstOrDefault();
            //LockDapperUtil<LockPers>.New.Get(Id);
            //old.IsDel = true;
            //old.DelTime = DateTime.Now;

            var t = DapperFuncs .Update<LockPers>(
                del => {
                    del._IsWriteFiled = true; // 开启标记字段
                    del.IsDel = true;
                    del.DelTime = DateTime.Now;
                }, w => w.Id == Id ); //LockDapperUtil<LockPers>.New.Update(old);
            return Content(Convert.ToInt32(t).ToString());

            // del
            ////string sql = string.Format(" delete from LockPers where Id = @Id ");
            //string sql = string.Format(" update LockPers set IsDel = 1 where Id = @Id ");
            //var isSuccess = LockSQLitehelper.ExecuteNonQuery(sql, new SQLiteParameter("@Id", DbType.String) { Value = Id }) > 0;
            //return Content(Convert.ToInt32(isSuccess).ToString());
        }


        #region 19.1.5 修改为DapperSqlMaker
        public ActionResult Load(int page, int rows ,string serh)
        {
            serh = $"%{serh}%";
            int records;
            var where = PredicateBuilder.WhereStart<LockPers>();
            where = where.And( m => m.IsDel != true );
            if (!string.IsNullOrWhiteSpace(serh)) {
                where = where.And( m => (m.Name.Contains(serh) || m.Prompt.Contains(serh) ) );
            }
            var query = LockDapperUtilsqlite<LockPers>
                .Selec().Column(p => new { t = "datetime(a.InsertTime) as InsertTimestr"
                    , p.Id, p.Name, p.Content, p.Prompt, p.EditCount, p.CheckCount})
                .From().Where(where).Order(m => new { m.Name });
            var list = query.LoadPagelt(page, rows, out records);
            int total = (int)Math.Ceiling(records * 1.0 / rows); // 总页数
            var resultjson = JsonConvert.SerializeObject(new { data = list, rows = list, total= total, records = records});
            return Content(resultjson);
        }
        //Tuple<StringBuilder, Dapper.DynamicParameters> ru = query.RawSqlParams();
        //var list = query.ExecuteQuery<LockPers>();
        //Tuple<StringBuilder, DynamicParameters, StringBuilder> ru = query.RawLimitSqlParams();
        
            #endregion


        public ActionResult GetWhere(LockPers lpmodel, Users umodel) {


            //var Name =  lpmodel.Name; // "%蛋蛋%";
            //var IsDel = lpmodel.IsDel; //false;
            //var uName =  umodel.UserName; // "jiaojiao";

            var Name = Request.Form["Name"]; // "%蛋蛋%";
            var IsDel = bool.Parse(Request.Form["IsDel"]); //false;
            var uName = Request.QueryString["UserName"]; // "jiaojiao";

            //3
            //DapperSqlMaker<LockPers, Users> query = LockDapperUtilsqlite<LockPers, Users>
            ////2
            ////QueryMaker<LockPers, Users> query = new LockDapperUtilsqlite<LockPers, Users>();
            ////query
            ////1
            ////QueryMaker<LockPers, Users> query = LockDapperUtilsqlite<LockPers, Users>.New
            //    .Selec()
            //    .Column((lp, u) => null ) //new { lp.Id, lp.InsertTime, lp.EditCount, lp.IsDel, u.UserName }
            //    .FromJoin( JoinType.Left, (lpp, uu) => uu.Id == lpp.UserId)
            //    .Where((lpw, uw) => lpw.Name.Contains(Name) && lpw.IsDel == IsDel && uw.UserName == uName
            //        )  // 
            //    .Order((lp, w) => new { lp.EditCount, lp.Name })
            //    ; // .ExecuteQuery();
            //var resultsqlparams = query.RawSqlParams();
            ////var result = query.ExecuteQuery();

            return Content("");
        }


        
    }

}
