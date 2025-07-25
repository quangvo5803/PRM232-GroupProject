using AutoMapper;
using BusinessObject.DTOs.User;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Authorize;
using DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserProfileAsync(UserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(updateDto.Id);
            if (user == null)
                return false;

            _mapper.Map(updateDto, user);

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
