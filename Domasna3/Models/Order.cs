using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domasna3.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Local { get; set; }
        public List<FoodModel> FoodOrdered { get; set; }


    }
}