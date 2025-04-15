using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace szerveroldalihf3.Entities.Entity
{
    public class AppUser : IdentityUser
    {
        [StringLength(200)]
        public required string FamilyName { get; set; } = "";

        [StringLength(200)]
        public required string GivenName { get; set; } = "";

        [StringLength(200)]
        public required string RefreshToken { get; set; } = "";

        [NotMapped]
        public virtual ICollection<Ticket>? Bugs { get; set; }
    }
}
