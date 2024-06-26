﻿namespace Application.UserCQ.ViewModels
{
    public record UserInfoViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
    }
}
