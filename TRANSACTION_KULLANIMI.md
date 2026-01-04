# 🔄 Transaction Kullanım Örnekleri - Mevcut Projede

## ✅ Transaction Altyapısı Hazır!

Projede **Unit of Work** pattern ile transaction desteği **HAZIR** durumda!

**Mevcut durumda:**
- ✅ `UnitOfWork.cs` - Transaction metodları var
- ✅ `BeginTransaction()`, `Commit()`, `Rollback()` çalışır
- ⚠️ Ama MainWindow ve LoginWindow'da **kullanılmıyor**
- ℹ️ Tek işlem için transaction gerekmez

---

## 🔍 Şu Anki Kod Nasıl Çalışıyor?

### MainWindow.xaml.cs - Ürün Ekleme

```csharp
private void BtnEkle_Click(object sender, RoutedEventArgs e)
{
    // ... validasyon ...
    
    var yeniUrun = new Urun { /* ... */ };
    
    // Transaction YOK - Direkt kaydet
    _unitOfWork.Urunler.Add(yeniUrun);
    _unitOfWork.SaveChanges(); // ← Hata olursa EF Core zaten rollback yapar
    
    Listele();
    FormuTemizle();
}
```

**Sorun var mı?** 
- ❌ HAYIR! Tek bir işlem için transaction **GEREK YOK**
- ✅ Entity Framework Core zaten otomatik rollback yapıyor

---

## 🎯 Transaction Ne Zaman Gerekli?

### ❌ Transaction GEREK YOK:
```
- Tek bir ürün ekleme
- Tek bir ürün güncelleme
- Tek bir ürün silme
- Kullanıcı kaydı
- Login işlemi
```

### ✅ Transaction GEREKLİ:
```
- Birden fazla ürün ekleme (hepsi ya da hiçbiri)
- Stok transfer (bir üründen azalt, diğerine ekle)
- Satış işlemi (stok düş + satış kaydı ekle)
- Toplu güncelleme
- İlişkili tablolara yazma
```

---

## 💡 Transaction Nasıl Eklenir?

### Örnek 1: Toplu Ürün Ekleme (Transaction İLE)

MainWindow.xaml.cs'ye yeni bir metod ekle:

```csharp
// Toplu ürün ekleme butonu için metod
private void BtnTopluEkle_Click(object sender, RoutedEventArgs e)
{
    try
    {
        // Transaction başlat
        _unitOfWork.BeginTransaction();

        // 3 ürün ekleyelim
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

        var urun3 = new Urun
        {
            UrunAdi = "Klavye",
            StokAdedi = 30,
            Fiyat = 500,
            Barkod = "KEY001",
            Kategori = "Elektronik"
        };

        _unitOfWork.Urunler.Add(urun1);
        _unitOfWork.Urunler.Add(urun2);
        _unitOfWork.Urunler.Add(urun3);

        // Hepsi başarılıysa commit et
        _unitOfWork.Commit();

        MessageBox.Show("3 ürün başarıyla eklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        Listele();
    }
    catch (Exception ex)
    {
        // HATA OLURSA BURAYA DÜŞER
        // Otomatik rollback yapılır
        _unitOfWork.Rollback();
        
        MessageBox.Show(
            $"Hata oluştu, hiçbir ürün eklenmedi!\n\n{ex.Message}",
            "Hata",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
```

**Ne Olur?**
- ✅ 3 ürün de eklenirse → Commit → Hepsi kaydedilir
- ❌ Herhangi biri hata verirse → Rollback → Hiçbiri eklenmez

---

### Örnek 2: Stok Transfer İşlemi (Transaction İLE)

```csharp
// İki ürün arasında stok transferi
private void StokTransfer(int kaynakUrunId, int hedefUrunId, int miktar)
{
    try
    {
        _unitOfWork.BeginTransaction();

        // Kaynak üründen stok düş
        var kaynakUrun = _unitOfWork.Urunler.GetById(kaynakUrunId);
        if (kaynakUrun == null)
            throw new Exception("Kaynak ürün bulunamadı!");

        if (kaynakUrun.StokAdedi < miktar)
            throw new Exception($"Yetersiz stok! Mevcut: {kaynakUrun.StokAdedi}");

        kaynakUrun.StokAdedi -= miktar;
        _unitOfWork.Urunler.Update(kaynakUrun);

        // Hedef ürüne stok ekle
        var hedefUrun = _unitOfWork.Urunler.GetById(hedefUrunId);
        if (hedefUrun == null)
            throw new Exception("Hedef ürün bulunamadı!");

        hedefUrun.StokAdedi += miktar;
        _unitOfWork.Urunler.Update(hedefUrun);

        // Her iki işlem de başarılıysa commit
        _unitOfWork.Commit();

        MessageBox.Show(
            $"Transfer başarılı!\n{kaynakUrun.UrunAdi} → {hedefUrun.UrunAdi}\nMiktar: {miktar}",
            "Başarılı",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        Listele();
    }
    catch (Exception ex)
    {
        // Hata olursa her iki işlem de geri alınır
        _unitOfWork.Rollback();
        
        MessageBox.Show(
            $"Transfer başarısız!\n\n{ex.Message}",
            "Hata",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
```

