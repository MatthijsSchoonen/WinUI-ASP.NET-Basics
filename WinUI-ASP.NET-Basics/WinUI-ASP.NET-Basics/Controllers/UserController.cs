using Microsoft.AspNetCore.Mvc;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;
using WinUI_ASP.NET_Basics.Helpers;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Microsoft.AspNetCore.Identity;

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
        [HttpGet("GetUser")]
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

                    if (!IsPasswordValid(user.Password))
                    {
                        return BadRequest("Password must atleast contain 8 character a symbol a number and a upper and lower case letter");
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

                    return Ok(user);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdatePassword")]

        public ActionResult<User> UpdatePassword(int id, string password)
        {
            try
            {
                if(id == 0 || string.IsNullOrEmpty(password))
                {
                    return BadRequest();
                }

                if (!IsPasswordValid(password))
                {
                    return BadRequest("Password must atleast contain 8 character a symbol a number and a upper and lower case letter");
                }

                using (AppDbContext db = new())
                {
                    User? user = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    if(user == null)
                    {
                        return BadRequest("user not found");
                    }

                    user.Password = BCrypt.Net.BCrypt.HashPassword(password);

                    db.Users.Update(user);
                    db.SaveChanges();
                    return Ok(user);
                }   
            }
            catch
            {
                return BadRequest();
            }
        }


        // DELETE api/<UserController>/DeleteUser
        [HttpDelete("DeleteUser")]
        public ActionResult Delete(int id)
        {
            try
            {
                using (AppDbContext db = new()) {

                    User? user = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    if(user == null)
                    {
                        return BadRequest("User not found");
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return BadRequest();
            }

        }

        private bool IsEmailOrUsernameTaken(string email, string username)
        {
            using (AppDbContext db = new())
            {
                return db.Users.Any(u => u.Email == email || u.Name == username);
            }
        }

        private bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            bool hasMinimumLength = password.Length >= 8;
            bool hasUpperCaseLetter = password.Any(char.IsUpper);
            bool hasLowerCaseLetter = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasMinimumLength && hasUpperCaseLetter && hasLowerCaseLetter && hasDigit && hasSymbol;
        }

    }
}
