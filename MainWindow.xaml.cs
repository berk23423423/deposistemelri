using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Globalization;
using Microsoft.Win32;
using DepoEnvanterApp.Data;
using DepoEnvanterApp.Models;
using DepoEnvanterApp.Repositories;

namespace DepoEnvanterApp
{
    public partial class MainWindow : Window
    {
        // Repository Pattern kullanımı (Unit of Work ile)
        private readonly IUnitOfWork _unitOfWork;
        private string _secilenResimYolu = "envanter.ico";
        private bool _isMenuOpen = true;
        private bool _isEditMode = false;

        private Urun? _yedekUrun = null;
        private Urun? _suAnkiDuzenlenenUrun = null;

        public ObservableCollection<Urun> UrunlerListesi { get; set; } = new ObservableCollection<Urun>();

        public MainWindow(string aktifKullanici = "aksa")
        {
            InitializeComponent();
            lblAktifKullanici.Text = aktifKullanici;
            this.DataContext = this;

            // Unit of Work başlat
            _unitOfWork = new UnitOfWork(new AppDbContext());
            
            Listele();
        }

        private void Listele()
        {
            try
            {
                // Repository üzerinden veri çekme
                var liste = _unitOfWork.Urunler.GetAll();
                UrunlerListesi.Clear();
                foreach (var urun in liste)
                {
                    UrunlerListesi.Add(urun);
                }
                dgUrunler.ItemsSource = UrunlerListesi;
                dgGenelUrunler.ItemsSource = UrunlerListesi;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri yükleme hatası: {ex.Message}");
            }
        }

