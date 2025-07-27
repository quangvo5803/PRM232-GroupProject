using AutoMapper;
using BusinessObject.DTOs.User;
using BusinessObject.Services.Interfaces;
using DataAccess.Entities.Authorize;
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

        public async Task<IEnumerable<UserDto>> GetAllCustomersAsync()
        {
            var users = _userManager.Users.ToList();
            var customers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    customers.Add(user);
                }
            }
            var customersDto = _mapper.Map<IEnumerable<UserDto>>(
                customers ?? new List<ApplicationUser>()
            );
            return customersDto;
        }

        public async Task<UserDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var dto = _mapper.Map<UserDto>(user);

            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault();

            return dto;
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
