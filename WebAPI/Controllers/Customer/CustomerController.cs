using System.Security.Claims;
using BusinessObject.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Customer
{
    public partial class CustomerController : ControllerBase
    {
        [HttpGet("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userDto = await _facadeService.User.GetUserProfileAsync(userId);
            return userDto == null ? NotFound() : Ok(userDto);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto updateDto)
        {
            if (string.IsNullOrEmpty(updateDto.Id))
                return Unauthorized();

            var success = await _facadeService.User.UpdateUserProfileAsync(updateDto);
            return success ? NoContent() : NotFound();
        }
    }
}