**Ne Olur?**
- ✅ İkisi de başarılıysa → Commit → Transfer tamamlanır
- ❌ Biri başarısızsa → Rollback → Hiçbir değişiklik yapılmaz

---

### Örnek 3: Ürün Silme + İlişkili Kayıtları Silme

```csharp
// Ürünü ve ilişkili kayıtları sil (örn: satış kayıtları varsa)
private void UrunVeKayitlariniSil(int urunId)
{
    try
    {
        _unitOfWork.BeginTransaction();

        // Ürünü bul
        var urun = _unitOfWork.Urunler.GetById(urunId);
        if (urun == null)
            throw new Exception("Ürün bulunamadı!");

        // Önce ilişkili kayıtları sil (eğer başka tablolar varsa)
        // Örnek: Satış kayıtları, stok hareketleri vs.
        // var satislar = _unitOfWork.Satislar.Find(x => x.UrunId == urunId);
        // _unitOfWork.Satislar.RemoveRange(satislar);

        // Sonra ürünü sil
        _unitOfWork.Urunler.Remove(urun);

        // Her şey başarılıysa commit
        _unitOfWork.Commit();

        MessageBox.Show("Ürün ve ilişkili kayıtlar silindi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        Listele();
    }
    catch (Exception ex)
    {
        _unitOfWork.Rollback();
        MessageBox.Show($"Silme başarısız!\n\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
```

---

## 🔄 Mevcut Ürün Ekleme Metodunu Transaction'lı Yapsak?

### ÖNCE (Şu anki kod):

```csharp
private void BtnEkle_Click(object sender, RoutedEventArgs e)
{
    if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
    {
        MessageBox.Show("Lütfen ürün adı giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

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

    _unitOfWork.Urunler.Add(yeniUrun);
    _unitOfWork.SaveChanges(); // ← Transaction YOK
    
    Listele();
    FormuTemizle();
}
```

### SONRA (Transaction ile):

```csharp
private void BtnEkle_Click(object sender, RoutedEventArgs e)
{
    if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
    {
        MessageBox.Show("Lütfen ürün adı giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    try
    {
        // Transaction başlat
        _unitOfWork.BeginTransaction();

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

        _unitOfWork.Urunler.Add(yeniUrun);
        
        // Commit et
        _unitOfWork.Commit();
        
        MessageBox.Show("Ürün başarıyla eklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        Listele();
        FormuTemizle();
    }
    catch (Exception ex)
    {
        // Hata olursa rollback
        _unitOfWork.Rollback();
        MessageBox.Show($"Ürün eklenirken hata oluştu!\n\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
```

**Ama dikkat:** ⚠️
- Tek bir ürün için transaction **GEREKSIZ**
- Entity Framework zaten hata durumunda rollback yapıyor
- Transaction kullanmak **performansı biraz düşürür**
- Sadece **birden fazla işlem** için mantıklı

---

## 📊 Transaction Kullan/Kullanma Kararı

| İşlem | Transaction Gerekli mi? | Neden? |
|-------|------------------------|--------|
| Tek ürün ekleme | ❌ Hayır | EF Core zaten rollback yapıyor |
| Tek ürün silme | ❌ Hayır | Tek işlem |
| Tek ürün güncelleme | ❌ Hayır | Tek işlem |
| 3 ürün birden ekleme | ✅ Evet | Ya hepsi ya hiçbiri mantığı |
| Stok transfer | ✅ Evet | 2 ürün etkileniyor |
| Ürün + Stok kaydı | ✅ Evet | 2 tablo etkileniyor |
| Toplu fiyat güncelleme | ✅ Evet | Çok sayıda ürün |

---

## 🎯 Sonuç

**Mevcut kodda transaction var mı?**
- ✅ Altyapı VAR (`UnitOfWork.cs`)
- ❌ Kullanılmıyor (tek işlemler için gerek yok)
- ✅ Rollback mekanizması HAZIR
- ℹ️ İhtiyaç olursa kolayca eklenebilir

**Eklememeli miyiz?**
- Tek ürün işlemleri için → ❌ Gerek yok
- Toplu işlemler için → ✅ Eklenebilir

**Nasıl ekleriz?**
- Yukarıdaki örnekleri kopyala
- MainWindow.xaml.cs'ye ekle
- UI'da buton ekle (opsiyonel)

---

## 📚 Daha Fazla Örnek

`TransactionExamples.cs` dosyasında 4 hazır örnek var:
1. `TopluUrunEkle()` - Birden fazla ürün
2. `UrunGuncelleVeStokDusur()` - Stok düşürme
3. `KullaniciVeUrunEkle()` - İki farklı tablo
4. `BasitUrunEkle()` - Transaction'sız

---

**Hazırlayan:** AI Asistan  
**Tarih:** 4 Ocak 2025  
**Versiyon:** 1.0

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║  Transaction altyapısı hazır! 💪                         ║
║  İhtiyacın olduğunda kullanabilirsin.                    ║
║                                                           ║
║  Detaylı örnekler:                                       ║
║  📚 REPOSITORY_PATTERN_KULLANIMI.md                      ║
║  📁 Repositories/TransactionExamples.cs                  ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

