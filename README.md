# 📦 Depo Envanter Yönetim Sistemi

Modern ve kullanıcı dostu WPF tabanlı envanter yönetim uygulaması.

![.NET](https://img.shields.io/badge/.NET-10.0-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-red)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core%209.0-green)
![License](https://img.shields.io/badge/license-MIT-blue)

---

## 🚀 Hızlı Başlangıç

```bash
# Projeyi klonla
git clone https://github.com/berk23423423/deposistemelri.git

# Visual Studio'da aç
deposistemelri.sln

# F5'e bas - Veritabanı otomatik oluşacak!
```

### 🔐 İlk Giriş Bilgileri

```
Kullanıcı Adı: admin
Şifre: admin123
```

---

## 📋 Özellikler

### ✨ Ana Özellikler
- ✅ **Ürün Yönetimi** - Ekleme, güncelleme, silme, arama
- ✅ **Stok Takibi** - Gerçek zamanlı stok takibi
- ✅ **Barkod Desteği** - Her ürüne benzersiz barkod
- ✅ **Kategori Sistemi** - Ürünleri kategorilere ayırma
- ✅ **Fiyat Yönetimi** - Ürün fiyatlandırma
- ✅ **Resim Desteği** - Ürünlere resim ekleme
- ✅ **Kullanıcı Yönetimi** - Çoklu kullanıcı girişi
- ✅ **Modern UI** - Kullanıcı dostu arayüz

### 🏗️ Mimari Özellikler
- ✅ **Repository Pattern** - Temiz ve test edilebilir kod
- ✅ **Unit of Work Pattern** - Transaction desteği
- ✅ **Entity Framework Core** - Modern ORM
- ✅ **MVVM Pattern** - INotifyPropertyChanged ile
- ✅ **Otomatik Veritabanı Kurulumu** - İlk çalıştırmada hazır

---

## 🛠️ Teknolojiler

| Teknoloji | Versiyon | Açıklama |
|-----------|----------|----------|
| .NET | 10.0 | Framework |
| WPF | - | UI Framework |
| Entity Framework Core | 9.0 | ORM |
| SQL Server | Express | Veritabanı |
| C# | 12.0 | Programlama Dili |

---

## 📦 Kurulum

### Gereksinimler

- ✅ Windows 10/11
- ✅ Visual Studio 2022 (Community ücretsiz)
- ✅ .NET 10.0 SDK
- ✅ SQL Server Express

### Detaylı Kurulum Kılavuzu

**🎯 İlk kez kuruyorsan:** [`REPOSITORY_PATTERN_KULLANIMI.md`](REPOSITORY_PATTERN_KULLANIMI.md) dosyasındaki **"SIFIRDAN KURULUM KILAVUZU"** bölümünü oku!

Orada her şey adım adım anlatılmış:
- Visual Studio kurulumu
- SQL Server kurulumu
- Proje açma
- Çalıştırma
- Hata çözümleri

### Hızlı Kurulum

```bash
# 1. Projeyi klonla
git clone https://github.com/berk23423423/deposistemelri.git

# 2. Proje klasörüne gir
cd deposistemelri

# 3. NuGet paketlerini yükle
dotnet restore

# 4. Projeyi çalıştır
dotnet run
```

Ya da:
1. `deposistemelri.sln` dosyasını Visual Studio'da aç
2. F5'e bas
3. Veritabanı otomatik oluşacak!

---

## 📖 Kullanım

### İlk Giriş

1. Uygulamayı çalıştır
2. Giriş ekranında:
   - Kullanıcı: `admin`
   - Şifre: `admin123`
3. "Login" butonuna tıkla

### Ürün Ekleme

1. Sol menüden "Ürün Ekle" seçeneğini seç
2. Form alanlarını doldur:
   - Ürün Adı (zorunlu)
   - Stok Adedi
   - Fiyat
   - Barkod
   - Kategori
   - Resim (isteğe bağlı)
3. "Ekle" butonuna tıkla

### Ürün Güncelleme

1. Ürünler listesinde güncellemek istediğin ürünü bul
2. Satırdaki "Düzenle" (✏️) butonuna tıkla
3. Satır düzenleme moduna geçecek
4. Değişiklikleri yap
5. "Kaydet" (✓) butonuna tıkla

### Ürün Arama

1. Üst kısımdaki arama kutusuna yaz
2. Ürün adı veya barkoda göre anında filtreler

---

## 🏗️ Proje Yapısı

```
deposistemelri/
│
├── Data/
│   ├── AppDbContext.cs          # Entity Framework DbContext
│   └── DbInitializer.cs         # Başlangıç verileri
│
├── Models/
│   ├── Urun.cs                  # Ürün modeli
│   └── Kullanici.cs             # Kullanıcı modeli
│
├── Repositories/                 # 🆕 YENİ!
│   ├── IRepository.cs           # Generic Repository Interface
│   ├── Repository.cs            # Repository Implementation
│   ├── IUnitOfWork.cs           # Unit of Work Interface
│   ├── UnitOfWork.cs            # Unit of Work Implementation
│   └── TransactionExamples.cs   # Kullanım örnekleri
│
├── Windows/
│   └── LoginWindow.xaml.cs      # Giriş ekranı
│
├── MainWindow.xaml.cs           # Ana ekran
├── App.xaml.cs                  # Uygulama başlangıcı
└── REPOSITORY_PATTERN_KULLANIMI.md  # 📚 Detaylı kılavuz (1500+ satır!)
```

---

## 🔄 Yeni Eklenen Özellikler

### v2.0 - Repository Pattern & Transaction Desteği

#### ✅ Repository Pattern
- Temiz kod yapısı
- Test edilebilir mimari
- Generic repository implementasyonu
- SOLID prensipleri

#### ✅ Unit of Work Pattern
- Transaction yönetimi
- Atomik işlemler
- Rollback desteği
- Hata yönetimi

#### ✅ Otomatik Veritabanı Kurulumu
- İlk çalıştırmada otomatik oluşturulma
- Migration'ların otomatik uygulanması
- Varsayılan kullanıcı oluşturma
- Örnek ürünler

#### ✅ Detaylı Dokümantasyon
- 1500+ satır kılavuz
- Kod örnekleri
- Hata çözümleri
- Adım adım kurulum

---

## 💡 Kullanım Örnekleri

### Basit Ürün Ekleme

```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    var yeniUrun = new Urun
    {
        UrunAdi = "Laptop",
        StokAdedi = 10,
        Fiyat = 15000,
        Barkod = "LAP001",
        Kategori = "Elektronik"
    };

    unitOfWork.Urunler.Add(yeniUrun);
    unitOfWork.SaveChanges();
}
```

### Transaction ile Toplu İşlem

```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    try
    {
        unitOfWork.BeginTransaction();

        // Birden fazla ürün ekle
        unitOfWork.Urunler.Add(urun1);
        unitOfWork.Urunler.Add(urun2);
        unitOfWork.Urunler.Add(urun3);

        // Hepsi başarılıysa kaydet
        unitOfWork.Commit();
    }
    catch
    {
        // Hata varsa hepsini geri al
        unitOfWork.Rollback();
    }
}
```

Daha fazla örnek için: [`REPOSITORY_PATTERN_KULLANIMI.md`](REPOSITORY_PATTERN_KULLANIMI.md)

---

## 🐛 Sorun Giderme

### "The current .NET SDK does not support targeting .NET 10.0"
```
Çözüm:
1. Visual Studio Installer'ı aç
2. Update butonuna tıkla
VEYA
1. DepoEnvanterApp.csproj'da net10.0 → net8.0 yap
```
**Detaylı çözüm:** [`HATA_COZUMU_NET10.md`](HATA_COZUMU_NET10.md) ⚡

### "Cannot open database" Hatası
```
Çözüm:
1. SQL Server Express'in çalıştığından emin ol
2. Services > SQL Server (SQLEXPRESS) > Start
```

### "A network-related error" Hatası
```
Çözüm:
1. SQL Server Configuration Manager aç
2. Protocols for SQLEXPRESS > TCP/IP > Enable
3. SQL Server servisini restart et
```

### NuGet Paketi Yüklenmiyor
```
Çözüm:
1. Package Manager Console'da: dotnet restore
2. Solution'ı rebuild et
```

**Daha fazla hata çözümü:** 
- [`HATA_COZUMU_NET10.md`](HATA_COZUMU_NET10.md) - .NET 10 hatası
- [`KURULUM_KILAVUZU.md`](KURULUM_KILAVUZU.md) - Tüm hatalar
- [`REPOSITORY_PATTERN_KULLANIMI.md`](REPOSITORY_PATTERN_KULLANIMI.md) - Sorun Giderme bölümü

---

## 📚 Dokümantasyon

- **[REPOSITORY_PATTERN_KULLANIMI.md](REPOSITORY_PATTERN_KULLANIMI.md)** - Kapsamlı kılavuz (MUTLAKA OKU!)
  - Sıfırdan kurulum (adım adım)
  - Repository Pattern açıklaması
  - Transaction kullanımı
  - Gerçek dünya örnekleri
  - Sorun giderme
  - SSS

---

## 🎯 Gelecek Özellikler (Roadmap)

- [ ] Şifre hash'leme (BCrypt)
- [ ] Async/Await desteği
- [ ] Excel export
- [ ] PDF rapor oluşturma
- [ ] Çoklu dil desteği
- [ ] REST API
- [ ] Loglama sistemi
- [ ] Yedekleme özelliği

---

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Branch'e push yapın (`git push origin feature/AmazingFeature`)
5. Pull Request açın

---

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

---

## 👨‍💻 Geliştirici

**İletişim:**
- GitHub: [@berk23423423](https://github.com/berk23423423)
- Proje Linki: [https://github.com/berk23423423/deposistemelri](https://github.com/berk23423423/deposistemelri)

---

## 🙏 Teşekkürler

- Microsoft Entity Framework Core ekibine
- WPF topluluğuna
- Katkıda bulunan herkese

---

## ⭐ Destek

Projeyi beğendiyseniz yıldız vermeyi unutmayın! ⭐

---

**Not:** İlk kez kuruyorsan [`REPOSITORY_PATTERN_KULLANIMI.md`](REPOSITORY_PATTERN_KULLANIMI.md) dosyasındaki **"SIFIRDAN KURULUM KILAVUZU"** bölümünü oku!

