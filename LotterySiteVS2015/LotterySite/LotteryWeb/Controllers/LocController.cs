using Dapper;
using DapperSqlMaker.DapperExt;
using FW.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LotteryWeb.Controllers
{
    public class LocController : Controller
    {
        //
        // GET: /Loc/

        public ActionResult Index()
        {
            return View();
        }

       




        public void delinsert()
        {


            // 2. Update (跟新部分字段 set和where里不能有相同字段)
            LockPers pset = new LockPers(true);
            pset.Name = "修改95 只修改Name字段";
            LockPers pwhere = new LockPers(true);
            pwhere.Content = "7fa867c5b404547797614abe57341844";
            //var efrwostest2 = LockDapperUtil.Update<LockPers>(pset, pwhere);

            // 4. Delete 
            LockPers pdel = new LockPers(true);
            pdel.Id = "1339a621-09f5-4d44-8653-09354013c3c0";
            pdel.IsDel = true;
            //var efrowsdel = LockDapperUtil.Delete(pdel);

            // 6. Update
            //var efrowsupdate2 = LockDapperUtil<LockPers>.Update(
            //set =>
            //{
            //    set.Name = "修改95 修改Name和Content字段";
            //    set.Prompt = "BMWWWWWWWWWWWWWWW";
            //}
            //, where => where.Content = "xxxxxxxoooooooo");

            // 7. Delete
            //var efrowsdelete = LockDapperUtil<LockPers>.Delete(where =>
            //{
            //    where.Id = "3e478c04-5e5b-41a8-af7a-5185cc141618";
            //    where.IsDel = "1";
            //});

        }

        public void insert()
        {
            // 1. Add
            //LockPers padd = new LockPers(true);
            //padd.Id = Guid.NewGuid().ToString();
            //padd.InsertTime = DateTime.Now;
            //padd.IsDel = "1";
            //padd.Content = "xxxxxxxoooooooo";
            //padd.Name = "913 添加数据";
            //var efrowsadd = LockDapperUtil.Insert<LockPers>(padd);

            // 3. Update Amobject  批量插入/修改/删除 ... 
            // string sqlamobject = "update LockPers set IsDel = @IsDelN where IsDel = @IsDel ";
            // var efrows = LockDapperUtil.Execute(sqlamobject, new { IsDelN = "0", IsDel = "False" });

            // -----------------------------------------------------------------------------
            // 5.Add
            //var efrowsadd2 = LockDapperUtil<LockPers>.Insert(
            //filed =>
            //{ 
            //    filed.Id = Guid.NewGuid().ToString();
            //    filed.InsertTime = DateTime.Now;
            //    filed.IsDel = "1";
            //    filed.Content = "xxxxxxxoooooooo";
            //    filed.Name = "913 添加数据";
            //});

            // 8. In
            //string sql = "SELECT * FROM SomeTable WHERE id IN @ids"
            //var results = conn.Query(sql, new { ids = new[] { 1, 2, 3, 4, 5 });

            // 8. Update  条件使用表达式 Like, In, Or ??  表达式转sql  区分表达式和赋值字段
            //var efrowsupdateex = LockDapperUtil<LockPers>.Update(
            //set =>
            //{
            //    set.Name = "修改913 修改Name和Content字段 模糊匹配xxxx";
            //    set.Prompt = "BMWWWWWWWWWWWWWWW";
            //    set.UpdateTime = DateTime.Now;   
            //    set.IsDel = "1";
            //}
            //, where => SQLMethods.DB_Like(where.Content, "%xxoo%") && where.IsDel == "1");

            // 9. select 表达式 
            //var test = LockDapperUtil<LockPers>.New.Get(w => SM.Like(w.Name, "%Steam%") && w.IsDel == true);

            // 10. delete 表达式 
            // var delresult = LockDapperUtil<LockPers>.Delete(w => SQLMethods.DB_Like(w.Content, "%xxoo%") && w.IsDel == "1");

            // 11. dynamic 
            //var test2 = LockDapperUtil.Query("SELECT * FROM LockPers where isDel != 1  order by Name ", null);

            // 12. order by 排序



            // --------------------------------------------------------------------------------------

            //DynamicParameters mapped = new DynamicParameters();
            //// mapped.AddParameters(cmd, identity);
            //mapped.Add("@isDel", 1);
            //var efrows = LockDapperUtil.Execute(sqlSelect, mapped);


            //LockDapperUtil.Get<LockPers>("xxxxxxxoooooooo");


            // --------------------------------------------------------------------------------------


            // --------------------------------------------------------------------------------------
            //var name = "xxxo";
            //var key = new { Name = "test" };
            //string sql = $"select * from {name} where {key.Name} = @id";

            //var customers = new List<LockPers>();

            //// 1
            //IEnumerable<LockPers> customerQuery = from cust in customers
            //                                      where cust.Content == "London"
            //                                      select cust;

            //// 2
            //IEnumerable<LockPers> customerQuery2 = customers.Where(p => p.Content == "London");

            //ExpressionType
            // var a = new System.Linq.Expressions.Expression();


            //Expression<Func<LockPers, bool>> expression = t => SM.In(t.Name, new string[] { "马", "码" }) 
            //   && t.Name == "农码一生" && t.Prompt == "男" || t.Name.Contains("11") ;
            //StringBuilder sb = null;
            //DynamicParameters spars = null;
            //AnalysisExpression.VisitExpression(expression, ref sb, ref spars);


        }


    }
}
