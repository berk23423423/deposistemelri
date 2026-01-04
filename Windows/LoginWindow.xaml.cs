using System;
using System.Linq;
using System.Windows;
using DepoEnvanterApp.Data; // Veritabanı bağlantı sınıfı
using DepoEnvanterApp.Models; // Kullanici modeli

namespace DepoEnvanterApp
{
    public partial class LoginWindow : Window
    {
        // Veritabanı işlemlerini yönetecek olan nesnemiz
        private readonly AppDbContext _db = new AppDbContext();

        public LoginWindow()
        {
            InitializeComponent();
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

                // LINQ ile kullanıcıyı doğrulayalım
                var user = _db.Kullanicilar.FirstOrDefault(x =>
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

                // Kullanıcı adı daha önce alınmış mı?
                var varMi = _db.Kullanicilar.Any(x => x.KullaniciAdi == txtUser.Text);
                if (varMi)
                {
                    lblDurum.Text = "Bu kullanıcı adı zaten alınmış!";
                    return;
                }

                // Yeni kullanıcıyı oluştur
                var yeniKullanici = new Kullanici
                {
                    KullaniciAdi = txtUser.Text,
                    Sifre = txtPass.Password
                };

                _db.Kullanicilar.Add(yeniKullanici);
                _db.SaveChanges(); // SQL'e kaydet

                MessageBox.Show("Kayıt başarıyla oluşturuldu! Artık giriş yapabilirsiniz.");

                // Giriş kutularını temizle
                txtUser.Text = "";
                txtPass.Password = "";
                lblDurum.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt hatası: " + ex.Message);
            }
        }
    }
}