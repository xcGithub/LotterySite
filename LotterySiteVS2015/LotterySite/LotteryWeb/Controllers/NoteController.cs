using DapperSqlMaker.DapperExt;
using FW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LotteryWeb.Controllers
{
    public class NoteController : Controller
    {
        //
        // GET: /Note/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNotes() {
            int UserId = 1;
            var where = PredicateBuilder.WhereStart<SynNote>();
            where = where.And(p => p.IsDel != 1 && p.UserId == UserId);
            

            var list = LockSqlite<SynNote>.Selec().Column(p => new { p.Id,p.Name,p.Content, x = SM.Sql("substr(notedate,0,17) as NoteDate") }).From().Where(where).Order(p=> new {p.NoteDate },true).ExecuteQuery<SynNote>();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        public ActionResult AddNote(string name,string content,int id) {
            var datetime = DateTime.Now.ToString("yy-MM-dd HH:mm");
            int UserId = 1;
            // 2>
            var additem = new SynNote(true) { Name = name, Content = content, NoteDate = datetime
                ,UserId = UserId };
            //
            try
            {
                int efrows = 0;
                if(id == -1)
                    efrows = DapperFuncs.New.Insert<SynNote>(additem);
                else
                    efrows = Convert.ToInt32( DapperFuncs.New.Update<SynNote>(additem, p => p.Id == id) );

                return Content(efrows > 0 ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public ActionResult RemoveNote(int id) {
            try
            { 
                int UserIds = 1;
                //int Id = int.Parse( Request.Form["Id"]);
                int Id_ = id;
                bool isSuccess = DapperFuncs.New.Update<SynNote>(s => {
                    s._IsWriteFiled = true; s.IsDel = 1;
                }, w => w.Id == Id_ && w.UserId == UserIds);

                return Content(isSuccess ? "1" : "0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
