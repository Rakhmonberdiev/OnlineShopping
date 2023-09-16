using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopping.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext (DbContextOptions options)
          : base(options)
        {
        }

    }
}
