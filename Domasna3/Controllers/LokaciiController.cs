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
        static Order order = new Order();
        List<string> orderedFood = new List<string>();
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
            var list = new List<FoodModel>
            {
                 new FoodModel{Id = 1, Name = "Hamburger", Checked = false},
                 new FoodModel{Id = 2, Name = "Pizza", Checked = false},
                 new FoodModel{Id = 3, Name = "Salad", Checked = false},
                 new FoodModel{Id = 4, Name = "Juice", Checked = false},
                 new FoodModel{Id = 5, Name = "Cupcake", Checked = false},

            };
            return View(list);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Order(List<FoodModel> checkBoxList)
        {
            List<FoodModel> orderedFood = new List<FoodModel>();
            foreach(var item in checkBoxList)
            {
                if (item.Checked)
                {
                    if(item.Id == 1)
                    {
                        item.Name = "Hamburger";
                    }else if(item.Id == 2)
                    {
                        item.Name = "Pizza";
                    }else if(item.Id == 3)
                    {
                        item.Name = "Salad";
                    }else if(item.Id == 4)
                    {
                        item.Name = "Juice";
                    }
                    else
                    {
                        item.Name = "Cupcake";
                    }
                    orderedFood.Add(item);
                }
            }
            if(orderedFood.Count == 0)
            {
                // nema selektirano nisto
            }
            order.FoodOrdered = orderedFood;
            return View();
        }
    }
}