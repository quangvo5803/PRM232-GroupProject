using BusinessObject.DTOs.User;
using BusinessObject.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers.Customer
{
    public partial class CustomerController : ControllerBase
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var userDto = await _userService.GetUserProfileAsync(userId);
            return userDto == null ? NotFound() : Ok(userDto);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateRequestDto updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await _userService.UpdateUserProfileAsync(userId, updateDto);
            return success ? NoContent() : NotFound();
        }
    }
}
