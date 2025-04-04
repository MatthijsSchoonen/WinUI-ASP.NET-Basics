using Microsoft.AspNetCore.Mvc;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;
using WinUI_ASP.NET_Basics.Helpers;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>/GetAllUsers
        [HttpGet("GetAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            using (AppDbContext db = new())
            {
                List<User> users = db.Users.Include(u => u.Role).ToList();
                if (users.Count == 0)
                {
                    return NotFound();
                }
                return Ok(users);
            }
        }

        // GET api/<UserController>/GetUser5
        [HttpGet("GetUser{id}")]
        public ActionResult<User> GetUser(int id)
        {
            using (AppDbContext db = new())
            {
                User? user = db.Users.Where(u => u.Id == id).Include(u => u.Role).FirstOrDefault();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
        }

        // POST api/<UserController>/Register
        [HttpPost("Register")]
        public ActionResult Register(User user)
        {
            try
            {
                using (AppDbContext db = new())
                {

                    if(string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
                    {
                        return BadRequest("Email, Name or Password is empty");
                    }

                    if (IsEmailOrUsernameTaken(user.Email, user.Name))
                    {
                        return BadRequest("Email or username already taken");
                    }
                    EmailHelper emailHelper = new();

                    if (!emailHelper.IsValidEmail(user.Email))
                    {
                        return BadRequest("Invalid email");
                    }

                    db.Users.Add(new()
                    {
                        Name = user.Name,
                        Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                        Email = user.Email,
                        RoleId = 3
                    });
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return BadRequest();
            }
        }


        // get api/<UserController>/Login
        [HttpGet("Login")]

        public ActionResult<User> login(string email, string password)
        {
            try
            {
                if(string.IsNullOrEmpty(email)&& string.IsNullOrEmpty(password))
                {
                    return BadRequest("email or password is empty");
                }
                using (AppDbContext db = new())
                {
                    User? user = db.Users.Where(u => u.Email == email).Include(u => u.Role).FirstOrDefault();
                    if (user == null)
                    {
                        return NotFound();
                    }
                    if(!BCrypt.Net.BCrypt.Verify(password, user.Password)) 
                    {
                        return BadRequest("Invalid Password");
                    }


                    return Ok(user);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<UserController>/UpdateUser5
        [HttpPut("UpdateUser")]
        public ActionResult<User> UpdateUser(User user)
        {
            try
            {
                using (AppDbContext db = new())
                {

                    if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
                    {
                        return BadRequest("Email, Name or Password is empty");
                    }

                    if (db.Users.Any(u => u.Email == user.Email && u.Id != user.Id) || db.Users.Any(u => u.Name == user.Name && u.Id != user.Id))
                    {
                        return BadRequest("Email or username already taken");
                    }
                       
                    EmailHelper emailHelper = new();

                    if (!emailHelper.IsValidEmail(user.Email))
                    {
                        return BadRequest("Invalid email");
                    }

                    db.Users.Update(user);
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return BadRequest();
            }
        }


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private bool IsEmailOrUsernameTaken(string email, string username)
        {
            using (AppDbContext db = new())
            {
                return db.Users.Any(u => u.Email == email || u.Name == username);
            }
        }
    }
}
