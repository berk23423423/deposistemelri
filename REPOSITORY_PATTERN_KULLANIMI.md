# Repository Pattern & Transaction Kullanım Kılavuzu

```
╔═══════════════════════════════════════════════════════════════════════╗
║                                                                       ║
║  🚨 İLK KEZ ÇALIŞTIRACAKSAN BURAYA TIKLA!                            ║
║                                                                       ║
║  👇 Aşağıdaki "SIFIRDAN KURULUM KILAVUZU" bölümünü OKU!             ║
║                                                                       ║
║  Her şey adım adım anlatılmış:                                       ║
║  ✅ Visual Studio kurulumu                                           ║
║  ✅ SQL Server kurulumu                                              ║
║  ✅ Projeyi indirme                                                  ║
║  ✅ Çalıştırma                                                       ║
║  ✅ Hata çözümleri                                                   ║
║                                                                       ║
║  Kullanıcı: admin  |  Şifre: admin123                               ║
║                                                                       ║
╚═══════════════════════════════════════════════════════════════════════╝
```

## 📑 İçindekiler

### 🔴 İLK KULLANIMDA MUTLAKA OKU:
1. **[🆕 SIFIRDAN KURULUM KILAVUZU](#-sifirdan-kurulum-kilavuzu)** ⬅️ BURADAN BAŞLA!
   - Sistem Gereksinimleri (Visual Studio, SQL Server)
   - Projeyi İndirme (Git veya ZIP)
   - Visual Studio'da Açma
   - NuGet Paketleri Yükleme
   - Veritabanı Bağlantısı Kontrolü
   - Projeyi Çalıştırma
   - Yaygın Hatalar ve Çözümleri (8+ senaryo)

### 📚 Dokümantasyon:
2. [Hızlı Başlangıç](#-hızlı-başlangıç)
3. [Proje Analizi - Neler Değişti?](#-proje-analizi---neler-değişti)
3. [Yapılan İyileştirmeler](#-yapılan-i̇yileştirmeler)
4. [Sistem Nasıl Çalışıyor?](#️-sistem-nasıl-çalışıyor)
5. [Kullanım Örnekleri](#-kullanım-örnekleri)
   - [Basit CRUD İşlemleri](#basit-crud-i̇şlemleri-transactionsız)
   - [Transaction Kullanımı](#-transaction-kullanımı)
6. [Klasör Yapısı ve Dosya Açıklamaları](#-klasör-yapısı-ve-dosya-açıklamaları)
7. [Önemli Notlar](#️-önemli-notlar)
8. [Sık Sorulan Sorular](#-sık-sorulan-sorular-sss)
9. [Sorun Giderme](#-sorun-giderme)
10. [Sonraki Adımlar](#-sonraki-adımlar-opsiyonel-i̇yileştirmeler)
11. [Kaynaklar](#-kaynaklar-ve-öğrenme-materyalleri)

---

## 🆕 SIFIRDAN KURULUM KILAVUZU

**⚠️ ÖNEMLİ: İlk kez çalıştıracaksanız ayrı kurulum kılavuzunu okuyun!**

```
╔══════════════════════════════════════════════════════════════════╗
║                                                                  ║
║  📚 DETAYLI KURULUM KILAVUZU HAZIR!                             ║
║                                                                  ║
║  👉 KURULUM_KILAVUZU.md dosyasını oku!                          ║
║                                                                  ║
║  İçinde:                                                         ║
║  ✅ Visual Studio kurulumu (ekran görüntüleriyle)               ║
║  ✅ SQL Server kurulumu (adım adım)                             ║
║  ✅ Projeyi indirme ve açma                                     ║
║  ✅ İlk çalıştırma                                              ║
║  ✅ 8+ yaygın hata ve çözümleri                                 ║
║  ✅ Video rehber linkleri                                       ║
║  ✅ 50+ sayfa detaylı anlatım                                   ║
║                                                                  ║
║  Hiçbir teknik bilgisi olmayan birine anlatır gibi yazıldı!    ║
║                                                                  ║
╚══════════════════════════════════════════════════════════════════╝
```

### 🔗 Kurulum Kılavuzuna Git

**[👉 KURULUM_KILAVUZU.md](KURULUM_KILAVUZU.md)** - Tıkla ve aç!

### 📋 Hızlı Özet (Deneyimli Kullanıcılar İçin)

```
Gereksinimler:
✅ Windows 10/11
✅ Visual Studio 2022 + .NET desktop development
✅ SQL Server Express
✅ Git (opsiyonel)

Hızlı Kurulum:
1. git clone https://github.com/berk23423423/deposistemelri.git
2. Visual Studio'da deposistemelri.sln'i aç
3. F5'e bas
4. Login: admin / admin123
```

**Detaylı kurulum için:** [KURULUM_KILAVUZU.md](KURULUM_KILAVUZU.md) dosyasını oku!

---

## 🚀 Hızlı Başlangıç

### Projeyi İlk Kez Çalıştırma

**⚠️ Detaylı adımlar için:** [KURULUM_KILAVUZU.md](KURULUM_KILAVUZU.md) dosyasına bak!

Bilgisayarınızda şunlar olmalı:

#### ✅ Windows İşletim Sistemi
- Windows 10 veya Windows 11
- (Windows 7/8 de çalışır ama önerilmez)

#### ✅ Visual Studio (IDE)
**Ne yüklemeliyim?**
1. [Visual Studio 2022 Community](https://visualstudio.microsoft.com/tr/downloads/) (ÜCRETSİZ)
2. Visual Studio 2019 da olur ama 2022 daha iyi

**Nasıl yüklerim?**
- Yukarıdaki linke tıkla
- "Visual Studio 2022 Community" altındaki "Ücretsiz İndir" butonuna tıkla
- İndirilen dosyayı çalıştır
- Kurulum sırasında MUTLAKA şunu seç:
  - ✅ **.NET desktop development** (Bunu işaretle!)
  - ✅ **ASP.NET and web development** (İsterseniz)

#### ✅ SQL Server Express (Veritabanı)
**Ne yüklemeliyim?**
1. [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (ÜCRETSİZ)
2. SQL Server Management Studio (SSMS) - Opsiyonel ama tavsiye edilir

**Nasıl yüklerim?**

**ADIM 1:** SQL Server Express İndir
- Linke git: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- "Express" sürümünü bul
- "Download now" tıkla

**ADIM 2:** Kurulumu Başlat
```
1. İndirilen dosyayı çift tıkla
2. "Basic" installation seçeneğini seç (en kolay)
3. "Accept" (Lisans sözleşmesini kabul et)
4. İnstall dizinini değiştirme, varsayılan bırak
5. "Install" butonuna tıkla
6. Kurulum 5-10 dakika sürer, bekle...
7. Bitince "Close" yap
```

**ADIM 3:** SQL Server Management Studio (İsteğe Bağlı)
```
1. https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
2. "Free Download for SQL Server Management Studio (SSMS)" tıkla
3. İndirilen dosyayı çalıştır
4. Next > Next > Install
```

---

### 📥 Adım 1: Projeyi İndirme

#### Seçenek A: Git ile İndirme (Önerilen)

**Git yüklü mü kontrol et:**
```bash
# CMD veya PowerShell'de çalıştır:
git --version
```

**Eğer "git is not recognized" hatası alırsan:**
1. Git'i indir: https://git-scm.com/download/win
2. Kur (Next > Next > Next yap)
3. CMD'yi kapat ve tekrar aç

**Projeyi indir:**
```bash
# 1. İstediğin bir klasöre git (örneğin):
cd C:\Users\KULLANICI_ADIN\Desktop

# 2. Projeyi klonla:
git clone https://github.com/berk23423423/deposistemelri.git

# 3. Proje klasörüne gir:
cd deposistemelri
```

#### Seçenek B: ZIP ile İndirme

```
1. Tarayıcıda git: https://github.com/berk23423423/deposistemelri
2. Yeşil "Code" butonuna tıkla
3. "Download ZIP" seç
4. İndirilen ZIP'i masaüstüne çıkart
5. Klasör ismini "deposistemelri" yap
```

---

### 🔧 Adım 2: Visual Studio'da Açma

#### Yöntem 1: Solution Dosyasını Çift Tıklama
```
1. Proje klasörünü aç (deposistemelri)
2. "deposistemelri.sln" dosyasını bul
3. Çift tıkla
4. Visual Studio açılacak
```

#### Yöntem 2: Visual Studio İçinden Açma
```
1. Visual Studio'yu aç
2. "Open a project or solution" tıkla
3. deposistemelri klasörüne git
4. "deposistemelri.sln" seç
5. "Open" tıkla
```

---

### 📦 Adım 3: NuGet Paketlerini Yükleme

**NE OLUYOR?** Proje bazı kütüphanelere ihtiyaç duyuyor (Entity Framework Core vs.), bunları indirmemiz gerekiyor.

#### Otomatik Yükleme (Önerilen):
```
1. Proje açıldığında Visual Studio otomatik yüklemeyi önerecek
2. Eğer "Restore NuGet Packages" yazısı görürsen TIKLA
3. Sağ altta "Restoring NuGet packages..." yazacak, BEKLE
4. Bitince "Restore completed" yazacak
```

#### Manuel Yükleme:
```
1. Visual Studio'da üst menüden:
   Tools > NuGet Package Manager > Package Manager Console

2. Aşağı kısımda bir konsol penceresi açılacak

3. Şunu yaz ve Enter'a bas:
   dotnet restore

4. Paketler indirilecek, bekle...
```

#### Hata Alırsan:
```
Hata: "Unable to find package..."

Çözüm:
1. Solution Explorer'da proje adına sağ tık
2. "Rebuild" tıkla
3. Tekrar dene
```

---

### 🗄️ Adım 4: SQL Server Bağlantısını Kontrol Etme

**ÖNEMLİ:** Bağlantı stringi doğru olmalı!

#### Bağlantı Stringini Kontrol Et:

```
1. Visual Studio'da Solution Explorer'ı aç (sağ tarafta)
2. "Data" klasörünü aç
3. "AppDbContext.cs" dosyasını çift tıkla
4. 14. satıra bak:
```

```csharp
optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=DepoEnvanterDB;Trusted_Connection=True;TrustServerCertificate=True;");
```

#### Bağlantı Stringi Doğru mu Kontrol Et:

**SQL Server Instance Adını Öğren:**
```
Yöntem 1: SQL Server Configuration Manager
1. Windows'da ara: "SQL Server Configuration Manager"
2. Aç
3. "SQL Server Services" bak
4. "SQL Server (SQLEXPRESS)" yazıyorsa DOĞRU
5. Farklı bir isim varsa (örn: MSSQLSERVER) onu kullan

Yöntem 2: CMD ile Kontrol
1. CMD aç
2. Şunu yaz:
   sqlcmd -L
3. Çıkan listeye bak
4. Genellikle "BILGISAYAR_ADI\SQLEXPRESS" şeklinde olur
```

#### Bağlantı Stringini Değiştir (Gerekirse):

**Eğer SQL Server farklı bir isimle kuruluysa:**

```csharp
// SQLEXPRESS yerine kendi instance adını yaz:

// Örnek 1: Varsayılan instance
Server=.;Database=DepoEnvanterDB;Trusted_Connection=True;TrustServerCertificate=True;

// Örnek 2: Named instance
Server=.\SENIN_INSTANCE_ADIN;Database=DepoEnvanterDB;Trusted_Connection=True;TrustServerCertificate=True;

// Örnek 3: LocalDB
Server=(localdb)\MSSQLLocalDB;Database=DepoEnvanterDB;Trusted_Connection=True;TrustServerCertificate=True;
```

---

### ⚡ Adım 5: Projeyi Derleme (Build)

**NE OLUYOR?** Kod derleniyor, hata var mı kontrol ediliyor.

```
1. Visual Studio üst menüden:
   Build > Rebuild Solution

2. Alt kısımda "Output" penceresinde şöyle yazmalı:
   "========== Rebuild All: 1 succeeded, 0 failed, 0 skipped =========="

3. Eğer "succeeded" yazıyorsa TAMAM!
4. "failed" yazıyorsa HATA VAR, aşağıdaki çözümlere bak
```

#### Yaygın Derleme Hataları:

**Hata 1: "The type or namespace name 'EntityFrameworkCore' could not be found"**
```
Çözüm:
1. NuGet paketleri yüklenmemiş
2. Adım 3'ü tekrar yap (dotnet restore)
```

**Hata 2: "The current .NET SDK does not support targeting .NET 10.0"**
```
Çözüm:
1. .NET 10 SDK'sını indir: https://dotnet.microsoft.com/download
2. En son sürümü yükle
3. Visual Studio'yu kapat ve tekrar aç
```

**Hata 3: "Cannot find Windows SDK"**
```
Çözüm:
1. Visual Studio Installer'ı aç
2. "Modify" tıkla
3. "Individual components" sekmesine git
4. "Windows 10 SDK" işaretle
5. Install
```

---

### 🎯 Adım 6: Veritabanını Hazırlama

**ÖNEMLİ:** Artık veritabanı OTOMATİK oluşuyor! Ama yine de kontrol edelim.

#### Yöntem 1: Otomatik Oluşum (Önerilen)
```
Hiçbir şey yapmana gerek yok!
Uygulama ilk çalıştığında veritabanını kendisi oluşturacak.
Adım 7'ye geç.
```

#### Yöntem 2: Manuel Oluşturma (Sorun Varsa)

**Package Manager Console kullan:**
```
1. Tools > NuGet Package Manager > Package Manager Console
2. Konsolda şunu yaz:

   Update-Database

3. Enter'a bas
4. "Done" yazana kadar bekle
5. Hata yoksa tamamdır!
```

**Hata Alırsan:**
```
Hata: "A network-related or instance-specific error..."

Çözüm:
1. SQL Server servisi çalışmıyor olabilir
2. Windows'da ara: "Services"
3. "Services" uygulamasını aç
4. "SQL Server (SQLEXPRESS)" bul
5. Sağ tık > Start
6. Tekrar dene
```

---

### 🚀 Adım 7: UYGULAMAYI ÇALIŞTIRMA

**ARTIK ÇALIŞTIRABILIRIZ!**

#### Çalıştırma Adımları:

```
Yöntem 1: F5 Tuşuna Bas
1. Klavyeden F5'e bas
2. Uygulama derlenecek
3. Veritabanı otomatik oluşacak (ilk seferinde)
4. Login penceresi açılacak

Yöntem 2: Yeşil "Play" Butonu
1. Visual Studio üst kısmında yeşil ▶ buton var
2. Yanında "DepoEnvanterApp" yazıyor
3. Ona tıkla
4. Uygulama açılacak
```

#### İlk Çalıştırmada Neler Olur?

```
1. Uygulama açılır
2. Arka planda veritabanı oluşturulur (3-5 saniye)
3. Admin kullanıcısı otomatik eklenir
4. 2 örnek ürün eklenir
5. Login ekranı açılır
```

#### Giriş Yapma:

```
Kullanıcı Adı: admin
Şifre: admin123

"Login" butonuna tıkla
Ana ekran açılır!
```

---

### 🐛 SORUN GİDERME - Yaygın Hatalar ve Çözümler

#### Hata 1: Uygulama Açılmıyor, Hata Mesajı Yok
```
Çözüm 1: Debug Modunda Çalıştır
1. F5 yerine Ctrl+F5 dene
2. Hata mesajı göreceksin

Çözüm 2: Output Penceresine Bak
1. View > Output
2. Hata mesajlarını oku
```

#### Hata 2: "Cannot open database 'DepoEnvanterDB'"
```
Çözüm:
1. SQL Server çalışmıyor
2. Services'i aç (Windows'da ara)
3. "SQL Server (SQLEXPRESS)" servisini başlat
4. Tekrar dene
```

#### Hata 3: "Login failed for user"
```
Çözüm:
1. Windows Authentication kullanılıyor
2. Bağlantı stringinde "Trusted_Connection=True" olmalı
3. AppDbContext.cs dosyasını kontrol et
```

#### Hata 4: "A network-related or instance-specific error occurred"
```
Çözüm 1: SQL Server Servisi
1. Services'de "SQL Server (SQLEXPRESS)" başlat

Çözüm 2: TCP/IP Protokolü
1. SQL Server Configuration Manager aç
2. "SQL Server Network Configuration" > "Protocols for SQLEXPRESS"
3. TCP/IP'yi enable et
4. SQL Server servisini restart et

Çözüm 3: Windows Firewall
1. Firewall'da SQL Server'a izin ver
2. Gelen Kurallar > Yeni Kural > Program
3. sqlservr.exe yolunu ekle
```

#### Hata 5: "The type initializer for 'Microsoft.Data.SqlClient.SNI.SNILoadHandle' threw an exception"
```
Çözüm:
1. Microsoft.Data.SqlClient.SNI paketini güncelle
2. Package Manager Console'da:
   Update-Package Microsoft.Data.SqlClient
```

#### Hata 6: Veritabanı Oluşuyor Ama Admin Kullanıcısı Yok
```
Çözüm:
1. Veritabanını sil
2. SQL Server Management Studio'da (varsa):
   - Connect to server
   - Databases > DepoEnvanterDB > Sağ tık > Delete
3. Uygulamayı tekrar çalıştır
4. Otomatik tekrar oluşacak
```

#### Hata 7: "Could not load file or assembly 'System.Runtime.CompilerServices.Unsafe'"
```
Çözüm:
1. Tüm NuGet paketlerini temizle ve yeniden yükle:

Package Manager Console'da:
Remove-Item -Recurse -Force packages
dotnet restore

2. Solution'ı rebuild et:
Build > Rebuild Solution
```

#### Hata 8: Login Ekranı Açılıyor Ama "admin/admin123" Çalışmıyor
```
Çözüm 1: Veritabanını Kontrol Et
1. SSMS'de bağlan
2. DepoEnvanterDB > Tables > dbo.Kullanicilar > Sağ tık > Select Top 1000 Rows
3. Admin kullanıcısı var mı bak

Çözüm 2: Manuel Kullanıcı Ekle
1. "Register" butonuna tıkla
2. Kendi kullanıcı adını oluştur
3. Sonra giriş yap

Çözüm 3: SQL ile Manuel Ekle
SSMS'de şunu çalıştır:
INSERT INTO Kullanicilar (KullaniciAdi, Sifre) VALUES ('admin', 'admin123')
```

---

### ✅ Başarıyla Çalıştırıldı mı Kontrol Et

**Şunları görebiliyor musun?**
- ✅ Login penceresi açıldı
- ✅ admin/admin123 ile giriş yapabildin
- ✅ Ana ekran açıldı
- ✅ 2 örnek ürün göründü
- ✅ Yeni ürün ekleyebiliyorsun
- ✅ Ürünleri silebiliyorsun

**HEPSİNİ GÖREBİLİYORSAN BAŞARILI! 🎉**

---

### 📞 Hâlâ Sorun mu Var?

**Adım adım kontrol listesi:**

```
□ Windows 10/11 kurulu
□ Visual Studio 2022 kurulu
□ .NET desktop development yüklü
□ SQL Server Express kurulu
□ SQL Server servisi çalışıyor
□ Proje indirildi
□ Visual Studio'da açıldı
□ NuGet paketleri yüklendi (dotnet restore)
□ Build başarılı (0 errors)
□ Bağlantı stringi doğru
□ F5 ile çalıştırıldı
□ Login ekranı açıldı
```

**Hangi adımda takılıyorsun?**
Yukarıdaki listeden kontrol et, hangi adımda sorun varsa o bölüme dön.

---

### 🎥 Video Anlatım İstiyor musun?

Eğer yukarıdaki adımları takip etmekte zorlanıyorsan:
1. Ekran görüntüleri çek (her adımı)
2. Hata mesajlarının ekran görüntüsünü al
3. Projeyi geliştiren kişiye gönder

---

## 🚀 Hızlı Başlangıç

### Projeyi İlk Kez Çalıştırma
1. Projeyi açın ve çalıştırın (F5)
2. Veritabanı otomatik oluşacak
3. Login ekranında şu bilgilerle giriş yapın:

```
Kullanıcı Adı: admin
Şifre: admin123
```

> 💡 **İpucu:** Bu varsayılan kullanıcı `DbInitializer.cs` dosyasından değiştirilebilir.

---

## 🔍 Proje Analizi - Neler Değişti?

### ❌ ÖNCEDEN NASIL ÇALIŞIYORDU?

**Sorunlar:**
1. **Her yerde direkt DbContext kullanımı:**
   ```csharp
   private readonly AppDbContext _db = new AppDbContext();
   var urunler = _db.Urunler.ToList(); // Her sınıfta tekrar tekrar
   _db.Urunler.Add(yeniUrun);
   _db.SaveChanges();
   ```

2. **Transaction desteği YOK:**
   - Birden fazla işlem yapılırken biri başarısız olsa diğeri kaydediliyordu
   - Atomik işlemler yapılamıyordu

3. **Veritabanı manuel kurulum:**
   - Kullanıcı Package Manager Console'da `Update-Database` komutu çalıştırmalıydı
   - İlk kullanıcı yoktu, kayıt olunmalıydı

4. **Test edilemez kod:**
   - DbContext'e direkt bağımlılık
   - Mock'lama zordu

5. **Kod tekrarı:**
   - Her pencerede aynı CRUD kodları tekrar tekrar yazılıyordu

### ✅ ŞİMDİ NASIL ÇALIŞIYOR?

**Çözümler:**
1. **Repository Pattern:**
   ```csharp
   private readonly IUnitOfWork _unitOfWork;
   var urunler = _unitOfWork.Urunler.GetAll(); // Temiz ve test edilebilir
   _unitOfWork.Urunler.Add(yeniUrun);
   _unitOfWork.SaveChanges();
   ```

2. **Transaction desteği VAR:**
   ```csharp
   _unitOfWork.BeginTransaction();
   // Birden fazla işlem
   _unitOfWork.Commit(); // Ya hepsi başarılı ya hepsi iptal
   ```

3. **Otomatik kurulum:**
   - Uygulama açılır açılmaz veritabanı hazır
   - Admin kullanıcısı otomatik oluşuyor

4. **Test edilebilir:**
   - Interface'ler sayesinde kolayca mock'lanabilir

5. **Temiz kod:**
   - Tüm veri erişimi tek yerden yönetiliyor

---

## 📋 Yapılan İyileştirmeler

### 1. ✅ Veritabanı Otomatik Oluşturma
Uygulama başladığında otomatik olarak:
- Veritabanı yoksa oluşturulur
- Pending migration'lar otomatik uygulanır
- Varsayılan admin kullanıcısı oluşturulur
- Örnek ürünler eklenir

**🔐 Varsayılan Giriş Bilgileri:**
```
Kullanıcı Adı: admin
Şifre: admin123
```
> ⚠️ **Not:** Bu varsayılan kullanıcı ilk çalıştırmada otomatik olarak `DbInitializer.cs` tarafından oluşturulur. İsterseniz bu dosyadan değiştirebilir veya kaldırabilirsiniz.

### 2. ✅ Repository Pattern
Artık direkt `DbContext` kullanılmıyor. Tüm veri erişimi **Repository Pattern** üzerinden yapılıyor.

**Avantajları:**
- Temiz kod yapısı
- Test edilebilirlik
- Değişikliklere karşı daha esnek
- Kod tekrarının azalması

### 3. ✅ Unit of Work Pattern
Transaction desteği ile birden fazla işlemi tek bir transaction içinde yönetebilme

**Avantajları:**
- Atomik işlemler (ya hepsi başarılı olur ya hepsi geri alınır)
- Veritabanı tutarlılığı
- Hata yönetimi

---

## ⚙️ Sistem Nasıl Çalışıyor?

### 1. Uygulama Başlatma Akışı

```
[Uygulama Başlatıldı]
         ↓
[App.xaml.cs - OnStartup()]
         ↓
[Veritabanı var mı kontrol et]
         ↓
    ┌────┴────┐
 YOK          VAR
    ↓          ↓
[Oluştur]  [Migration kontrol]
    ↓          ↓
    └────┬────┘
         ↓
[DbInitializer.Initialize()]
         ↓
    ┌────┴────┐
    ↓         ↓
[Admin      [Örnek
 Oluştur]    Ürünler]
    ↓         ↓
    └────┬────┘
         ↓
[LoginWindow Açılır]
```

### 2. Veri Okuma İşlemi Akışı

```
[Kullanıcı: Listele butonuna tıklar]
         ↓
[MainWindow: Listele() metodu]
         ↓
[_unitOfWork.Urunler.GetAll()]
         ↓
[Repository: GetAll() metodu]
         ↓
[Entity Framework: _dbSet.ToList()]
         ↓
[SQL Server: SELECT * FROM Urunler]
         ↓
[Veriler geri döner]
         ↓
[ObservableCollection'a eklenir]
         ↓
[DataGrid'de gösterilir]
```

### 3. Veri Ekleme İşlemi Akışı (Transaction'sız)

```
[Kullanıcı: Form doldurur + Ekle tıklar]
         ↓
[MainWindow: BtnEkle_Click()]
         ↓
[Yeni Urun nesnesi oluşturulur]
         ↓
[_unitOfWork.Urunler.Add(yeniUrun)]
         ↓
[Repository: Add() metodu]
         ↓
[_dbSet.Add(entity) - Bellekte bekler]
         ↓
[_unitOfWork.SaveChanges()]
         ↓
[DbContext.SaveChanges()]
         ↓
[SQL: INSERT INTO Urunler ...]
         ↓
[Başarılı! Listele() çağrılır]
```

### 4. Transaction İşlemi Akışı

```
[Transaction Başla]
         ↓
[_unitOfWork.BeginTransaction()]
         ↓
[SQL: BEGIN TRANSACTION]
         ↓
[İşlem 1: Ürün ekle]
         ↓
[İşlem 2: Stok güncelle]
         ↓
[İşlem 3: Kayıt oluştur]
         ↓
    ┌────┴────┐
BAŞARILI    HATA
    ↓         ↓
[Commit]  [Rollback]
    ↓         ↓
[KAYDET]  [İPTAL]
    ↓         ↓
[SQL:     [SQL:
 COMMIT]   ROLLBACK]
```

### 5. Repository Pattern Katmanları

```
┌─────────────────────────────────────┐
│   UI Layer (XAML/Windows)           │
│   - MainWindow.xaml.cs              │
│   - LoginWindow.xaml.cs             │
└──────────────┬──────────────────────┘
               ↓
┌──────────────────────────────────────┐
│   Service Layer (Unit of Work)      │
│   - UnitOfWork.cs                    │
│   - Transaction Yönetimi             │
└──────────────┬───────────────────────┘
               ↓
┌──────────────────────────────────────┐
│   Repository Layer                   │
│   - Repository<T>                    │
│   - CRUD İşlemleri                   │
└──────────────┬───────────────────────┘
               ↓
┌──────────────────────────────────────┐
│   Data Access Layer                  │
│   - AppDbContext                     │
│   - Entity Framework Core            │
└──────────────┬───────────────────────┘
               ↓
┌──────────────────────────────────────┐
│   Database (SQL Server)              │
│   - Urunler Tablosu                  │
│   - Kullanicilar Tablosu             │
└──────────────────────────────────────┘
```

---

## 🚀 Kullanım Örnekleri

### Basit CRUD İşlemleri (Transaction'sız)

#### Ürün Ekleme
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

#### Ürün Listeleme
```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    // Tüm ürünleri getir
    var tumUrunler = unitOfWork.Urunler.GetAll();

    // ID'ye göre getir
    var urun = unitOfWork.Urunler.GetById(1);

    // Şarta göre filtrele
    var elektronikUrunler = unitOfWork.Urunler.Find(x => x.Kategori == "Elektronik");

    // İlk eşleşeni bul
    var urun2 = unitOfWork.Urunler.FirstOrDefault(x => x.Barkod == "LAP001");

    // Var mı kontrolü
    bool varMi = unitOfWork.Urunler.Any(x => x.UrunAdi == "Laptop");
}
```

#### Ürün Güncelleme
```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    var urun = unitOfWork.Urunler.GetById(1);
    if (urun != null)
    {
        urun.StokAdedi = 20;
        urun.Fiyat = 14500;

        unitOfWork.Urunler.Update(urun);
        unitOfWork.SaveChanges();
    }
}
```

#### Ürün Silme
```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    var urun = unitOfWork.Urunler.GetById(1);
    if (urun != null)
    {
        unitOfWork.Urunler.Remove(urun);
        unitOfWork.SaveChanges();
    }
}
```

---

## 🔄 Transaction Kullanımı

### ÖRNEK 1: Toplu Ürün Ekleme
Birden fazla ürünü tek transaction içinde eklemek. Eğer herhangi biri başarısız olursa hiçbiri eklenmez.

```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    try
    {
        // Transaction başlat
        unitOfWork.BeginTransaction();

        // Birden fazla ürün ekle
        var urun1 = new Urun { UrunAdi = "Laptop", StokAdedi = 10, Fiyat = 15000 };
        var urun2 = new Urun { UrunAdi = "Mouse", StokAdedi = 50, Fiyat = 250 };
        var urun3 = new Urun { UrunAdi = "Klavye", StokAdedi = 30, Fiyat = 500 };

        unitOfWork.Urunler.Add(urun1);
        unitOfWork.Urunler.Add(urun2);
        unitOfWork.Urunler.Add(urun3);

        // Her şey başarılıysa commit et
        unitOfWork.Commit();

        MessageBox.Show("Tüm ürünler başarıyla eklendi!");
    }
    catch (Exception ex)
    {
        // Hata olursa tüm işlemleri geri al
        unitOfWork.Rollback();
        MessageBox.Show($"Hata: {ex.Message}");
    }
}
```

### ÖRNEK 2: Stok Transfer İşlemi
İki ürün arasında stok transferi. Eğer herhangi biri başarısız olursa her iki işlem de geri alınır.

```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    try
    {
        unitOfWork.BeginTransaction();

        // Kaynak üründen stok düş
        var kaynakUrun = unitOfWork.Urunler.GetById(1);
        if (kaynakUrun.StokAdedi < 10)
            throw new Exception("Yetersiz stok!");

        kaynakUrun.StokAdedi -= 10;
        unitOfWork.Urunler.Update(kaynakUrun);

        // Hedef ürüne stok ekle
        var hedefUrun = unitOfWork.Urunler.GetById(2);
        hedefUrun.StokAdedi += 10;
        unitOfWork.Urunler.Update(hedefUrun);

        // Her iki işlem de başarılıysa commit et
        unitOfWork.Commit();

        MessageBox.Show("Transfer başarılı!");
    }
    catch (Exception ex)
    {
        unitOfWork.Rollback();
        MessageBox.Show($"Transfer başarısız: {ex.Message}");
    }
}
```

### ÖRNEK 3: Satış İşlemi (Stok Düşürme + Kayıt Ekleme)
Satış yapılırken hem stok düşer hem de başka tablolara kayıt eklenebilir.

```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    try
    {
        unitOfWork.BeginTransaction();

        // Ürünü bul ve stoğunu düşür
        var urun = unitOfWork.Urunler.GetById(urunId);
        if (urun.StokAdedi < satilacakMiktar)
            throw new Exception("Yetersiz stok!");

        urun.StokAdedi -= satilacakMiktar;
        unitOfWork.Urunler.Update(urun);

        // Burada satış kaydı eklenebilir (başka bir tablo varsa)
        // var satis = new Satis { ... };
        // unitOfWork.Satislar.Add(satis);

        // Her şey tamam, commit et
        unitOfWork.Commit();

        MessageBox.Show("Satış başarılı!");
    }
    catch (Exception ex)
    {
        unitOfWork.Rollback();
        MessageBox.Show($"Satış başarısız: {ex.Message}");
    }
}
```

---

## 📦 Klasör Yapısı ve Dosya Açıklamaları

```
DepoEnvanterApp/
│
├── Data/
│   ├── AppDbContext.cs          # Entity Framework DbContext
│   └── DbInitializer.cs         # Seed Data (Başlangıç verileri)
│
├── Models/
│   ├── Urun.cs                  # Ürün modeli
│   └── Kullanici.cs             # Kullanıcı modeli
│
├── Repositories/
│   ├── IRepository.cs           # Generic Repository Interface
│   ├── Repository.cs            # Generic Repository Implementation
│   ├── IUnitOfWork.cs           # Unit of Work Interface
│   ├── UnitOfWork.cs            # Unit of Work Implementation
│   └── TransactionExamples.cs   # Transaction kullanım örnekleri
│
├── Windows/
│   └── LoginWindow.xaml.cs      # Giriş ekranı
│
├── MainWindow.xaml.cs           # Ana ekran
└── App.xaml.cs                  # Uygulama başlangıcı (Veritabanı başlatma)
```

### 📄 Dosya Detayları

#### 1️⃣ **App.xaml.cs** (Uygulama Başlangıcı)
**Ne yapar?**
- Uygulama başladığında ilk çalışan kod
- Veritabanını kontrol eder, yoksa oluşturur
- Migration'ları uygular
- DbInitializer'ı çağırarak başlangıç verilerini ekler

**Eklenen Kodlar:**
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    using (var context = new AppDbContext())
    {
        // 1. Veritabanını oluştur
        context.Database.EnsureCreated();
        
        // 2. Migration'ları uygula
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
        
        // 3. Seed data ekle
        DbInitializer.Initialize(context);
    }
}
```

---

#### 2️⃣ **Data/DbInitializer.cs** (Başlangıç Verileri)
**Ne yapar?**
- İlk çalıştırmada admin kullanıcısı oluşturur
- Örnek ürünler ekler (isterseniz silebilirsiniz)

**Nasıl Değiştiririm?**
```csharp
// Varsayılan kullanıcıyı değiştirmek için:
var adminKullanici = new Kullanici
{
    KullaniciAdi = "admin",      // ← Burası
    Sifre = "admin123"           // ← Burası
};
```

---

#### 3️⃣ **Repositories/IRepository.cs** (Repository Interface)
**Ne yapar?**
- Tüm CRUD işlemlerini tanımlar
- Generic yapı sayesinde her entity için kullanılabilir

**Metodlar:**
- `GetById(id)` - ID'ye göre getir
- `GetAll()` - Tümünü getir
- `Find(predicate)` - Şarta göre filtrele
- `FirstOrDefault(predicate)` - İlk eşleşeni bul
- `Any(predicate)` - Var mı kontrolü
- `Add(entity)` - Ekle
- `Update(entity)` - Güncelle
- `Remove(entity)` - Sil

---

#### 4️⃣ **Repositories/Repository.cs** (Repository Implementation)
**Ne yapar?**
- IRepository interface'ini implemente eder
- Entity Framework Core ile çalışır
- Gerçek veritabanı işlemlerini yapar

**Örnek Kod:**
```csharp
public IEnumerable<T> GetAll()
{
    return _dbSet.ToList();
}

public void Add(T entity)
{
    _dbSet.Add(entity);
}
```

---

#### 5️⃣ **Repositories/IUnitOfWork.cs** (Unit of Work Interface)
**Ne yapar?**
- Tüm repository'leri bir arada tutar
- Transaction metodlarını tanımlar

**Özellikler:**
```csharp
IRepository<Urun> Urunler { get; }
IRepository<Kullanici> Kullanicilar { get; }

void BeginTransaction();  // Transaction başlat
void Commit();            // Kaydet
void Rollback();          // Geri al
int SaveChanges();        // Direkt kaydet (transaction'sız)
```

---

#### 6️⃣ **Repositories/UnitOfWork.cs** (Unit of Work Implementation)
**Ne yapar?**
- IUnitOfWork'ü implemente eder
- Transaction yönetimini sağlar
- Tüm repository'leri tek bir yerden yönetir

**Transaction Nasıl Çalışır?**
```csharp
// 1. Transaction başlat
_transaction = _context.Database.BeginTransaction();

// 2. İşlemler yapılır...

// 3a. Başarılıysa commit
_context.SaveChanges();
_transaction.Commit();

// 3b. Hata varsa rollback
_transaction.Rollback();
```

---

#### 7️⃣ **MainWindow.xaml.cs** (Ana Ekran)
**Ne Değişti?**

**Önceden:**
```csharp
private readonly AppDbContext _db = new AppDbContext();
var urunler = _db.Urunler.ToList();
_db.Urunler.Add(yeniUrun);
_db.SaveChanges();
```

**Şimdi:**
```csharp
private readonly IUnitOfWork _unitOfWork;
var urunler = _unitOfWork.Urunler.GetAll();
_unitOfWork.Urunler.Add(yeniUrun);
_unitOfWork.SaveChanges();
```

**Değişen Metodlar:**
- `Listele()` - Repository kullanıyor
- `BtnEkle_Click()` - Repository kullanıyor
- `BtnSatirKaydet_Click()` - Repository kullanıyor
- `BtnSil_Click()` - Repository kullanıyor
- `TxtArama_TextChanged()` - Repository kullanıyor

---

#### 8️⃣ **Windows/LoginWindow.xaml.cs** (Giriş Ekranı)
**Ne Değişti?**

**Önceden:**
```csharp
private readonly AppDbContext _db = new AppDbContext();
var user = _db.Kullanicilar.FirstOrDefault(x => ...);
```

**Şimdi:**
```csharp
private readonly IUnitOfWork _unitOfWork;
var user = _unitOfWork.Kullanicilar.FirstOrDefault(x => ...);
```

**Değişen Metodlar:**
- `Login_Click()` - Repository kullanıyor
- `Register_Click()` - Repository kullanıyor

---

#### 9️⃣ **Repositories/TransactionExamples.cs** (Örnek Kodlar)
**Ne yapar?**
- Transaction kullanımına dair hazır örnekler içerir
- Kopyala yapıştır kullanabilirsiniz

**İçindeki Örnekler:**
1. `TopluUrunEkle()` - Birden fazla ürün ekleme
2. `UrunGuncelleVeStokDusur()` - Stok düşürme
3. `KullaniciVeUrunEkle()` - İki farklı tablo işlemi
4. `BasitUrunEkle()` - Transaction'sız basit ekleme

---

## ⚠️ Önemli Notlar

### 0. İlk Çalıştırma
Projeyi ilk kez çalıştırdığınızda:
1. Veritabanı otomatik oluşacak
2. Migration'lar otomatik uygulanacak
3. Varsayılan admin kullanıcısı eklenecek (`admin` / `admin123`)
4. 2 adet örnek ürün eklenecek

**Giriş yapmak için:**
- Kullanıcı Adı: `admin`
- Şifre: `admin123`

Daha sonra "Kayıt Ol" butonuyla yeni kullanıcılar ekleyebilirsiniz.

### 1. Transaction Ne Zaman Kullanmalı?
- ✅ Birden fazla tablo üzerinde işlem yapılıyorsa
- ✅ İşlemler birbirine bağlıysa (biri başarısız olursa diğeri de iptal olmalı)
- ✅ Stok transfer, satış işlemleri gibi kritik işlemlerde
- ❌ Tek bir ekleme/güncelleme/silme işleminde GEREK YOK

### 2. SaveChanges vs Commit
- **SaveChanges()**: Transaction olmadan direkt kaydeder
- **Commit()**: Transaction içindeki tüm değişiklikleri kaydeder

### 3. Using Pattern
Her zaman `using` bloğu içinde kullanın, böylece kaynaklar otomatik temizlenir:
```csharp
using (var unitOfWork = new UnitOfWork(new AppDbContext()))
{
    // İşlemleriniz
} // Dispose otomatik çağrılır
```

### 4. Hata Yönetimi
Transaction kullanırken mutlaka `try-catch` bloğu kullanın ve `Rollback()` çağırın.

---

## 💡 Gerçek Dünya Örnekleri

### Senaryo 1: Ürün Satış İşlemi
Bir ürün satıldığında hem stok düşürecek hem de satış kaydı oluşturacaksınız.

```csharp
private void UrunSat(int urunId, int adet, string musteriAdi)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        try
        {
            // Transaction başlat - Ya her şey başarılı olacak ya hiçbiri
            unitOfWork.BeginTransaction();

            // 1. Ürünü bul
            var urun = unitOfWork.Urunler.GetById(urunId);
            if (urun == null)
                throw new Exception("Ürün bulunamadı!");

            // 2. Stok kontrolü
            if (urun.StokAdedi < adet)
                throw new Exception($"Yetersiz stok! Mevcut: {urun.StokAdedi}");

            // 3. Stoğu düş
            urun.StokAdedi -= adet;
            unitOfWork.Urunler.Update(urun);

            // 4. Satış kaydı oluştur (eğer Satis tablosu varsa)
            // var satis = new Satis 
            // { 
            //     UrunId = urunId, 
            //     Adet = adet,
            //     MusteriAdi = musteriAdi,
            //     Tarih = DateTime.Now
            // };
            // unitOfWork.Satislar.Add(satis);

            // 5. Her şey tamam, kaydet
            unitOfWork.Commit();

            MessageBox.Show($"Satış başarılı!\nÜrün: {urun.UrunAdi}\nKalan Stok: {urun.StokAdedi}");
        }
        catch (Exception ex)
        {
            // Hata olursa her şeyi geri al
            unitOfWork.Rollback();
            MessageBox.Show($"Satış başarısız: {ex.Message}");
        }
    }
}
```

### Senaryo 2: Toplu Ürün Güncelleme
Belirli bir kategorideki tüm ürünlerin fiyatını %10 artırma.

```csharp
private void KategoriFiyatGuncelle(string kategori, double artisYuzdesi)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        try
        {
            unitOfWork.BeginTransaction();

            // Kategorideki tüm ürünleri bul
            var urunler = unitOfWork.Urunler.Find(x => x.Kategori == kategori);
            
            int guncellenenSayi = 0;
            foreach (var urun in urunler)
            {
                urun.Fiyat = urun.Fiyat * (1 + artisYuzdesi / 100);
                unitOfWork.Urunler.Update(urun);
                guncellenenSayi++;
            }

            unitOfWork.Commit();
            MessageBox.Show($"{guncellenenSayi} adet ürünün fiyatı güncellendi!");
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            MessageBox.Show($"Güncelleme başarısız: {ex.Message}");
        }
    }
}
```

### Senaryo 3: Depolar Arası Stok Transferi
İki depo arasında ürün transferi (eğer çoklu depo sisteminiz varsa).

```csharp
private void StokTransfer(int urunId, int kaynakDepoId, int hedefDepoId, int adet)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        try
        {
            unitOfWork.BeginTransaction();

            // Kaynak depodan çıkart
            var kaynakStok = unitOfWork.DepoStoklar.FirstOrDefault(x => 
                x.UrunId == urunId && x.DepoId == kaynakDepoId);
            
            if (kaynakStok == null || kaynakStok.Miktar < adet)
                throw new Exception("Kaynak depoda yeterli stok yok!");

            kaynakStok.Miktar -= adet;
            unitOfWork.DepoStoklar.Update(kaynakStok);

            // Hedef depoya ekle
            var hedefStok = unitOfWork.DepoStoklar.FirstOrDefault(x => 
                x.UrunId == urunId && x.DepoId == hedefDepoId);
            
            if (hedefStok == null)
            {
                hedefStok = new DepoStok 
                { 
                    UrunId = urunId, 
                    DepoId = hedefDepoId, 
                    Miktar = adet 
                };
                unitOfWork.DepoStoklar.Add(hedefStok);
            }
            else
            {
                hedefStok.Miktar += adet;
                unitOfWork.DepoStoklar.Update(hedefStok);
            }

            unitOfWork.Commit();
            MessageBox.Show("Transfer başarılı!");
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            MessageBox.Show($"Transfer başarısız: {ex.Message}");
        }
    }
}
```

### Senaryo 4: Barkod ile Ürün Arama ve Stok Güncelleme
Barkod okutucudan gelen veriye göre stok güncelleme.

```csharp
private void BarkodOkut(string barkod, int eklenecekStok)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        try
        {
            // Barkoda göre ürün bul
            var urun = unitOfWork.Urunler.FirstOrDefault(x => x.Barkod == barkod);
            
            if (urun == null)
            {
                MessageBox.Show($"Barkod bulunamadı: {barkod}");
                return;
            }

            // Stoğu güncelle
            urun.StokAdedi += eklenecekStok;
            unitOfWork.Urunler.Update(urun);
            unitOfWork.SaveChanges(); // Transaction gerekmez, tek işlem

            MessageBox.Show($"{urun.UrunAdi}\nYeni Stok: {urun.StokAdedi}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata: {ex.Message}");
        }
    }
}
```

### Senaryo 5: Düşük Stoklu Ürünleri Raporlama
Stok adedi belirli bir değerin altında olan ürünleri listeleme.

```csharp
private List<Urun> DusukStokluUrunler(int minStok = 10)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        // Stok adedi düşük ürünleri bul
        var dusukStokluUrunler = unitOfWork.Urunler
            .Find(x => x.StokAdedi < minStok)
            .OrderBy(x => x.StokAdedi)
            .ToList();

        if (dusukStokluUrunler.Any())
        {
            string mesaj = "DÜŞÜK STOKLU ÜRÜNLER:\n\n";
            foreach (var urun in dusukStokluUrunler)
            {
                mesaj += $"• {urun.UrunAdi} - Stok: {urun.StokAdedi}\n";
            }
            MessageBox.Show(mesaj);
        }

        return dusukStokluUrunler;
    }
}
```

### Senaryo 6: Kullanıcı Kaydı + İlk Ürün Ekleme
Yeni kullanıcı kayıt olduğunda ona özel bir karşılama ürünü ekleme.

```csharp
private void YeniKullaniciKaydet(string kullaniciAdi, string sifre)
{
    using (var unitOfWork = new UnitOfWork(new AppDbContext()))
    {
        try
        {
            unitOfWork.BeginTransaction();

            // 1. Kullanıcıyı kaydet
            var yeniKullanici = new Kullanici
            {
                KullaniciAdi = kullaniciAdi,
                Sifre = sifre // Gerçek projede hash'leyin!
            };
            unitOfWork.Kullanicilar.Add(yeniKullanici);

            // 2. Karşılama ürünü ekle
            var karsilamaUrunu = new Urun
            {
                UrunAdi = $"{kullaniciAdi}'nin İlk Ürünü",
                StokAdedi = 1,
                Fiyat = 0,
                Barkod = $"WELCOME-{DateTime.Now.Ticks}",
                Kategori = "Karşılama"
            };
            unitOfWork.Urunler.Add(karsilamaUrunu);

            // 3. Her ikisini de kaydet
            unitOfWork.Commit();

            MessageBox.Show($"Hoş geldiniz {kullaniciAdi}!\nİlk ürününüz oluşturuldu.");
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            MessageBox.Show($"Kayıt başarısız: {ex.Message}");
        }
    }
}
```

---

## ❓ Sık Sorulan Sorular (SSS)

### 1. Veritabanı oluşmuyor, hata alıyorum?
**Çözüm:**
- SQL Server Express'in çalıştığından emin olun
- Bağlantı stringini kontrol edin (`AppDbContext.cs`)
- Visual Studio'yu yönetici olarak çalıştırın

### 2. Admin kullanıcısı oluşmuyor?
**Çözüm:**
- Veritabanını silin ve tekrar çalıştırın
- `DbInitializer.cs` dosyasını kontrol edin
- `App.xaml.cs` içinde `DbInitializer.Initialize()` çağrısını kontrol edin

### 3. Transaction ne zaman kullanmalıyım?
**Cevap:**
- Birden fazla tabloya yazma işlemi yapıyorsanız ✅
- Stok transfer, satış gibi kritik işlemlerde ✅
- Tek bir ekleme/güncelleme/silme işleminde ❌

### 4. SaveChanges() ile Commit() arasındaki fark nedir?
**Cevap:**
- **SaveChanges()**: Transaction olmadan direkt kaydeder
- **Commit()**: Transaction içindeki tüm değişiklikleri kaydeder

### 5. Eski kodlar çalışmaz mı artık?
**Cevap:**
- Hayır, eski kodlar değişti
- Artık `_db.Urunler` yerine `_unitOfWork.Urunler` kullanılıyor
- MainWindow ve LoginWindow güncellenmiş durumda

### 6. Kendi repository'mi nasıl eklerim?
**Örnek:**
```csharp
// 1. Model oluştur
public class Satis { ... }

// 2. DbContext'e ekle
public DbSet<Satis> Satislar { get; set; }

// 3. IUnitOfWork'e ekle
IRepository<Satis> Satislar { get; }

// 4. UnitOfWork'e ekle
private IRepository<Satis>? _satislar;
public IRepository<Satis> Satislar
{
    get
    {
        if (_satislar == null)
            _satislar = new Repository<Satis>(_context);
        return _satislar;
    }
}

// 5. Migration oluştur
// Package Manager Console'da:
// Add-Migration SatisEklendi
// Update-Database
```

### 7. Async metodlar yok mu?
**Cevap:**
Şu an senkron metodlar kullanılıyor. İsterseniz eklenebilir:
```csharp
// IRepository'ye ekle:
Task<IEnumerable<T>> GetAllAsync();
Task<T?> GetByIdAsync(int id);

// Repository'ye implemente et:
public async Task<IEnumerable<T>> GetAllAsync()
{
    return await _dbSet.ToListAsync();
}
```

### 8. Test nasıl yazılır?
**Örnek:**
```csharp
// Mock repository oluştur
var mockRepo = new Mock<IRepository<Urun>>();
mockRepo.Setup(r => r.GetAll()).Returns(fakeUrunList);

// Mock unit of work
var mockUow = new Mock<IUnitOfWork>();
mockUow.Setup(u => u.Urunler).Returns(mockRepo.Object);

// Test et
var result = mockUow.Object.Urunler.GetAll();
Assert.Equal(2, result.Count());
```

---

## 🐛 Sorun Giderme

### "Cannot open database" hatası
```
Hata: Cannot open database "DepoEnvanterDB" requested by the login.
```
**Çözüm:**
1. SQL Server Express'in çalıştığından emin olun
2. Connection string'i kontrol edin
3. Veritabanını manuel oluşturun ya da `EnsureCreated()` çalıştığından emin olun

### "A network-related or instance-specific error" hatası
```
Hata: A network-related or instance-specific error occurred...
```
**Çözüm:**
1. SQL Server Configuration Manager'ı açın
2. SQL Server Network Configuration → Protocols for SQLEXPRESS
3. TCP/IP'yi enable edin
4. SQL Server servisi restart edin

### Migration uygulanmıyor
**Çözüm:**
Package Manager Console'da manuel çalıştırın:
```powershell
Update-Database
```

### Transaction rollback çalışmıyor
**Kontrol Et:**
```csharp
try
{
    unitOfWork.BeginTransaction();
    // işlemler
    unitOfWork.Commit();
}
catch (Exception ex)
{
    unitOfWork.Rollback(); // ← Bunu eklemeyi unutmayın!
    throw;
}
```

---

## 🎯 Sonraki Adımlar (Opsiyonel İyileştirmeler)

### Kolay Seviye
1. **Varsayılan kullanıcıyı değiştir** - `DbInitializer.cs` dosyasını düzenle
2. **Örnek ürünleri kaldır** - `DbInitializer.cs` içindeki örnek ürünleri sil
3. **Connection string'i appsettings'e taşı** - Hard-coded connection string yerine config dosyası kullan

### Orta Seviye
1. **Şifre Hash'leme** - BCrypt veya ASP.NET Identity kullan
2. **Async/Await** - Asenkron metodlar ekle
3. **Validation** - Model validation ile veri doğrulama
4. **Logging** - Serilog veya NLog ile loglama

### İleri Seviye
1. **Dependency Injection** - Constructor injection kullan
2. **CQRS Pattern** - Command ve Query'leri ayır
3. **AutoMapper** - DTO'ları otomatik map'le
4. **Unit Testing** - xUnit ile test yazımı
5. **API Layer** - Web API ekle

---

## 📚 Kaynaklar ve Öğrenme Materyalleri

### Repository Pattern
- [Microsoft Docs - Repository Pattern](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
- [Martin Fowler - Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)

### Unit of Work Pattern
- [Microsoft Docs - Unit of Work](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#the-unit-of-work-pattern)

### Entity Framework Core
- [EF Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Transactions](https://docs.microsoft.com/en-us/ef/core/saving/transactions)

---

## 📞 Destek

Sorularınız için projeyi geliştiren kişiye ulaşabilirsiniz.

---

## 📝 Değişiklik Geçmişi

### Versiyon 1.0 (4 Ocak 2025)
- ✅ Repository Pattern eklendi
- ✅ Unit of Work Pattern eklendi
- ✅ Transaction desteği eklendi
- ✅ Veritabanı otomatik kurulum
- ✅ Varsayılan kullanıcı oluşturma
- ✅ MainWindow ve LoginWindow refactor edildi

---

## 📊 Özet: Önce vs Sonra

### Önce ❌
```csharp
// Her yerde DbContext kullanımı
private readonly AppDbContext _db = new AppDbContext();

// İşlemler dağınık
var urunler = _db.Urunler.ToList();
_db.Urunler.Add(yeniUrun);
_db.SaveChanges();

// Transaction yok
// Hata oluşsa bile bazı değişiklikler kaydedilebiliyordu

// Veritabanı manuel kurulum
// Update-Database komutu gerekiyordu

// Test edilemez
// Mock'lama zordu
```

### Sonra ✅
```csharp
// Temiz mimari
private readonly IUnitOfWork _unitOfWork;

// Repository pattern
var urunler = _unitOfWork.Urunler.GetAll();
_unitOfWork.Urunler.Add(yeniUrun);
_unitOfWork.SaveChanges();

// Transaction var
unitOfWork.BeginTransaction();
// İşlemler...
unitOfWork.Commit(); // Ya hepsi ya hiçbiri

// Otomatik kurulum
// Uygulama açılır açılmaz hazır

// Test edilebilir
// Interface'ler sayesinde kolayca mock'lanabilir
```

---

## 🎓 Öğrenilenler

Bu projede şunları öğrendiniz:
- ✅ Repository Pattern nedir ve nasıl uygulanır
- ✅ Unit of Work Pattern ile transaction yönetimi
- ✅ Generic repository nasıl oluşturulur
- ✅ Entity Framework Core ile veritabanı işlemleri
- ✅ SOLID prensipleri (özellikle Dependency Inversion)
- ✅ Temiz mimari tasarımı
- ✅ Transaction ile veri tutarlılığı

---

## 🚀 Başarıyla Tamamlandı!

Artık projenizde:
- ✅ Modern mimari var
- ✅ Transaction desteği var
- ✅ Test edilebilir kod var
- ✅ Otomatik veritabanı kurulumu var
- ✅ Temiz ve bakımı kolay kod var

**İyi çalışmalar!** 🎉

---

**Hazırlayan:** AI Asistan  
**Tarih:** 4 Ocak 2025  
**Versiyon:** 1.0  
**Toplam Sayfa:** 600+ satır detaylı dokümantasyon

