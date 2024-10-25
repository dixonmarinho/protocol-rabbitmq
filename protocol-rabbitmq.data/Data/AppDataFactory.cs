using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace protocol.rabbitmq.data.Data
{
    // Utiliazado para criar o contexto do banco de dados em nivel de migracao
    public class AppDataFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        AppDataContext IDesignTimeDbContextFactory<AppDataContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            optionsBuilder.UseNpgsql("Server=10.0.1.1;Port=5432;User Id=postgres;Password=123;Database=protocol;Timeout=30;CommandTimeout=120");
            return new AppDataContext(optionsBuilder.Options);
        }
    }
}
