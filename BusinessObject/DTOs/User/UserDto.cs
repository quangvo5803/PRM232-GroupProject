﻿using DataAccess.Entities.Authorize;

namespace BusinessObject.DTOs.User
{
    public class UserDto
    {
        public  string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
    }
}
