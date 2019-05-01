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


            //var first = LockDapperUtilsqlite<Users, Skin>.Selec().Column((a, b) => new { Value = b.Value,img = a.img }).FromJoin(JoinType.Inner, (a, b) => a.SkinId == b.Id).Where( (a,b) => a.Id ==1 && a.UserName == "cc").ExcuteSelect<Users>().FirstOrDefault();

            //ViewData.Model = first;

            //var first = LockDapperUtilsqlite<Skin>.Selec().Column().From().Where(p => p.IsDel != 1 && p.IsEnabled == 1 && p.Type == "bg").ExcuteSelect<Skin>().FirstOrDefault();

            //if (first == null)
            //    ViewBag.url = "/Content/folio/images/header-image/jike_1_pic.gif";
            //else 
            //    ViewBag.url = first.Value;

            var query = LockDapperUtilsqlite<Users,Roles,Skin>.Selec()
                .Column((a,b,c) => new { x = SM.Sql("a.*"), rolename =  b.Name, bgurl = c.Value})
                .FromJoin(JoinType.Inner, (a,b,c) => a.RolesId == b.Id, JoinType.Inner, (a,b,c)=> a.SkinId == c.Id)
                .Where((p, b, c) => p.UserName == "cc" && p.Id == 1);
            Tuple<StringBuilder, DynamicParameters>  rawsql = query.RawSqlParams();
            var user = query.ExcuteSelect<Users>().FirstOrDefault();
            ViewData.Model = user;
            return View();
        }


        public ActionResult Lock()
        {
            return View();
        }

        public ActionResult Skin()
        {
            int UserId = 1;
            var user = LockDapperUtilsqlite<Users>.Selec().Column().From().Where(p => p.Id == UserId).ExcuteSelect<Users>().FirstOrDefault();
            ViewBag.img = user.img;
            return View();
        }

        public ActionResult GetSkin() {
            int UserId = 1;
           var list = LockDapperUtilsqlite<Skin>.Selec().Column().From().Where(p=> p.IsDel != 1 && p.Type == "bg" && p.UserId == UserId).ExcuteSelect<Skin>();
           return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        public ActionResult AddSkin(string name, string url)
        {
            //string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            //List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
            var datetime = DateTime.Now.ToString("yy/MM/dd HH:mmss");
            //list.Add(new skinfo() { name = name, url = url, datetime = datetime });
            int UserId = 1;
            var additem = new Skin(true) { Name = name, Value = url, InsertDate = datetime, Type="bg", Remake="bg背景",UserId = UserId };
            //
            try
            {
                int efrows = LockDapperUtilsqlite<Skin>.Cud.Insert(additem);
                //System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
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
                bool isSuccess = LockDapperUtilsqlite<Skin>.Cud.Update(s => {
                    s._IsWriteFiled = true; s.IsDel = 1;  },  w => w.Id == Id && w.UserId == 1);

                return Content(isSuccess ?"1": "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public ActionResult SetSkin(int Id) {



            //string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            //List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);

            //int selIndex = list.FindIndex(p => p.name == name && p.datetime == datetime);
            //skinfo temp = list[selIndex];
            //list[selIndex] = list[0];
            //list[0] = temp;



            //List<Skin> skinlist = new List<Skin>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    list[i].type = "bg";
                //var item = list[i];
                //var nitem = new Skin(false) { Name = item.name, Value = item.url, Type = item.type, Remake = "bg背景" ,InsertDate = item.datetime};
                //skinlist.Add(nitem);
            //}

            //LockDapperUtilsqlite<Skin>.Cud.InserList(skinlist);

            
            //
            try
            {
                bool isSuccess = LockDapperUtilsqlite<Users>.Cud.Update(s => {
                    s._IsWriteFiled = true; s.SkinId = Id;  }, w => w.Id == 1 && w.UserName == "cc" );

                //   var setobj = LockDapperUtilsqlite<Skin>.Selec().Column().From().Where(p => p.Id == Id).ExcuteSelect<Skin>().FirstOrDefault();
                //  setobj.IsEnabled = 1;
                //bool isSuccess = LockDapperUtilsqlite<Skin>.Cud.Updat(setobj);
                //System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            } 
        }


        public ActionResult setuserimg(string img) {
            try
            {
                bool isSuccess = LockDapperUtilsqlite<Users>.Cud.Update(s => {
                    s._IsWriteFiled = true; s.img = img;
                }, w => w.Id == 1 && w.UserName == "cc");
                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }


    public class skinfo
    {
        public string name { get; set; }
        public string url { get; set; }
        public string datetime { get; set; }
        /// <summary>
        /// hd/bg
        /// </summary>
        public string type { get; set; }
    }
}