        private void dgUrunler_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!_isEditMode) e.Cancel = true;
        }

        private void BtnSatirDuzenle_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditMode)
            {
                SoruSorVeIslemiYonet();
                return;
            }

            if (sender is Button btn && btn.DataContext is Urun urun)
            {
                _yedekUrun = new Urun
                {
                    UrunAdi = urun.UrunAdi,
                    StokAdedi = urun.StokAdedi,
                    Fiyat = urun.Fiyat,
                    Barkod = urun.Barkod,
                    ResimYolu = urun.ResimYolu,
                    Kategori = urun.Kategori
                };

                _suAnkiDuzenlenenUrun = urun;
                _isEditMode = true;
                dgUrunler.IsReadOnly = false;
                dgUrunler.SelectedItem = urun;
                dgUrunler.BeginEdit();
            }
        }

        private void BtnSatirKaydet_Click(object sender, RoutedEventArgs e)
        {
            // Hücredeki veriyi modele aktarır. XAML tarafında 'en-US' dediğimiz için 
            // buradaki CommitEdit noktayı otomatik olarak doğru double değere çevirir.
            if (!dgUrunler.CommitEdit(DataGridEditingUnit.Row, true)) return;

            if (_suAnkiDuzenlenenUrun != null)
            {
                try
                {
                    // Transaction başlat - Güvenli güncelleme için
                    _unitOfWork.BeginTransaction();

                    // Repository üzerinden ürünü bul
                    var dbUrun = _unitOfWork.Urunler.GetById(_suAnkiDuzenlenenUrun.Id);
                    if (dbUrun == null)
                    {
                        throw new Exception("Ürün veritabanında bulunamadı!");
                    }

                    dbUrun.UrunAdi = _suAnkiDuzenlenenUrun.UrunAdi;
                    dbUrun.StokAdedi = _suAnkiDuzenlenenUrun.StokAdedi;

                    // XAML'daki ConverterCulture='en-US' sayesinde noktadan sonrası kuruş oldu.
                    // Modeldeki double değerini direkt DB'ye aktarıyoruz.
                    dbUrun.Fiyat = _suAnkiDuzenlenenUrun.Fiyat;

                    dbUrun.Barkod = _suAnkiDuzenlenenUrun.Barkod;
                    dbUrun.ResimYolu = _suAnkiDuzenlenenUrun.ResimYolu;

                    string hamKategori = _suAnkiDuzenlenenUrun.Kategori ?? "Diğer";
                    dbUrun.Kategori = hamKategori.Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim();

                    // Repository pattern ile güncelleme
                    _unitOfWork.Urunler.Update(dbUrun);
                    
                    // Transaction'ı commit et
                    _unitOfWork.Commit();
                    
                    MessageBox.Show("Ürün başarıyla güncellendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    KilitleriAc();
                    Listele();
                }
                catch (Exception ex)
                {
                    // Hata oluşursa rollback yap
                    _unitOfWork.Rollback();
                    
                    MessageBox.Show(
                        $"Ürün güncellenirken bir hata oluştu!\n\nHata: {ex.Message}", 
                        "Hata", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    
                    // Düzenleme modunu kapat ve eski verileri geri yükle
                    if (_suAnkiDuzenlenenUrun != null && _yedekUrun != null)
                    {
                        _suAnkiDuzenlenenUrun.UrunAdi = _yedekUrun.UrunAdi;
                        _suAnkiDuzenlenenUrun.StokAdedi = _yedekUrun.StokAdedi;
                        _suAnkiDuzenlenenUrun.Fiyat = _yedekUrun.Fiyat;
                        _suAnkiDuzenlenenUrun.Barkod = _yedekUrun.Barkod;
                        _suAnkiDuzenlenenUrun.ResimYolu = _yedekUrun.ResimYolu;
                        _suAnkiDuzenlenenUrun.Kategori = _yedekUrun.Kategori;
                    }
                    
                    KilitleriAc();
                    dgUrunler.Items.Refresh();
                }
            }
        }

        // NOKTA VE RAKAM KONTROLÜ
        private void TxtFiyat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            // Eğer girilen karakter nokta ise
            if (e.Text == ".")
            {
                // Kutuda zaten nokta varsa veya kutu boşsa noktayı engelle
                e.Handled = textBox.Text.Contains(".") || textBox.Text.Length == 0;
            }
            else
            {
                // Girilen karakter rakam değilse engelle
                e.Handled = !char.IsDigit(e.Text, 0);
            }
        }

        private void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
            {
                MessageBox.Show("Lütfen ürün adı giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Transaction başlat - Güvenli ekleme için
                _unitOfWork.BeginTransaction();

                // NOKTA KULLANIMINI KURUS OLARAK ALGILATMA (Invariant Culture)
                // Bu sayede 10.50 yazıldığında sistem dili ne olursa olsun 'On lira elli kuruş' olarak okunur.
                double.TryParse(txtFiyat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double fiyatSonuc);

                var yeniUrun = new Urun
                {
                    UrunAdi = txtUrunAdi.Text,
                    StokAdedi = int.TryParse(txtStok.Text, out int s) ? s : 0,
                    Fiyat = fiyatSonuc,
                    Barkod = txtBarkod.Text,
                    ResimYolu = string.IsNullOrEmpty(_secilenResimYolu) ? "envanter.ico" : _secilenResimYolu,
                    Kategori = (cmbKategori.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Diğer"
                };

                // Repository pattern ile ekleme
                _unitOfWork.Urunler.Add(yeniUrun);
                
                // Transaction'ı commit et
                _unitOfWork.Commit();
                
                MessageBox.Show("Ürün başarıyla eklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                
                Listele();
                FormuTemizle();
            }
            catch (Exception ex)
            {
                // Hata oluşursa rollback yap
                _unitOfWork.Rollback();
                
                MessageBox.Show(
                    $"Ürün eklenirken bir hata oluştu!\n\nHata: {ex.Message}", 
                    "Hata", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private void FormuTemizle()
        {
            txtUrunAdi.Clear();
            txtStok.Clear();
            txtFiyat.Clear();
            txtBarkod.Clear();
            cmbKategori.SelectedIndex = 3;
            _secilenResimYolu = "envanter.ico";
        }

        private void SoruSorVeIslemiYonet()
        {
            var sonuc = MessageBox.Show("Kaydedilmemiş değişiklikler var. İptal edip eski haline döndürmek ister misiniz?",
                "Düzenleme Modu Aktif", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (sonuc == MessageBoxResult.Yes)
            {
                if (_suAnkiDuzenlenenUrun != null && _yedekUrun != null)
                {
                    _suAnkiDuzenlenenUrun.UrunAdi = _yedekUrun.UrunAdi;
                    _suAnkiDuzenlenenUrun.StokAdedi = _yedekUrun.StokAdedi;
                    _suAnkiDuzenlenenUrun.Fiyat = _yedekUrun.Fiyat;
                    _suAnkiDuzenlenenUrun.Barkod = _yedekUrun.Barkod;
                    _suAnkiDuzenlenenUrun.ResimYolu = _yedekUrun.ResimYolu;
                    _suAnkiDuzenlenenUrun.Kategori = _yedekUrun.Kategori;
                }
                dgUrunler.CancelEdit();
                KilitleriAc();
                dgUrunler.Items.Refresh();
            }
        }

        private void KilitleriAc()
        {
            _isEditMode = false;
            dgUrunler.IsReadOnly = true;
            _yedekUrun = null;
            _suAnkiDuzenlenenUrun = null;
        }

        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgUrunler.SelectedItem is Urun s)
            {
                if (MessageBox.Show($"{s.UrunAdi} isimli ürünü silmek istediğinize emin misiniz?", "Silme Onayı",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Transaction başlat - Güvenli silme için
                        _unitOfWork.BeginTransaction();
                        
                        // Repository pattern ile silme
                        _unitOfWork.Urunler.Remove(s);
                        
                        // Transaction'ı commit et
                        _unitOfWork.Commit();
                        
                        MessageBox.Show($"{s.UrunAdi} başarıyla silindi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        Listele();
                    }
                    catch (Exception ex)
                    {
                        // Hata oluşursa rollback yap
                        _unitOfWork.Rollback();
                        
                        MessageBox.Show(
                            $"Ürün silinirken bir hata oluştu!\n\nHata: {ex.Message}", 
                            "Hata", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void TxtArama_TextChanged(object sender, TextChangedEventArgs e)
        {
            string aranan = txtArama.Text.ToLower().Trim();
            
            // Repository pattern ile filtreleme
            var filtrelenmis = _unitOfWork.Urunler.Find(x => 
                x.UrunAdi.ToLower().Contains(aranan) || 
                x.Barkod.Contains(aranan));
            
            dgUrunler.ItemsSource = filtrelenmis;
            dgGenelUrunler.ItemsSource = filtrelenmis;
        }

        private void BtnSayfaDegistir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btnUrunler.Background = Brushes.Transparent;
                btnStok.Background = Brushes.Transparent;
                btn.Background = (Brush)new BrushConverter().ConvertFrom("#34495E");

                if (btn.Tag.ToString() == "Stok")
                {
                    StokIslemleriGrid.Visibility = Visibility.Visible;
                    UrunlerSayfasiGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    StokIslemleriGrid.Visibility = Visibility.Collapsed;
                    UrunlerSayfasiGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnResimSec_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Resim Dosyaları|*.png;*.jpg;*.jpeg;*.bmp" };
            if (ofd.ShowDialog() == true)
                _secilenResimYolu = ofd.FileName;
        }

        private void BtnSatirResimDegistir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Resim Dosyaları|*.png;*.jpg;*.jpeg" };
            if (ofd.ShowDialog() == true)
            {
                var button = sender as Button;
                if (button?.DataContext is Urun urun)
                {
                    urun.ResimYolu = ofd.FileName;

                    var row = (DataGridRow)dgUrunler.ItemContainerGenerator.ContainerFromItem(urun);
                    if (row != null)
                    {
                        row.BindingGroup?.UpdateSources();

                        dgUrunler.UpdateLayout();
                    }
                }
            }
        }

        private void BtnMenu_Toggle_Click(object sender, RoutedEventArgs e)
        {
            _isMenuOpen = !_isMenuOpen;
            SidebarColumn.Width = _isMenuOpen ? new GridLength(250) : new GridLength(0);
            btnMenuToggle.Content = _isMenuOpen ? "✕" : "☰";
        }

        private void TxtStok_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        private void MainRoot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isEditMode && e.OriginalSource is DependencyObject element)
            {
                if (!IsAllowedElement(element))
                {
                    e.Handled = true;
                    SoruSorVeIslemiYonet();
                }
            }
        }

        private bool IsAllowedElement(DependencyObject? element)
        {
            while (element != null)
            {
                if (element is ComboBox || element is ComboBoxItem) return true;
                if (element is Button btn && (btn.Name == "btnKaydet" || btn.Name == "btnDuzenle" || btn.Name == "btnResimSec")) return true;
                if (element is DataGridCell cell)
                {
                    var row = DataGridRow.GetRowContainingElement(cell);
                    if (row != null && row.IsEditing) return true;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Oturumu kapatmak istediğinize emin misiniz?", "Oturumu Kapat", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
                catch
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}