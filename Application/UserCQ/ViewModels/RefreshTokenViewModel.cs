namespace Application.UserCQ.ViewModels
{
    public record RefreshTokenViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
        public string? Token { get; set; }
    }
}
