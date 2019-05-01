using System;
using System.Collections.Generic;
using System.Linq;
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
            string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");
            List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
            if (list.Count == 0)
                ViewBag.url = "/Content/folio/images/header-image/jike_1_pic.gif";
            else 
                ViewBag.url = list.FirstOrDefault().url;


            return View();
        }


        public ActionResult Lock()
        {
            return View();
        }

        public ActionResult Skin()
        {
            return View();
        }

        public ActionResult AddSkin(string name, string url)
        {
            string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
            var datetime = DateTime.Now.ToString("yy/MM/dd HH:mmss");
            list.Add(new skinfo() { name = name, url = url, datetime = datetime });
            //
            try
            {
                System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public ActionResult RemoveSkin(string name, string datetime)
        {
            string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (name + datetime == item.name + item.datetime) list.RemoveAt(i--);
            }
            //
            try
            {
                System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public ActionResult SetSkin(string name, string datetime, string url) {

            string skininfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/skininfo.js")).Replace("var skininfoarr = ", "");

            List<skinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<skinfo>>(skininfoarr);

            int selIndex = list.FindIndex(p => p.name == name && p.datetime == datetime);
            skinfo temp = list[selIndex];
            list[selIndex] = list[0];
            list[0] = temp;
             
            //
            try
            {
                System.IO.File.WriteAllText(Request.MapPath("/Content/skininfo.js"), "var skininfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content("1");
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
    }
}
