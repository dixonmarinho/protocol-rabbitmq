using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using protocol.rabbitmq.shared.Models;

namespace protocol.rabbitmq.data.Data
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
            base.OnModelCreating(builder);
            // Protocolo
            // Chave Unica
            builder.Entity<Protocol>().HasKey(p => new { p.Id });
            // Indice para Procura por Protocolo, CPF e RG
            builder.Entity<Protocol>().HasIndex(p => p.NumProtocol);
            builder.Entity<Protocol>().HasIndex(p => p.CPF);
            builder.Entity<Protocol>().HasIndex(p => p.RG);
            // Chave Unica - Numero do Protocolo e CPF
            builder.Entity<Protocol>().HasAlternateKey(p => new { p.NumProtocol, p.CPF });
            // Via do documento deve ser maior que 0
            builder.Entity<Protocol>().ToTable(x => x.HasCheckConstraint("chk_document_way", "numviadocumento > 0"));
            builder.Entity<Protocol>().ToTable(x => x.HasCheckConstraint("chk_cpf", "CHAR_LENGTH(cpf) = 11"));


            const string PREFIX = "auth_";
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
