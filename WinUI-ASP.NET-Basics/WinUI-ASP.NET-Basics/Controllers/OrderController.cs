using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET: api/<OrderController>
        [HttpGet("GetAllOrders")]
        public ActionResult GetAllOrders()
        {
            try
            {
                using (AppDbContext db = new())
                {
                    List<Order> orders = db.Orders
                        .Include(o => o.Pizzas)
                        .ThenInclude(p => p.PizzaToppings)
                        .ThenInclude(pt => pt.Topping)
                        .ToList();

                    if (orders.Count == 0)
                    {
                        return NotFound();
                    }

                    var ordersWithPizzas = orders.Select(order => new
                    {
                        order.Id,
                        order.UserId,
                        order.StatusId,
                        order.OrderedAt,
                        Pizzas = order.Pizzas.Select(pizza => new
                        {
                            pizza.Id,
                            pizza.Name,
                            pizza.Price,
                            pizza.ImgUrl,
                            Toppings = pizza.PizzaToppings.Select(pt => new { pt.Topping.Id, pt.Topping.Name }).ToList()
                        }).ToList()
                    }).ToList();

                    return Ok(ordersWithPizzas);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<OrderController>/5
        [HttpGet("GetSpecificOrder")]
        public ActionResult GetSpecificOrder(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Order? order = db.Orders
                        .Where(o => o.Id == id)
                        .Include(o => o.Pizzas)
                        .ThenInclude(p => p.PizzaToppings)
                        .ThenInclude(pt => pt.Topping)
                        .FirstOrDefault();

                    if (order == null)
                    {
                        return NotFound();
                    }

                    var orderWithPizzas = new
                    {
                        order.Id,
                        order.UserId,
                        order.StatusId,
                        order.OrderedAt,
                        Pizzas = order.Pizzas.Select(pizza => new
                        {
                            pizza.Id,
                            pizza.Name,
                            pizza.Price,
                            pizza.ImgUrl,
                            Toppings = pizza.PizzaToppings.Select(pt => new { pt.Topping.Id, pt.Topping.Name }).ToList()
                        }).ToList()
                    };

                    return Ok(orderWithPizzas);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<OrderController>
        [HttpPost("CreateOrder")]
        public ActionResult CreateOrder(Order order)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    if (order == null)
                    {
                        return BadRequest("Order is null");
                    }

                    // Add the order to the database
                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Add the pizzas to the order
                    foreach (var pizza in order.Pizzas)
                    {
                        var existingPizza = db.Pizzas.Find(pizza.Id);
                        if (existingPizza != null)
                        {
                            order.Pizzas.Add(existingPizza);
                        }
                    }
                    db.SaveChanges();

                    return Ok(order);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<OrderController>/EditOrder
        [HttpPut("EditOrder")]
        public ActionResult EditOrder(Order order)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    var existingOrder = db.Orders
                        .Include(o => o.Pizzas)
                        .FirstOrDefault(o => o.Id == order.Id);

                    if (existingOrder == null)
                    {
                        return NotFound();
                    }

                    // Update order details
                    existingOrder.UserId = order.UserId;
                    existingOrder.StatusId = order.StatusId;
                    existingOrder.OrderedAt = order.OrderedAt;

                    // Get existing pizzas
                    var existingPizzas = existingOrder.Pizzas.ToList();

                    // Remove pizzas that are no longer in the new order
                    foreach (var existingPizza in existingPizzas)
                    {
                        if (!order.Pizzas.Any(p => p.Id == existingPizza.Id))
                        {
                            existingOrder.Pizzas.Remove(existingPizza);
                        }
                    }

                    // Add new pizzas that are not in the existing order
                    foreach (var newPizza in order.Pizzas)
                    {
                        if (!existingPizzas.Any(ep => ep.Id == newPizza.Id))
                        {
                            var pizzaToAdd = db.Pizzas.Find(newPizza.Id);
                            if (pizzaToAdd != null)
                            {
                                existingOrder.Pizzas.Add(pizzaToAdd);
                            }
                        }
                    }

                    db.SaveChanges();

                    return Ok(existingOrder);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateOrderStatus")]
        public ActionResult UpdateOrderStatus(int id, int statusId)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    var order = db.Orders.Find(id);
                    if (order == null)
                    {
                        return NotFound();
                    }
                    order.StatusId = statusId;
                    db.Orders.Update(order);
                    db.SaveChanges();
                    return Ok(order);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("DeleteOrder")]
        public ActionResult DeleteOrder(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Order? order = db.Orders
                        .Include(o => o.Pizzas)
                        .ThenInclude(p => p.PizzaToppings)
                        .FirstOrDefault(o => o.Id == id);

                    if (order == null)
                    {
                        return NotFound();
                    }

                    // Remove associated PizzaOrder entries
                    var pizzaOrders = db.PizzaOrders.Where(po => po.OrderId == id).ToList();
                    db.PizzaOrders.RemoveRange(pizzaOrders);

                    // Remove the order
                    db.Orders.Remove(order);
                    db.SaveChanges();
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
