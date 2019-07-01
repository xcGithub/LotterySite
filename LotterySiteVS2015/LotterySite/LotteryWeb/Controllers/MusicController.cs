using FW.Model;
using DapperSqlMaker.DapperExt;
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

        public ActionResult Get(string page)
        {
            var data = LockSqlite<Skin>.Selec().Column().From().Where(p => p.Type == page).Order(p=> new { p.Seq }).ExecuteQuery<Skin>().ToList<Skin>();
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return Content(result);

        }

        public ActionResult Set(string name, string url, string type, string page)
        {
            // Take Me To Infinity
            //http://fdfs.xmcdn.com/group43/M00/9C/09/wKgKjVsvLhizbsJeABlc7rqvwsU325.mp3

             var seq = LockSqlite<Skin>.Selec().Column(p => new { x = " count(1) counts" }).From().Where(p=> p.Type == page).ExecuteScalar<int>();
             Skin skin = new Skin(true) {
                  Name = name // "Balfe - Terminated (《终结者：创世纪》电影插曲)"
                , Value = url // "http://fdfs.xmcdn.com/group45/M07/66/6A/wKgKlFs9nArA5ryhAA6LBGufq7o053.mp3"
                , Type = page //"pbgm"
                , Value2 = type // "ximalaya"
                , Remake = "相册背景音"
                , UserId = 1
                , InsertDate = DateTime.Now.ToString("yy/MM/dd HH:mmss")
                , Seq = seq//0
                , IsDel = 0
                };
            try
            {
               var efrow =  DapperFuncs.New.Insert<Skin>(skin);
               return Content(efrow > 0 ? "1" : "0");
            }
            catch (Exception ex)
            {
                throw;
            }

            // ------------------
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

        public ActionResult Sub(string songarr, string page)
        {

            var nametypearr = songarr.TrimEnd(',').Split(',');

            var del = LockSqlite<Skin>.Delet().Where(p => p.Type == page && SM.In(p.Id, nametypearr) ).ExecuteDelete();
            return Content(del > 0 ? "1" : "0");

            // ------------------
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

        public ActionResult editseq(int id, string page, int seq, int seqn) {

            var where = PredicateBuilder.WhereStart<Skin>();
            string seqsql = null;
            if (seq < seqn) { // 移到大序号
                seqsql = " seq - 1 ";
                where = where.And( p => p.Seq > seq && p.Seq <= seqn);
            }
            else if(seq > seqn){
                seqsql = " seq + 1 ";
                where = where.And(p => p.Seq >= seqn && p.Seq < seq );
            }
            where = where.And(p => p.Type == page);
            //var a =  LockSqlite<Skin>.Updat().EditColumn(p => new bool[] { SM.Sql(p.Seq, seqsql) } ).From().Where(where).RawSqlParams();
            var efrow = LockSqlite<Skin>.Updat().EditColumn(p => new bool[] { SM.Sql(p.Seq, seqsql) }).Where(where).ExecuteUpdate();
            var efrow2 = LockSqlite<Skin>.Updat().EditColumn(p => new bool[] { p.Seq == seqn }).Where( p=> p.Id == id).ExecuteUpdate();

            return Content(efrow > 0 && efrow2 > 0 ? "1" : "0");
            //return Content(del > 0 ? "1" : "0");
        }

        public ActionResult test(string page) {
            
            //Skin skin = new Skin() { Name = "Balfe - Terminated (《终结者：创世纪》电影插曲)"
            //, Value = "http://fdfs.xmcdn.com/group45/M07/66/6A/wKgKlFs9nArA5ryhAA6LBGufq7o053.mp3"
            //, Type = "hbgm"
            //, Value2 = "ximalaya"
            //, Remake = "主页背景音"
            //, UserId = 1
            //, InsertDate = DateTime.Now.ToString("yy/MM/dd HH:mmss")
            //, Seq = 0
            //};

            string songinfoarr = System.IO.File.ReadAllText(Request.MapPath("/Content/songinfo.js")).Replace("var songinfoarr = ", "");
            List<songinfo> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<songinfo>>(songinfoarr);
            for (int i = 0; i < list.Count; i++)
            {
                var seq = LockSqlite<Skin>.Selec().Column(p => new { x = " count(1) counts" }).From().Where(p => p.Type == page).ExecuteScalar<int>();

                Skin skin = new Skin(true) {
                Name = list[i].name // "Balfe - Terminated (《终结者：创世纪》电影插曲)"
                , Value = list[i].url // "http://fdfs.xmcdn.com/group45/M07/66/6A/wKgKlFs9nArA5ryhAA6LBGufq7o053.mp3"
                , Type = page //"pbgm"
                , Value2 = list[i].type // "ximalaya"
                , Remake = "相册背景音"
                , UserId = 1
                , InsertDate = DateTime.Now.ToString("yy/MM/dd HH:mmss")
                , Seq = seq //0
                , IsDel = 0
                };
                try
                {
                    DapperFuncs.New.Insert<Skin>(skin);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            //LockDapperUtilmssql

            return Content("xxxx");

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
