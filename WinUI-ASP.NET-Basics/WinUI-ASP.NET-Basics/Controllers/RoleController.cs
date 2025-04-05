using Microsoft.AspNetCore.Mvc;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        // GET: api/<Role>
        [HttpGet("GetallRoles")]
        public ActionResult<Role> GetallRoles()
        {
            try
            {
                using (AppDbContext db = new())
                {
                    List<Role> roles = db.Roles.ToList();
                    if (roles.Count == 0)
                    {
                        return NotFound();
                    }
                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
