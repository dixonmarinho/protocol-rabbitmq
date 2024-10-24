using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using protocol.rabbitqm.shared.Models;

namespace protocol.rabbitqm.data.Data
{
    public partial class AppDataContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration config;

        public DbSet<Protocol> Protocols { get; set; }

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                //var connectionstring = config.GetConnectionString("Default");
                //optionsBuilder.UseNpgsql(connectionstring);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }
            }


            const string PREFIX = "auth_";
            base.OnModelCreating(builder);
            // Chave Unica
            builder.Entity<Protocol>().HasKey(p => new { p.Id });
            builder.Entity<Protocol>().HasAlternateKey(p => new { p.NumProtocol, p.CPF });


            builder.Entity<ApplicationUser>().ToTable($"{PREFIX}users");
            builder.Entity<IdentityRole>().ToTable($"{PREFIX}roles");
            builder.Entity<IdentityUserRole<string>>().ToTable($"{PREFIX}user_roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable($"{PREFIX}user_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable($"{PREFIX}user_logins");
            builder.Entity<IdentityUserToken<string>>().ToTable($"{PREFIX}user_tokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable($"{PREFIX}role_claims");
        }
    }
}
