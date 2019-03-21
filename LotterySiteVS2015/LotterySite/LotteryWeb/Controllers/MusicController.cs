using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using xxoo.Common;

namespace LotteryWeb.Controllers
{
    public class MusicController : Controller
    {
        //
        // GET: /Music/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string name, string type, string url)
        {
            //.net 4.0 设置： 
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            //1------ -
            HttpModel hm = new HttpModel();
            hm.Method = HttpMethod.Post;
            hm.Url = url == null ? "https://music.liuzhijin.cn/" : url;
            hm.Params = new List<HttpParams>();
            hm.Params.Add(new HttpParams() { Name = "input", Value = name });
            hm.Params.Add(new HttpParams() { Name = "filter", Value = "name" });
            hm.Params.Add(new HttpParams() { Name = "type", Value = type });
            hm.Params.Add(new HttpParams() { Name = "page", Value = "1" });
            hm.Heads = new List<HttpParams>();
            hm.Heads.Add(new HttpParams() { Name = "x-requested-with", Value = "XMLHttpRequest" });
            hm.Heads.Add(new HttpParams() { Name = "Cache-Control", Value = "no-cache" });
            //hm.Heads.Add(new HttpParams() { Name = "Content-Type", Value = "application/x-www-form-urlencoded" });
            //string response = HttpHelper.SendRequestFormGetText(hm
            string response = HttpHelper.SendRequestGetText(hm);


            return Content(response);
        }

        public ActionResult Set(string name, string url, string type)
        {
            // Take Me To Infinity
            //http://fdfs.xmcdn.com/group43/M00/9C/09/wKgKjVsvLhizbsJeABlc7rqvwsU325.mp3

            string songinfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/songinfo.js")).Replace("var songinfoarr = ", "");

            List<songinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<songinfo>>(songinfoarr);
            list.Add(new songinfo() { name = name, url = url, type = type, datetime = DateTime.Now.ToString("yyMMdd-HHmmss") });
            //
            try
            {
                System.IO.File.WriteAllText(Request.MapPath("/Content/songinfo.js"), "var songinfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public ActionResult Sub(string songarr)
        {

            var nametypearr = songarr.TrimEnd(',').Split(',');
            string songinfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/songinfo.js")).Replace("var songinfoarr = ", "");
            List<songinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<songinfo>>(songinfoarr);
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (nametypearr.Contains(item.name + item.type)) list.RemoveAt(i--);
            }

            if (list.Count < 2)
            {
                list.Add(new songinfo() { datetime = DateTime.Now.ToString("yyMMdd-HHmmss:sss") , name = "模仿翻唱 - Take Me to Infinity", url = "http://fdfs.xmcdn.com/group43/M00/9C/09/wKgKjVsvLhizbsJeABlc7rqvwsU325.mp3", type = "ximalaya" });
                list.Add(new songinfo() { datetime = DateTime.Now.ToString("yyMMdd-HHmmss") , name = "Cruisin", url = "http://fdfs.xmcdn.com/group23/M02/66/C6/wKgJL1g3voDgDRtvABzdzUcI_HU595.mp3", type = "ximalaya" });
            }
            try
            {
                System.IO.File.WriteAllText(Request.MapPath("/Content/songinfo.js"), "var songinfoarr = " + Newtonsoft.Json.JsonConvert.SerializeObject(list));
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

    }

    public class songinfo
    {
        public string name { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string datetime { get; set; }
    }
}
