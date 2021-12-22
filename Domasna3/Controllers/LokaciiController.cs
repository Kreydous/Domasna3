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
        Order order = new Order();

        [Authorize]
        public ActionResult Index()
        {
            //Choose city
            List<string> city = new List<String>();
            var podatoci = db.mapas.ToList();
            foreach (var item in podatoci)
            {
                city.Add(item.city);
            }
            List<string> uniqueLst = city.Distinct().ToList();

            return View(uniqueLst);

        }

        [Authorize]
        [HttpPost]
        public ActionResult ChooseLocal(string form)
        {
            
            var cityName = Request.Form["cityName"];
            order.City = cityName;
            List<string> locals = new List<string>();
            var podatoci = db.mapas.ToList();
            foreach (var item in podatoci)
            {
                string city = item.city;
                if (city == null)
                {
                    city = "test";
                }
                if (city.Equals(cityName))
                {
                    locals.Add(item.name);
                }
            }
            List<string> uniqueLst = locals.Distinct().ToList();
            //var number = Request.Form["cityName"];
            return View(uniqueLst);
        }


        [Authorize]
        public ActionResult ChooseFood()
        {
            var local = Request.Form["localName"];
            order.Local = local;
            List<string> menu = new List<string>();
            menu.Add("Hamburger");
            menu.Add("Pizza");
            menu.Add("Pasta");
            return View(menu);
        }


        [Authorize]
        public ActionResult Order()
        {
            var food = Request.Form["food"];
            order.Food = food;
            return View();
        }
    }
}