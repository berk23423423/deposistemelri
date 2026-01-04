using System.Linq;
using DepoEnvanterApp.Models;

namespace DepoEnvanterApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Varsayılan Admin Kullanıcısı Oluştur
            if (!context.Kullanicilar.Any())
            {
                var adminKullanici = new Kullanici
                {
                    KullaniciAdi = "admin",
                    Sifre = "admin123" // Gerçek projede şifreyi hash'leyerek saklayın!
                };

                context.Kullanicilar.Add(adminKullanici);
                context.SaveChanges();
            }

            // İsteğe bağlı: Örnek ürünler eklenebilir
            if (!context.Urunler.Any())
            {
                var ornekUrunler = new[]
                {
                    new Urun
                    {
                        UrunAdi = "Örnek Ürün 1",
                        StokAdedi = 100,
                        Fiyat = 25.50,
                        Barkod = "1234567890",
                        Kategori = "Elektronik",
                        ResimYolu = "pack://application:,,,/Resources/no-image.png"
                    },
                    new Urun
                    {
                        UrunAdi = "Örnek Ürün 2",
                        StokAdedi = 50,
                        Fiyat = 15.75,
                        Barkod = "0987654321",
                        Kategori = "Gıda",
                        ResimYolu = "pack://application:,,,/Resources/no-image.png"
                    }
                };

                context.Urunler.AddRange(ornekUrunler);
                context.SaveChanges();
            }
        }
    }
}

