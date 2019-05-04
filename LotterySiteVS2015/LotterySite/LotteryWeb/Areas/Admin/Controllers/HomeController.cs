using Dapper;
using DapperSqlMaker.DapperExt;
using FW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LotteryWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            //string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");
            //List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);


            //var first = LockSqlite<Users, Skin>.Selec().Column((a, b) => new { Value = b.Value,img = a.img }).FromJoin(JoinType.Inner, (a, b) => a.SkinId == b.Id).Where( (a,b) => a.Id ==1 && a.UserName == "cc").ExecuteQuery<Users>().FirstOrDefault();

            //ViewData.Model = first;

            //var first = LockSqlite<Skin>.Selec().Column().From().Where(p => p.IsDel != 1 && p.IsEnabled == 1 && p.Type == "bg").ExecuteQuery<Skin>().FirstOrDefault();

            //if (first == null)
            //    ViewBag.url = "/Content/folio/images/header-image/jike_1_pic.gif";
            //else 
            //    ViewBag.url = first.Value;

            var query = LockSqlite<Users,Roles,Skin>.Selec()
                .Column((a,b,c) => new { x = SM.Sql("a.*"), rolename =  b.Name, bgurl = c.Value})
                .FromJoin(JoinType.Inner, (a,b,c) => a.RolesId == b.Id, JoinType.Inner, (a,b,c)=> a.SkinId == c.Id)
                .Where((p, b, c) => p.UserName == "cc" && p.Id == 1);
            Tuple<StringBuilder, DynamicParameters>  rawsql = query.RawSqlParams();
            var user = query.ExecuteQuery<Users>().FirstOrDefault();
            ViewData.Model = user;
            return View();
        }

        #region Lock ^_^
        public ActionResult Lock()
        {
            return View();
        }

        #endregion

        #region 主题设置

        public ActionResult Skin()
        {
            int UserId = 1;
            var user = LockSqlite<Users>.Selec().Column().From().Where(p => p.Id == UserId).ExecuteQuery<Users>().FirstOrDefault();
            ViewBag.img = user.img;
            return View();
        }

        public ActionResult GetSkin()
        {
            int UserId = 1;
            var list = LockSqlite<Skin>.Selec().Column().From().Where(p => p.IsDel != 1 && p.Type == "bg" && p.UserId == UserId).ExecuteQuery<Skin>();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        //string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");
        //List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
        //list.Add(new skinfo() { name = name, url = url, datetime = datetime });
        //System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));

        public ActionResult AddSkin(string name, string url)
        {
            var datetime = DateTime.Now.ToString("yy/MM/dd HH:mmss");
            int UserId = 1;


            //// 1 >
            //var insert = LockSqlite<Skin>.Inser().AddColumn(p => new bool[] {
            //    p.Name == name, p.Value == url, p.InsertDate == datetime
            //    , p.Type == "bg",p.Remake == "bg背景", p.UserId == UserId
            //});
            //var sqlparms = insert.RawSqlParams();
            //int efrows = insert.ExecuteInsert();

            // 2>
            var additem = new Skin(true) { Name = name, Value = url, InsertDate = datetime, Type = "bg", Remake = "bg背景", UserId = UserId };
            //
            try
            {
                int efrows = DapperFuncs.New.Insert<Skin>(additem);
                return Content(efrows > 0 ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public ActionResult RemoveSkin(int Id)
        {
            //string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            //List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    var item = list[i];
            //    if (name + datetime == item.name + item.datetime) list.RemoveAt(i--);
            //}


            //
            try
            {
                //System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                int UserIds = 1;
                //int Id = int.Parse( Request.Form["Id"]);
                int Id_ = Id;
                bool isSuccess = DapperFuncs.New.Update<Skin>(s => {
                    s._IsWriteFiled = true; s.IsDel = 1;
                }, w => w.Id == Id_ && w.UserId == UserIds);

                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public ActionResult SetSkin(int Id)
        {

            //List<Skin> skinlist = new List<Skin>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    list[i].type = "bg";
            //var item = list[i];
            //var nitem = new Skin(false) { Name = item.name, Value = item.url, Type = item.type, Remake = "bg背景" ,InsertDate = item.datetime};
            //skinlist.Add(nitem);
            //}
            //LockSqlite<Skin>.Cud.InserList(skinlist);

            //
            try
            {
                bool isSuccess = DapperFuncs.New.Update<Users>(s => {
                    s._IsWriteFiled = true; s.SkinId = Id;
                }, w => w.Id == 1 && w.UserName == "cc");

                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public ActionResult setuserimg(string img)
        {
            try
            {
                bool isSuccess = DapperFuncs.New.Update<Users>(s => {
                    s._IsWriteFiled = true; s.img = img;
                }, w => w.Id == 1 && w.UserName == "cc");
                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        #endregion

    }

}
