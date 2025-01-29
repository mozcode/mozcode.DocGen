# .NET Belge Olu�turma K�t�phanesi - ReadMe

Bu k�t�phane, .NET tabanl� belge olu�turma ihtiya�lar�n� kar��lamak i�in tasarlanm��t�r. Kullan�c�lara PDF, Word gibi ��kt�lar �retme konusunda esnek ve mod�ler bir yap� sunar.

## 1. Giri� ve Kapsam
Bu k�t�phane, kurumsal d�zeyde belge �retimi i�in tasarlanm��t�r. A�a��daki b�l�mleri y�netir:
- **Belge �st Bilgileri (Header)**
- **Belge Alt Bilgileri (Footer)**
- **Belge �mza Alan� (Signature)**
- **Belge ��eri�i (Body)**
- **Varsay�lan Ayarlar (Default Settings)**
- **Ba�l�k Numaraland�rma (Title Numbering)**
- **Zengin Metin (Rich Text) Deste�i**
- **Ge�erlilik (Validation) Yap�lar�**

K�t�phane, arabirimler, alan tan�mlar� ve format d�n��t�r�c�ler ile esnek bir mimari sunar.

## 2. Fonksiyonel Gereksinimler
### 2.1 Meta Bilgileri (Header, Footer, Signature)
- **Header:** Tablo format�nda, logo, tarih ve belge numaras� eklenebilir.
- **Footer:** Sayfa numaras�, yasal uyar�lar gibi bilgileri i�erir.
- **Signature:** Belgenin belirli b�lgelerinde imza alanlar� tan�mlanabilir.

### 2.2 Belge ��eri�i (BodyContent)
- **Ba�l�k yap�s�:** 10 seviyeye kadar desteklenir (1, 1.1, 1.1.1 vb.).
- **Ba�l�k numaraland�rma:** Numeric, Roman, Alphabetic formatlar desteklenir.
- **Zengin metin deste�i:** Bold, italic, underline gibi stiller desteklenir.

### 2.3 Varsay�lan Ayarlar (Default Settings)
- **Font ayarlar�:** Renk, boyut, hizalama gibi varsay�lanlar belirlenebilir.
- **Ba�l�k boyutlar�:** Seviye baz�nda farkl� punto de�erleri atanabilir.
- **Hizalama:** Sol, sa�a, ortalanm�� veya justify formatlar desteklenir.

## 3. Teknik Olmayan (Non-Fonksiyonel) Gereksinimler
- **Performans:** Y�ksek hacimli belgeleri etkin �ekilde i�leyebilmelidir.
- **Geni�letilebilirlik:** Yeni formatlara veya ek b�l�mlere uyarlanabilir olmal�d�r.
- **Bak�m Kolayl���:** Mod�ler ve SOLID prensiplerine uygun mimari.
- **Uluslararas� Kullan�m:** Unicode deste�i ile farkl� dillerde belge olu�turma.

## 4. Sistem Bile�enleri ve �� Ak���
- **Abstract.Constants:** Sabit tan�mlar.
- **Abstract.Domain:** Temel "domain" s�n�flar�.
- **Abstract.Interface:** Servis, i�erik ve ayar aray�zleri.
- **Abstract.Application:** �� mant��� ve parse s�re�leri.
- **Abstract.Settings:** Varsay�lan ayarlar.
- **Abstract.Validation:** FluentValidation ile kurallar.

## 5. Kullan�m Senaryolar�
- **Standart rapor olu�turma:** Haftal�k raporlar, tablo �ablonlar�.
- **S�zle�me dok�manlar�:** Resm� belgelerde sabit format kullan�m�.
- **Form veya �ablon belgeler:** Sabit header/footer yap�lar�.

## 6. Validasyon Kurallar�
- **Ba�l�k seviyesi:** 1-10 aras�nda olmal�d�r.
- **Tablo h�cresi konumu:** Negatif de�erler ge�ersizdir.
- **Bo� ba�l�k:** Ba�l�k metni bo� olamaz.

## 7. Mimarinin Sa�lad��� Avantajlar
- **Mod�lerlik:** Header, Footer, Signature b�l�mleri ayr� y�netilebilir.
- **Bak�m kolayl���:** Katmanl� mimari sayesinde geli�tirme kolayl���.
- **Test edilebilirlik:** Validasyon ve parse i�lemleri ba��ms�z test edilebilir.
- **Geni�leme:** Yeni belge t�rleri kolayca eklenebilir.

## 8. Payda�lar ve Kullan�c� Rolleri
- **Yaz�l�m Geli�tiriciler:** K�t�phaneyi projelerine entegre eder.
- **�� Analistleri/PM:** Belge tasar�m s�re�lerini belirler.
- **Test Ekipleri (QA):** Validasyon ve ��kt� do�rulu�unu kontrol eder.

## 9. Sonu� ve �neriler
- **Kolay Kurulum ve Entegrasyon:** NuGet paketi veya DLL olarak eklenebilir.
- **�zelle�tirilebilir Mimari:** Tablo, imza, format gibi b�l�mler mod�ler halinde kullan�labilir.
- **Gelecekteki Geli�im Noktalar�:** Elektronik imza, HTML/Excel formatlar�, daha ileri zengin metin deste�i.

## 10. Ekler
- **Terimler S�zl���** (Header, Footer, Signature, Body, Validation vb.)
- **�lgili Mevzuat veya Kurumsal Kurallar**

Bu k�t�phane, belge olu�turma s�recini basitle�tirerek �irket i�i ve d��� d�k�man ihtiya�lar�na esnek ve standart bir ��z�m sunar.

