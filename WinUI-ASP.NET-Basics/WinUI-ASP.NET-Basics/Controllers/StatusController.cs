using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinUI_ASP.NET_Basics.Data;
using WinUI_ASP.NET_Basics.Models;

namespace WinUI_ASP.NET_Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("GetAllStatuses")]
        public ActionResult GetAllStatuses()
        {
            try
            {
                using (AppDbContext db = new())
                {
                    List<Status> statuses = db.Statuses.ToList();
                    if (statuses.Count == 0)
                    {
                        return NotFound();
                    }
                    return Ok(statuses);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
