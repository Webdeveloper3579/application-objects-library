using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AOL_Portal.Data
{
    public class AOLContext : IdentityDbContext<AolApplicationUser, AolUserRole, string>
    {
        public AOLContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(1200); // Set timeout to 60 seconds
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
