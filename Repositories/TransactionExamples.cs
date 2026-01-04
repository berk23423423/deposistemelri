using System;
using System.Windows;
using DepoEnvanterApp.Data;
using DepoEnvanterApp.Models;

namespace DepoEnvanterApp.Repositories
{
    /// <summary>
    /// Transaction Kullanım Örnekleri
    /// Bu sınıf, Unit of Work pattern ile transaction kullanımına örnek teşkil eder
    /// </summary>
    public static class TransactionExamples
    {
        /// <summary>
        /// ÖRNEK 1: Birden fazla ürünü tek transaction içinde ekleme
        /// Eğer herhangi bir hata olursa tüm işlemler geri alınır (rollback)
        /// </summary>
        public static void TopluUrunEkle()
        {
            using (var unitOfWork = new UnitOfWork(new AppDbContext()))
            {
                try
                {
                    // Transaction başlat
                    unitOfWork.BeginTransaction();

                    // Birden fazla ürün ekle
                    var urun1 = new Urun
                    {
                        UrunAdi = "Laptop",
                        StokAdedi = 10,
                        Fiyat = 15000,
                        Barkod = "LAP001",
                        Kategori = "Elektronik"
                    };

                    var urun2 = new Urun
                    {
                        UrunAdi = "Mouse",
                        StokAdedi = 50,
                        Fiyat = 250,
                        Barkod = "MOU001",
                        Kategori = "Elektronik"
                    };

                    unitOfWork.Urunler.Add(urun1);
                    unitOfWork.Urunler.Add(urun2);

                    // Transaction'ı commit et (tüm değişiklikleri kaydet)
                    unitOfWork.Commit();

                    MessageBox.Show("Tüm ürünler başarıyla eklendi!");
                }
                catch (Exception ex)
                {
                    // Hata olursa transaction'ı geri al
                    unitOfWork.Rollback();
                    MessageBox.Show($"Hata oluştu, işlemler geri alındı: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// ÖRNEK 2: Ürün güncelleme ve stok hareketi aynı transaction içinde
        /// </summary>
        public static void UrunGuncelleVeStokDusur(int urunId, int satilacakMiktar)
        {
            using (var unitOfWork = new UnitOfWork(new AppDbContext()))
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    // Ürünü bul
                    var urun = unitOfWork.Urunler.GetById(urunId);
                    if (urun == null)
                    {
                        throw new Exception("Ürün bulunamadı!");
                    }

                    // Stok kontrolü
                    if (urun.StokAdedi < satilacakMiktar)
                    {
                        throw new Exception("Yetersiz stok!");
                    }

                    // Stok güncelle
                    urun.StokAdedi -= satilacakMiktar;
                    unitOfWork.Urunler.Update(urun);

                    // Burada başka işlemler de yapılabilir
                    // Örneğin: Satış kaydı oluşturma, fatura kesme vs.

                    // Tüm işlemler başarılıysa commit et
                    unitOfWork.Commit();

                    MessageBox.Show($"Satış başarılı! Yeni stok: {urun.StokAdedi}");
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    MessageBox.Show($"İşlem başarısız: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// ÖRNEK 3: Kullanıcı ve ürün ekleme aynı transaction içinde
        /// </summary>
        public static void KullaniciVeUrunEkle(string kullaniciAdi, string sifre, Urun yeniUrun)
        {
            using (var unitOfWork = new UnitOfWork(new AppDbContext()))
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    // Yeni kullanıcı ekle
                    var kullanici = new Kullanici
                    {
                        KullaniciAdi = kullaniciAdi,
                        Sifre = sifre
                    };
                    unitOfWork.Kullanicilar.Add(kullanici);

                    // Yeni ürün ekle
                    unitOfWork.Urunler.Add(yeniUrun);

                    // Her iki işlem de başarılıysa commit et
                    unitOfWork.Commit();

                    MessageBox.Show("Kullanıcı ve ürün başarıyla eklendi!");
                }
                catch (Exception ex)
                {
                    // Herhangi bir hata olursa her şeyi geri al
                    unitOfWork.Rollback();
                    MessageBox.Show($"İşlem başarısız, değişiklikler geri alındı: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// ÖRNEK 4: Transaction kullanmadan basit ekleme
        /// Tek bir işlem için transaction gerekmez, direkt SaveChanges kullanılır
        /// </summary>
        public static void BasitUrunEkle()
        {
            using (var unitOfWork = new UnitOfWork(new AppDbContext()))
            {
                var yeniUrun = new Urun
                {
                    UrunAdi = "Klavye",
                    StokAdedi = 20,
                    Fiyat = 500,
                    Barkod = "KEY001",
                    Kategori = "Elektronik"
                };

                unitOfWork.Urunler.Add(yeniUrun);
                unitOfWork.SaveChanges(); // Transaction olmadan direkt kaydet

                MessageBox.Show("Ürün eklendi!");
            }
        }
    }
}

