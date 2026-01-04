using System;
using System.Linq;
using System.Windows;
using DepoEnvanterApp.Data; // Veritabanı bağlantı sınıfı
using DepoEnvanterApp.Models; // Kullanici modeli
using DepoEnvanterApp.Repositories; // Repository pattern

namespace DepoEnvanterApp
{
    public partial class LoginWindow : Window
    {
        // Repository Pattern kullanımı (Unit of Work ile)
        private readonly IUnitOfWork _unitOfWork;

        public LoginWindow()
        {
            InitializeComponent();
            
            // Unit of Work başlat
            _unitOfWork = new UnitOfWork(new AppDbContext());
        }

        // --- GİRİŞ YAPMA MANTIĞI ---
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string girilenKullanici = txtUser.Text;
                string girilenSifre = txtPass.Password;

                // Boş alan kontrolü
                if (string.IsNullOrEmpty(girilenKullanici) || string.IsNullOrEmpty(girilenSifre))
                {
                    lblDurum.Text = "Lütfen tüm alanları doldurun!";
                    return;
                }

                // Repository pattern ile kullanıcıyı doğrula
                var user = _unitOfWork.Kullanicilar.FirstOrDefault(x =>
                    x.KullaniciAdi == girilenKullanici &&
                    x.Sifre == girilenSifre);

                if (user != null)
                {
                    // Giriş başarılı! 
                    // user.KullaniciAdi yanına '!' ekleyerek null uyarısını çözüyoruz
                    MainWindow anaEkran = new MainWindow(user.KullaniciAdi!);
                    anaEkran.Show();

                    // Giriş penceresini kapatalım
                    this.Close();
                }
                else
                {
                    lblDurum.Text = "Kullanıcı adı veya şifre hatalı!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message);
            }
        }

        // --- KAYIT OLMA MANTIĞI ---
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Boş mu kontrol et
                if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Password))
                {
                    lblDurum.Text = "Lütfen kayıt için tüm alanları doldurun!";
                    return;
                }

                // Repository pattern ile kullanıcı adı kontrolü
                var varMi = _unitOfWork.Kullanicilar.Any(x => x.KullaniciAdi == txtUser.Text);
                if (varMi)
                {
                    lblDurum.Text = "Bu kullanıcı adı zaten alınmış!";
                    return;
                }

                // Transaction başlat - Güvenli kullanıcı kaydı için
                _unitOfWork.BeginTransaction();

                // Yeni kullanıcıyı oluştur
                var yeniKullanici = new Kullanici
                {
                    KullaniciAdi = txtUser.Text,
                    Sifre = txtPass.Password
                };

                // Repository pattern ile kaydetme
                _unitOfWork.Kullanicilar.Add(yeniKullanici);
                
                // Transaction'ı commit et
                _unitOfWork.Commit();

                MessageBox.Show("Kayıt başarıyla oluşturuldu! Artık giriş yapabilirsiniz.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // Giriş kutularını temizle
                txtUser.Text = "";
                txtPass.Password = "";
                lblDurum.Text = "";
            }
            catch (Exception ex)
            {
                // Hata oluşursa rollback yap
                _unitOfWork.Rollback();
                
                lblDurum.Text = "Kayıt başarısız!";
                MessageBox.Show($"Kayıt hatası!\n\nHata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}