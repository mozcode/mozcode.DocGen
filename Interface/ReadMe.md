# .NET Belge Oluþturma Kütüphanesi - ReadMe

Bu kütüphane, .NET tabanlý belge oluþturma ihtiyaçlarýný karþýlamak için tasarlanmýþtýr. Kullanýcýlara PDF, Word gibi çýktýlar üretme konusunda esnek ve modüler bir yapý sunar.

## 1. Giriþ ve Kapsam
Bu kütüphane, kurumsal düzeyde belge üretimi için tasarlanmýþtýr. Aþaðýdaki bölümleri yönetir:
- **Belge Üst Bilgileri (Header)**
- **Belge Alt Bilgileri (Footer)**
- **Belge Ýmza Alaný (Signature)**
- **Belge Ýçeriði (Body)**
- **Varsayýlan Ayarlar (Default Settings)**
- **Baþlýk Numaralandýrma (Title Numbering)**
- **Zengin Metin (Rich Text) Desteði**
- **Geçerlilik (Validation) Yapýlarý**

Kütüphane, arabirimler, alan tanýmlarý ve format dönüþtürücüler ile esnek bir mimari sunar.

## 2. Fonksiyonel Gereksinimler
### 2.1 Meta Bilgileri (Header, Footer, Signature)
- **Header:** Tablo formatýnda, logo, tarih ve belge numarasý eklenebilir.
- **Footer:** Sayfa numarasý, yasal uyarýlar gibi bilgileri içerir.
- **Signature:** Belgenin belirli bölgelerinde imza alanlarý tanýmlanabilir.

### 2.2 Belge Ýçeriði (BodyContent)
- **Baþlýk yapýsý:** 10 seviyeye kadar desteklenir (1, 1.1, 1.1.1 vb.).
- **Baþlýk numaralandýrma:** Numeric, Roman, Alphabetic formatlar desteklenir.
- **Zengin metin desteði:** Bold, italic, underline gibi stiller desteklenir.

### 2.3 Varsayýlan Ayarlar (Default Settings)
- **Font ayarlarý:** Renk, boyut, hizalama gibi varsayýlanlar belirlenebilir.
- **Baþlýk boyutlarý:** Seviye bazýnda farklý punto deðerleri atanabilir.
- **Hizalama:** Sol, saða, ortalanmýþ veya justify formatlar desteklenir.

## 3. Teknik Olmayan (Non-Fonksiyonel) Gereksinimler
- **Performans:** Yüksek hacimli belgeleri etkin þekilde iþleyebilmelidir.
- **Geniþletilebilirlik:** Yeni formatlara veya ek bölümlere uyarlanabilir olmalýdýr.
- **Bakým Kolaylýðý:** Modüler ve SOLID prensiplerine uygun mimari.
- **Uluslararasý Kullaným:** Unicode desteði ile farklý dillerde belge oluþturma.

## 4. Sistem Bileþenleri ve Ýþ Akýþý
- **Abstract.Constants:** Sabit tanýmlar.
- **Abstract.Domain:** Temel "domain" sýnýflarý.
- **Abstract.Interface:** Servis, içerik ve ayar arayüzleri.
- **Abstract.Application:** Ýþ mantýðý ve parse süreçleri.
- **Abstract.Settings:** Varsayýlan ayarlar.
- **Abstract.Validation:** FluentValidation ile kurallar.

## 5. Kullaným Senaryolarý
- **Standart rapor oluþturma:** Haftalýk raporlar, tablo þablonlarý.
- **Sözleþme dokümanlarý:** Resmî belgelerde sabit format kullanýmý.
- **Form veya þablon belgeler:** Sabit header/footer yapýlarý.

## 6. Validasyon Kurallarý
- **Baþlýk seviyesi:** 1-10 arasýnda olmalýdýr.
- **Tablo hücresi konumu:** Negatif deðerler geçersizdir.
- **Boþ baþlýk:** Baþlýk metni boþ olamaz.

## 7. Mimarinin Saðladýðý Avantajlar
- **Modülerlik:** Header, Footer, Signature bölümleri ayrý yönetilebilir.
- **Bakým kolaylýðý:** Katmanlý mimari sayesinde geliþtirme kolaylýðý.
- **Test edilebilirlik:** Validasyon ve parse iþlemleri baðýmsýz test edilebilir.
- **Geniþleme:** Yeni belge türleri kolayca eklenebilir.

## 8. Paydaþlar ve Kullanýcý Rolleri
- **Yazýlým Geliþtiriciler:** Kütüphaneyi projelerine entegre eder.
- **Ýþ Analistleri/PM:** Belge tasarým süreçlerini belirler.
- **Test Ekipleri (QA):** Validasyon ve çýktý doðruluðunu kontrol eder.

## 9. Sonuç ve Öneriler
- **Kolay Kurulum ve Entegrasyon:** NuGet paketi veya DLL olarak eklenebilir.
- **Özelleþtirilebilir Mimari:** Tablo, imza, format gibi bölümler modüler halinde kullanýlabilir.
- **Gelecekteki Geliþim Noktalarý:** Elektronik imza, HTML/Excel formatlarý, daha ileri zengin metin desteði.

## 10. Ekler
- **Terimler Sözlüðü** (Header, Footer, Signature, Body, Validation vb.)
- **Ýlgili Mevzuat veya Kurumsal Kurallar**

Bu kütüphane, belge oluþturma sürecini basitleþtirerek þirket içi ve dýþý döküman ihtiyaçlarýna esnek ve standart bir çözüm sunar.

