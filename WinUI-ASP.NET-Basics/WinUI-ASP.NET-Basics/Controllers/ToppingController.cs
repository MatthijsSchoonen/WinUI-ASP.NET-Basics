using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingController : ControllerBase
    {
        // GET: api/<ToppingController>
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            try
            {
                using (AppDbContext db = new())
                {
                    List<Topping> toppings = db.Toppings.ToList();
                    if (toppings.Count == 0)
                    {
                        return NotFound();
                    }
                    return Ok(toppings);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET api/<ToppingController>/5
        [HttpGet("GetSpecificTopping")]
        public ActionResult<Topping> GetSpecificTopping(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Topping? topping = db.Toppings.Where(t => t.Id == id).FirstOrDefault();
                    if (topping == null)
                    {
                        return NotFound();
                    }
                    return Ok(topping);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ToppingController>
        [HttpPost("CreateTopping")]
        public ActionResult CreateTopping(Topping topping)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    if(string.IsNullOrEmpty(topping.Name))
                    {
                        return BadRequest("Name is empty");
                    }
                    db.Toppings.Add(topping);
                    db.SaveChanges();
                    return Ok("Topping created successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ToppingController>/5
        [HttpPut("EditTopping")]
        public ActionResult EditTopping(Topping topping)
        {
            try
            {
                if (string.IsNullOrEmpty(topping.Name))
                {
                    return BadRequest("Name is empty");
                }
                using(AppDbContext db = new())
                {
                    Topping? existingTopping = db.Toppings.Where(t => t.Id == topping.Id).FirstOrDefault();
                    if (existingTopping == null)
                    {
                        return NotFound();
                    }
                    existingTopping.Name = topping.Name;
                    db.Toppings.Update(existingTopping);
                    db.SaveChanges();
                    return Ok("Topping updated successfully");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE api/<ToppingController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                using (AppDbContext db = new())
                {
                    Topping? topping = db.Toppings.Where(t => t.Id == id).FirstOrDefault();
                    if (topping == null)
                    {
                        return NotFound();
                    }

                    List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.ToppingId == topping.Id).ToList();
                    if (pizzaToppings.Count > 0)
                    {
                        // Remove the pizza toppings associated with the topping
                        foreach (var pizzaTopping in pizzaToppings)
                        {
                            db.PizzaToppings.Remove(pizzaTopping);
                            db.SaveChanges();
                        }
                    }
                    db.Toppings.Remove(topping);
                    db.SaveChanges();
                    return Ok("Topping deleted successfully");
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
