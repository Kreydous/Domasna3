using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Domasna3.Models;
using Newtonsoft.Json;

namespace Domasna3.Controllers
{
    public class LokaciiController : Controller
    {
        private lokaciiEntities2 db = new lokaciiEntities2();
        
        public ActionResult Index()
        {
            //Choose city
            return View();

        }

        public ActionResult ChooseLocal()
        {
            List<Map> lokaciiList = new List<Map>();
            var podatoci = db.mapas.ToList();
            foreach (var item in podatoci)
            {
                lokaciiList.Add(new Map(item.name, item.lat, item.lon, item.city));
            }

            ViewBag.markerData = lokaciiList;

            return View(lokaciiList);
        }
       
    }
}