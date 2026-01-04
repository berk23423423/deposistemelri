# Repository Pattern & Transaction Kullanım Kılavuzu

## 📑 İçindekiler

1. [Hızlı Başlangıç](#-hızlı-başlangıç)
2. [Proje Analizi - Neler Değişti?](#-proje-analizi---neler-değişti)
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

