using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("AOL Portal API is running!");
        }

        [HttpGet("public")]
        public ActionResult<string> GetPublic()
        {
            return Ok("This is a public endpoint - no authentication required");
        }

        [Authorize]
        [HttpGet("protected")]
        public ActionResult<string> GetProtected()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            
            return Ok($"This is a protected endpoint. User ID: {userId}, Email: {userEmail}");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public ActionResult<string> GetAdmin()
        {
            return Ok("This is an admin-only endpoint");
        }
    }
} 