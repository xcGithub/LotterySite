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
    public class LOLController : Controller
    {
        //
        // GET: /LOL/

        public ActionResult Index()
        {
            var isimpt = "";
            if (Request.QueryString["flag"] != "1") isimpt = "mui-hidden";
            ViewBag.impthid = isimpt;
            return View();
        }

        public ActionResult Search(string serh) {
            string[] names = serh.Replace("\t","").Replace(" ","").Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var where = PredicateBuilder.WhereStart<LOLkengbi>();
            for (int i = 0; i < names.Length; i++)
            {
                //var cname = names[i].Replace("加入了队伍聊天", "");
                //where = where.Or(p => p.Name == cname);
                names[i] = names[i].Replace("加入了队伍聊天", "");
            }
            where = where.And(p => SM.In(p.Name, names));
            var query = LockDapperUtilsqlite<LOLkengbi>.Selec().Column().From().Where(where);
            Tuple<StringBuilder, DynamicParameters> rawsqlparas = query.RawSqlParams();
            var list = query.ExcuteSelect<LOLkengbi>();
            var result = JsonConvert.SerializeObject(new { data = list, state = list.Count() > 0 ? 1 : 0 });
            return Content(result);
        }

        public ActionResult Import(string impt, string serv) {
            string[] rows = impt.Trim(' ').Trim('\n').Replace("\t","").Split( new string[1]{ "\n" },StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            List<LOLkengbi> list = new List<LOLkengbi>();
            foreach (string item in rows)
            {
                string[] itemarr = item.Split(new string[1] { " "},StringSplitOptions.RemoveEmptyEntries);
                LOLkengbi lm = new LOLkengbi(false);
                lm.Name = itemarr[0];
                lm.Hero = itemarr.Length > 1 ? itemarr[1] : "";
                lm.Remake = itemarr.Length > 2 ? itemarr[2] : "";
                lm.UserId = 1;
                lm.Server = string.IsNullOrWhiteSpace(serv) ? "扭曲丛林" : serv;
                list.Add(lm);
            }
            int efrows = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var where = PredicateBuilder.WhereStart<LOLkengbi>();
                where = where.And(p => p.Name == item.Name);
                if (!string.IsNullOrWhiteSpace(item.Hero))
                {
                    where = where.And(p => p.Hero == item.Hero);
                }
                if (!string.IsNullOrWhiteSpace(item.Server))
                {
                    where = where.And(p => p.Server == item.Server);
                }
                var query = LockDapperUtilsqlite<LOLkengbi>.Selec()
                    .Column(p => new { a = SM.Sql(" count(1) as counts ") }).From().Where(where);
                var lcounts = query.ExcuteSelect<int>().FirstOrDefault();
                if (lcounts > 0)
                {
                    sb.AppendLine($"{item.Name}  {item.Hero}  {item.Server} x");
                    continue;
                }
                var efrw = LockDapperUtilsqlite<LOLkengbi>.Cud.Inser(item, false);
                if (efrw > 0)
                {
                    sb.AppendLine($"{item.Name}  {item.Hero}  {item.Server} √");
                    efrows++;
                }
            }
            sb.Insert(0,$"成功√标记了{efrows}个 x是已被他人标记过的 (总上传{list.Count()})\n");
            var state = efrows > 0 ? 1 : 0;
            var result = JsonConvert.SerializeObject( new { state = state, msg = sb.ToString() } );
            return Content(result);
        }

    }
}
