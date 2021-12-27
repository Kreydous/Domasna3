using Domasna3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Domasna3.Controllers
{
    public class OrderHistoryController : Controller
    {
        // GET: OrderHistory

        [Authorize]
        public ActionResult Index()
        {
            List<Order> orders = new List<Order>();
            List<Order> filteredOrders = new List<Order>();
            
            using (var db = new OrderContext())
            {
                 orders = db.Orders.ToList();
            }

            

            foreach (var order in orders)
            {
                if (order.UserName.Equals(HttpContext.User.Identity.Name))
                {
                    FoodModel food = new FoodModel();
                    List<FoodModel> filteredFood = new List<FoodModel>();
                    string k = order.ID.ToString();
                    using (SqlConnection connection = new SqlConnection("Server=tcp:domasna3dbserver.database.windows.net,1433;Initial Catalog=Domasna3_db;Persist Security Info=False;User ID=dizajntim;Password=Dizajnt7.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"))
                    {
                        connection.Open();
                        string query = "SELECT * FROM FoodModels WHERE FoodModels.Order_ID = " + k;
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    food.Id = reader.GetInt32(0);
                                    food.Name = reader.GetString(1);
                                    food.Checked = reader.GetBoolean(2);
                                    food.UserName = reader.GetString(3);

                                }
                                filteredFood.Add(food);
                            }
                            
                        }
                    }
                    order.FoodOrdered = filteredFood;
                    filteredOrders.Add(order);

                }
            }
            return View(filteredOrders);
        }
    }
}