using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {

        // GET: api/<PizzaController>/GetAll
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            try
            {
                using (AppDbContext db = new())
                {
                    List<Pizza> pizzas = db.Pizzas
                        .Include(p => p.PizzaToppings)
                        .ThenInclude(pt => pt.Topping)
                        .ToList();

                    if (pizzas.Count == 0)
                    {
                        return NotFound();
                    }

                    var pizzasWithToppings = pizzas.Select(pizza => new
                    {
                        pizza.Id,
                        pizza.Name,
                        pizza.Price,
                        pizza.ImgUrl,
                        Toppings = pizza.PizzaToppings.Select(pt => new { pt.Topping.Id, pt.Topping.Name }).ToList()
                    }).ToList();

                    return Ok(pizzasWithToppings);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET api/<PizzaController>/5
        [HttpGet("GetSpecificPizza")]
        public ActionResult<Pizza> GetSpecificPizza(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Pizza? pizza = db.Pizzas
                        .Where(p => p.Id == id)
                        .Include(p => p.PizzaToppings)
                        .ThenInclude(pt => pt.Topping)
                        .FirstOrDefault();

                    if (pizza == null)
                    {
                        return NotFound();
                    }

                    var pizzaWithToppings = new
                    {
                        pizza.Id,
                        pizza.Name,
                        pizza.Price,
                        pizza.ImgUrl,
                        Toppings = pizza.PizzaToppings.Select(pt => new { pt.Topping.Id, pt.Topping.Name }).ToList()
                    };

                    return Ok(pizzaWithToppings);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<PizzaController>
        [HttpPost("CreatePizza")]
        public ActionResult CreatePizza(Pizza pizza)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    if (string.IsNullOrEmpty(pizza.Name) || pizza.Price <= 0)
                    {
                        return BadRequest("Name or Price is empty");
                    }
              

                    Pizza NewPizza = new Pizza{
                        Name = pizza.Name,
                        Price = pizza.Price,
                        ImgUrl = pizza.ImgUrl
                    }
                    ;

                    // Add the pizza to the database
                    db.Pizzas.Add(NewPizza);
                    db.SaveChanges();

                    // Add the toppings to the database
                    foreach (var topping in pizza.toppings)
                    {
                        PizzaToppings pizzaTopping = new PizzaToppings
                        {
                            ToppingId = topping.Id,
                            PizzaId = NewPizza.Id
                        };
                        db.PizzaToppings.Add(pizzaTopping);
                    }
                    db.SaveChanges();

                    return Ok(pizza);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<PizzaController>/5
        [HttpPut("EditPizza")]
        public ActionResult EditPizza(Pizza pizza)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    var existingPizza = db.Pizzas
                        .Include(p => p.PizzaToppings)
                        .FirstOrDefault(p => p.Id == pizza.Id);

                    if (existingPizza == null)
                    {
                        return NotFound();
                    }

                    // Update pizza details
                    existingPizza.Name = pizza.Name;
                    existingPizza.Price = pizza.Price;
                    existingPizza.ImgUrl = pizza.ImgUrl;

                    // Get existing toppings
                    var existingToppings = existingPizza.PizzaToppings.ToList();

                    // Remove toppings that are no longer in the new pizza
                    foreach (var existingTopping in existingToppings)
                    {
                        if (!pizza.PizzaToppings.Any(pt => pt.ToppingId == existingTopping.ToppingId))
                        {
                            db.PizzaToppings.Remove(existingTopping);
                        }
                    }

                    // Add new toppings that are not in the existing pizza
                    foreach (var newTopping in pizza.PizzaToppings)
                    {
                        if (!existingToppings.Any(et => et.ToppingId == newTopping.ToppingId))
                        {
                            newTopping.PizzaId = existingPizza.Id;
                            db.PizzaToppings.Add(newTopping);
                        }
                    }

                    db.SaveChanges();

                    return Ok(existingPizza);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<PizzaController>/5
        [HttpDelete("DeletePizza")]
        public ActionResult DeletePizza(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Pizza? pizza = db.Pizzas
                        .Include(p => p.PizzaToppings)
                        .FirstOrDefault(p => p.Id == id);
                    if (pizza == null)
                    {
                        throw new Exception("Pizza not found");
                    }
                    // Remove associated toppings
                    db.PizzaToppings.RemoveRange(pizza.PizzaToppings);
                    // Remove the pizza
                    db.Pizzas.Remove(pizza);
                    db.SaveChanges();
                    return Ok("Pizza deleted successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
