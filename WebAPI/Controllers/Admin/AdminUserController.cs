using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    public partial class AdminController : ControllerBase
    {
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _facadeService.User.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var userProfile = await _facadeService.User.GetUserProfileAsync(userId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }
    }
}
