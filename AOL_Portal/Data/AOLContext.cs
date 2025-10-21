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
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Object> Objects { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<SelectValue> SelectValues { get; set; }
    public DbSet<FieldValue> FieldValues { get; set; }
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

            // Configure AspNetCustomer relationships
            builder.Entity<AspNetCustomer>(entity =>
            {
                // Foreign key to AspNetUsers
                entity.HasOne(e => e.AspNetUser)
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId)
                      .HasPrincipalKey(u => u.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to AolCustomerCustomFields
                entity.HasOne(e => e.CustomField)
                      .WithMany()
                      .HasForeignKey(e => e.CustomFieldId)
                      .HasPrincipalKey(f => f.CustomerFieldId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure column order
                entity.Property(e => e.Id).HasColumnOrder(1);
                entity.Property(e => e.CustomerId).HasColumnOrder(2);
                entity.Property(e => e.CustomFieldId).HasColumnOrder(3);
                entity.Property(e => e.CustomFieldValue).HasColumnOrder(4);
                entity.Property(e => e.CreatedDate).HasColumnOrder(5);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(6);
            });

            // Configure Users table column order
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnOrder(1);
                entity.Property(e => e.FirstName).HasColumnOrder(2);
                entity.Property(e => e.LastName).HasColumnOrder(3);
                entity.Property(e => e.Email).HasColumnOrder(4);
                entity.Property(e => e.PasswordHash).HasColumnOrder(5);
                entity.Property(e => e.IsSiteAdmin).HasColumnOrder(6);
            });

            // Configure CustomerType table column order
            builder.Entity<CustomerType>(entity =>
            {
                entity.Property(e => e.CustomerTypeId).HasColumnOrder(1);
                entity.Property(e => e.CustomerTypeName).HasColumnOrder(2);
            });

            // Configure City table column order
            builder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId).HasColumnOrder(1);
                entity.Property(e => e.CityName).HasColumnOrder(2);
                entity.Property(e => e.CityCode).HasColumnOrder(3);
                entity.Property(e => e.CityCounty).HasColumnOrder(4);
                entity.Property(e => e.CityCountry).HasColumnOrder(5);
                entity.Property(e => e.CreatedDate).HasColumnOrder(6);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(7);
            });

            // Configure Object table column order
            builder.Entity<Object>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(1);
                entity.Property(e => e.ObjectName).HasColumnOrder(2);
                entity.Property(e => e.ObjectEnum).HasColumnOrder(3);
                entity.Property(e => e.CreatedDate).HasColumnOrder(4);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(5);
            });

            // Configure Field table column order
            builder.Entity<Field>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(1);
                entity.Property(e => e.ObjectId).HasColumnOrder(2);
                entity.Property(e => e.FieldLabel).HasColumnOrder(3);
                entity.Property(e => e.FieldType).HasColumnOrder(4);
                entity.Property(e => e.FieldDescription).HasColumnOrder(5);
                entity.Property(e => e.FieldName).HasColumnOrder(6);
                entity.Property(e => e.FieldStatus).HasColumnOrder(7);
                entity.Property(e => e.FieldEnum).HasColumnOrder(8);
                entity.Property(e => e.CreatedDate).HasColumnOrder(9);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(10);

                // Configure foreign key relationship
                entity.HasOne(e => e.Object)
                      .WithMany()
                      .HasForeignKey(e => e.ObjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SelectValue table column order
            builder.Entity<SelectValue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(1);
                entity.Property(e => e.FieldId).HasColumnOrder(2);
                entity.Property(e => e.Value).HasColumnOrder(3);
                entity.Property(e => e.CreatedDate).HasColumnOrder(4);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(5);

                // Configure foreign key relationship
                entity.HasOne(e => e.Field)
                      .WithMany()
                      .HasForeignKey(e => e.FieldId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<FieldValue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(1);
                entity.Property(e => e.UserId).HasColumnOrder(2);
                entity.Property(e => e.ObjectId).HasColumnOrder(3);
                entity.Property(e => e.FieldId).HasColumnOrder(4);
                entity.Property(e => e.Value).HasColumnOrder(5);
                entity.Property(e => e.CreatedDate).HasColumnOrder(6);
                entity.Property(e => e.ModifiedDate).HasColumnOrder(7);

                // Configure foreign key relationships
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Object)
                      .WithMany()
                      .HasForeignKey(e => e.ObjectId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Field)
                      .WithMany()
                      .HasForeignKey(e => e.FieldId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
