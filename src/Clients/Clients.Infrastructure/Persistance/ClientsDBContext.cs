using Clients.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clients.Infrastructure.Persistance
{
    internal class ClientsDBContext : DbContext
    {
        public ClientsDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientDbContextConfiguration());
        }
        public DbSet<Client> Clients { get; set; }
    }
}
