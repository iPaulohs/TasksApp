using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ICollection<Workspace>? Workspaces { get; set; }
        public ICollection<ListCard>? Lists { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }

        [NotMapped] 
        public new bool EmailConfirmed { get; set; }

        [NotMapped]
        public new bool PhoneNumber { get; set; }

        [NotMapped]
        public new bool PhoneNumberConfirmed { get; set; }

        [NotMapped]
        public new bool TwoFactorEnabled { get; set; }
    }
}
