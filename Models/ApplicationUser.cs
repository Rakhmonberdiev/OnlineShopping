using Microsoft.AspNetCore.Identity;

namespace OnlineShopping.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
