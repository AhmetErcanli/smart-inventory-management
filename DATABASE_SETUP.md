# Veritabanı Kurulum Rehberi

## SQL Server Instance'ınızı Bulma

SSMS'i açın ve bağlanırken göreceğiniz server adını not edin.

### Seçenek 1: SQL Server Express (En Yaygın)
Eğer SSMS'te server adı şöyle görünüyorsa:
- `localhost\SQLEXPRESS`
- `.\SQLEXPRESS`
- `YOUR_COMPUTER_NAME\SQLEXPRESS`

O zaman `appsettings.json` dosyasında şu ayarı kullanın:
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

### Seçenek 2: Default SQL Server Instance
Eğer SSMS'te server adı sadece şöyle görünüyorsa:
- `localhost`
- `YOUR_COMPUTER_NAME`
- `.`

O zaman `appsettings.json` dosyasında şu ayarı kullanın:
```json
"DefaultConnection": "Server=localhost;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

### Seçenek 3: Named Instance (Özel İsim)
Eğer özel bir instance adınız varsa (örn: `localhost\MYINSTANCE`):
```json
"DefaultConnection": "Server=localhost\\MYINSTANCE;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

## SQL Server Servisinin Çalıştığını Kontrol Etme

1. **Windows Servisleri ile:**
   - Windows + R tuşlarına basın
   - `services.msc` yazın ve Enter'a basın
   - "SQL Server (SQLEXPRESS)" veya "SQL Server (MSSQLSERVER)" servisinin "Çalışıyor" durumunda olduğunu kontrol edin
   - Eğer durdurulmuşsa, sağ tıklayıp "Başlat" deyin

2. **SSMS ile:**
   - SSMS'i açın
   - Server adınızı yazıp "Connect" butonuna basın
   - Bağlanabiliyorsanız servis çalışıyor demektir

## Veritabanı Otomatik Oluşturulacak

Uygulamayı çalıştırdığınızda:
- `SmartInventoryDb` veritabanı otomatik oluşturulur
- Tüm tablolar otomatik oluşturulur
- Hiçbir şey manuel yapmanıza gerek yok!

## Test Etme

Uygulamayı çalıştırın:
```powershell
cd src/SmartInventory.WebApi
dotnet run
```

Eğer hata alırsanız, hata mesajını kontrol edin. Genellikle:
- "Cannot open database" → Server adı yanlış
- "Login failed" → Authentication sorunu
- "Server not found" → SQL Server çalışmıyor

