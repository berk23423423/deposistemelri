using Microsoft.EntityFrameworkCore;
using DepoEnvanterApp.Models;

namespace DepoEnvanterApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Bağlantı cümlesinde en güvenli ayarları kullanıyoruz
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=DepoEnvanterDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}