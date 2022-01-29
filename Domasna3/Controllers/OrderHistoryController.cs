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
                    using (SqlConnection connection = new SqlConnection("Server = tcp:domasna3dbserver.database.windows.net,1433; Initial Catalog = Domasna3_db; Persist Security Info = False; User ID = dizajntim; Password =Dizajnt7.; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;"))
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

        public ActionResult ChangeStatusToDelivered(Order order)
        {
            int id = order.ID;
            using (var db = new OrderContext())
            {
                if(db.Orders.Where(e => e.ID == id).FirstOrDefault() != null)
                {
                    db.Orders.Where(e => e.ID == id).FirstOrDefault().orderStatus = Status.DELIVERED;
                   
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatusToCanceled(Order order)
        {
            int id = order.ID;
            using (var db = new OrderContext())
            {
                if (db.Orders.Where(e => e.ID == id).FirstOrDefault() != null)
                {
                    db.Orders.Where(e => e.ID == id).FirstOrDefault().orderStatus = Status.CANCELED;
                }


                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        

    }
}