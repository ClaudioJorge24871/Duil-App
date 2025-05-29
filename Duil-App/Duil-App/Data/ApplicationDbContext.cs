using Duil_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Fabricas> Fabricas { get; set; }
        public DbSet<Encomendas> Encomendas { get; set; }
        public DbSet<Pecas> Pecas { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<LinhaEncomenda> LinhasEncomendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // Heranças 
            modelBuilder.Entity<Empresas>().ToTable("Empresas");
            modelBuilder.Entity<Clientes>().ToTable("Clientes");
            modelBuilder.Entity<Fabricas>().ToTable("Fabricas");

            // Relacionamento linhaEncomenda com encomenda
            modelBuilder.Entity<LinhaEncomenda>()
                .HasOne(le => le.Encomenda)        
                .WithMany(e => e.LinhasEncomenda)  
                .HasForeignKey(le => le.EncomendaId);

        }

    }
}
