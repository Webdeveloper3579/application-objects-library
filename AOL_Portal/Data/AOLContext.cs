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

        public DbSet<AspNetCustomer> AspNetCustomers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<AolCustomerCustomField> AolCustomerCustomFields { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure AolCustomerCustomField table column order
            builder.Entity<AolCustomerCustomField>(entity =>
            {
                entity.Property(e => e.CustomerFieldId).HasColumnOrder(1);
                entity.Property(e => e.CustomerCustomFieldName).HasColumnOrder(2);
                entity.Property(e => e.CustomerCustomType).HasColumnOrder(3);
                entity.Property(e => e.CustomerCustomFieldLabel).HasColumnOrder(4);
                entity.Property(e => e.CustomerCustomFieldDescription).HasColumnOrder(5);
                entity.Property(e => e.CustomerCustomFieldType).HasColumnOrder(6);
                entity.Property(e => e.CustomerCustomFieldStatus).HasColumnOrder(7);
                entity.Property(e => e.CreatedDate).HasColumnOrder(8);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(9);
            });
        }
    }
}
