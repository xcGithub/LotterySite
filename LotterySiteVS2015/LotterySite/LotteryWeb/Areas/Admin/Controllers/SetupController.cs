using DapperSqlMaker.DapperExt;
using FW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace LotteryWeb.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        //
        // GET: /Admin/Setup/
        #region 菜单管理
        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult GetMenu()
        {
            //int UserId = 1;
            var where = PredicateBuilder.WhereStart<Menu>();
            where = where.And(p =>  p.IsDel != 1 && p.Level != -1); 
            var list = LockSqlite<Menu>.Selec().Column().From().Where(where)
                .ExecuteQuery<Menu>();

            ////排序字段
            //string[] property = new string[] { "Id", "ParentId" };
            ////对应排序字段的排序方式
            //bool[] sort = new bool[] { true, true };

            ////对 List 排序
            //list = new IListSort<Menu>(list.ToList(), property, sort).Sort();


            var pags = new
            {
                rows = list,
                total = 20,
                page = 1
            };

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(pags));
        }

        [ValidateInput(false)]
        public ActionResult SaveMenu(Menu m) {

            if (m.seq == null) // 空值自动计算排序号
                m.seq = LockSqlite<Menu>.Selec().Column(p => new { x = SM.Sql("count(1) counts") }).From().Where(p => p.ParentId == m.ParentId && p.IsDel != 1).ExecuteScalar<int>() + 1;
            if (m.href == null) m.href = "#"; // 默认值

            Menu add = new FW.Model.Menu(true);
            add.ParentId = m.ParentId;
            add.Title = m.Title;
            add.Level = m.Level; //js选中级别的时候传过来
            add.seq = m.seq;
            add.href = m.href;
            add.img = m.img;
            add.Target = m.Target;
            add.TitleTag = m.TitleTag;

            if (m.Id == -1)
            { //添加的字段 Id,Name,Content,Prompt,InsertTime

                var efrwos = DapperFuncs.New.Insert<Menu>(add);
                return Content(efrwos.ToString());
            }
            else
            { // 修改

                var efrwos = DapperFuncs.New.Update<Menu>(add, w => w.Id == m.Id);
                return Content(Convert.ToInt32(efrwos).ToString());
            }

        }

        public ActionResult DelMenu(int Id)
        {
            var t = DapperFuncs.New.Update<Menu>(
                del => {
                    del.SetWriteFiled(); // 开启标记字段
                    del.IsDel = 1;
                }, w => w.Id == Id); //LockDapperUtil<LockPers>.New.Update(old);
            return Content(Convert.ToInt32(t).ToString());

            // del
            ////string sql = string.Format(" delete from LockPers where Id = @Id ");
            //string sql = string.Format(" update LockPers set IsDel = 1 where Id = @Id ");
            //var isSuccess = LockSQLitehelper.ExecuteNonQuery(sql, new SQLiteParameter("@Id", DbType.String) { Value = Id }) > 0;
            //return Content(Convert.ToInt32(isSuccess).ToString());
        }

        public ActionResult GetComboMenu() {

            var list = LockSqlite<Menu>.Selec().Column(p => new { p.Id, p.ParentId, p.Title, p.Level }).From()
                .Where(p => p.IsDel != 1).Order(p => new { p.Lead,p.seq })
                .ExecuteQuery<Menu>();

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));

        }

        #endregion


        #region 字典管理

        public ActionResult Dic() {
            return View();
        }

        public ActionResult GetComboDic() {
            var list = LockSqlite<LKDataItem_>.Selec().Column(p => new { p.F_ItemId, p.F_ParentId, p.F_ItemName, p.F_Level })
                .From().Order(p => new { p.F_SortCode }).ExecuteQuery<LKDataItem_>();

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        public ActionResult GetDicDetail(string id) {

            var list = LockSqlite<LKDataItemDetail_>.Selec()
                .Column( p=> new { p.F_ItemDetailId, p.F_ItemId,p.F_ItemName,p.F_ItemValue, p.F_ItemCode, p.F_SortCode,p.F_Description })
                .From().Where(p => p.F_ItemId == id).Order(p=> new { p.F_SortCode }).ExecuteQuery<LKDataItemDetail_>();

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        #endregion
    }
     

}
