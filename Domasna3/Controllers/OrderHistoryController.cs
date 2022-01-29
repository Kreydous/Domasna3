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
                    using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Domasna3.Models.OrderContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {
                        connection.Open();
                        string query = "SELECT * FROM FoodModels WHERE Order_ID in (" + k +")";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    FoodModel food1 = new FoodModel
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Checked = reader.GetBoolean(2),
                                        UserName = reader.GetString(3)
                                    };
                                    order.FoodOrdered.Add(food1);
                                }

                            }
                            
                        }
                    }
                    filteredOrders.Add(order);

                }
            }
            return View(filteredOrders);
        }

        public ActionResult ChangeStatusToDelivered(string order)
        {
            var id1 = (string)(RouteData.Values["id"]);
            int id = Int32.Parse(id1);
            using (var db = new OrderContext())
            {
                
                    var o = db.Orders.SingleOrDefault(e => e.ID == id);
                    o.orderStatus = Status.DELIVERED;
                
                
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatusToCanceled(Order order)
        {
            var id1 = (string)(RouteData.Values["id"]);
            int id = Int32.Parse(id1);
            using (var db = new OrderContext())
            {

                var o = db.Orders.SingleOrDefault(e => e.ID == id);
                o.orderStatus = Status.CANCELED;


                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        

    }
}