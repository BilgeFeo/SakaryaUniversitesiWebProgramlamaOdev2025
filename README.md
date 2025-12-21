# 🏋️‍♂️ AI Destekli Fitness Yönetim ve Koçluk Sistemi

Bu proje, spor salonları, antrenörler ve üyeler arasındaki etkileşimi yöneten, aynı zamanda **Google Gemini** ve **Stable Diffusion** teknolojilerini kullanarak üyelerine kişiselleştirilmiş yapay zeka deneyimi sunan kapsamlı bir **ASP.NET Core MVC** web uygulamasıdır.

![Status](https://img.shields.io/badge/Status-Active-success)
![Framework](https://img.shields.io/badge/.NET_Core-MVC-purple)
![AI Powered](https://img.shields.io/badge/AI-Gemini_%26_StableDiffusion-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## 📋 Proje Hakkında

Klasik spor salonu yönetim sistemlerinin ötesine geçen bu uygulama, "N-Tier Architecture" (Katmanlı Mimari) prensipleriyle geliştirilmiştir. Üyeler randevu alabilir, profillerini yönetebilir ve en önemlisi; fiziksel özelliklerine göre AI tarafından oluşturulan özel beslenme/antrenman programlarına erişebilirler.

### 🌟 Öne Çıkan Özellikler

#### 🤖 Yapay Zeka Entegrasyonları
* **AI Kişisel Koç (Google Gemini):** Kullanıcının yaşı, boyu, kilosu, cinsiyeti ve hedeflerine (Kilo Verme, Kas Yapma vb.) göre anlık olarak **Kişiye Özel Beslenme ve Antrenman Programı** oluşturur.
* **AI Vücut Simülasyonu (Hugging Face / Stable Diffusion):** Kullanıcıların yüklediği fotoğrafları işleyerek, hedeflenen vücut formuna ulaştıklarında nasıl görüneceklerini simüle eder.

#### 📅 Yönetim ve Organizasyon
* **Rol Bazlı Yetkilendirme (Identity):** Admin, Gym (Salon), Trainer (Antrenör) ve Member (Üye) rolleri ile güvenli erişim.
* **Randevu Sistemi:** Üyeler, antrenörlerin müsaitlik durumuna göre online randevu alabilir. Sistem otomatik çakışma kontrolü yapar.
* **Dinamik Profil Yönetimi:** Kullanıcılar profil bilgilerini, fiziksel ölçülerini ve şifrelerini güvenli bir şekilde güncelleyebilir.
* **Dashboard:** Her rol için özelleştirilmiş gösterge panelleri.

## 🏗️ Mimari ve Teknolojiler

Proje, sürdürülebilirlik, test edilebilirlik ve temiz kod prensipleri gözetilerek geliştirilmiştir.

* **Backend:** ASP.NET Core MVC (.NET 6/7/8)
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core (Code First Yaklaşımı)
* **Tasarım Desenleri:**
    * **Repository Pattern:** Veri erişim katmanını soyutlamak için `Repository<T>` yapısı.
    * **Unit of Work:** Transaction yönetimi ve veri bütünlüğü için `UnitOfWork` yapısı.
* **Frontend:** Razor Views, Bootstrap 5, jQuery, AJAX.
* **External APIs:**
    * Google Gemini API (Metin Üretimi)
    * Hugging Face Inference API (Görüntü İşleme)

## 📂 Proje Yapısı

* `Controllers`: İstekleri karşılayan MVC denetleyicileri (Örn: `MemberAIPlanController`, `MemberHomeController`).
* `Models`: Veritabanı varlıkları (Örn: `Member`, `ApplicationUser`, `AIDailyPlanRecommendation`).
* `ViewModels (DTOs)`: View ve Controller arası veri taşıma nesneleri.
* `Repositories`: Veritabanı işlemleri (`TrainerRepository`, `MemberRepository`).
* `Services`: Dış servis entegrasyonları (`GeminiTextService`, `HuggingFaceAIPhotoService`).

## 🚀 Kurulum Adımları

Projeyi yerel ortamınızda çalıştırmak için:

1.  **Projeyi Klonlayın:**
    ```bash
    git clone [https://github.com/kullaniciadi/proje-adi.git](https://github.com/kullaniciadi/proje-adi.git)
    ```

2.  **Veritabanı Bağlantısını Ayarlayın:**
    `appsettings.json` dosyasındaki `DefaultConnection` alanını kendi SQL Server bilginize göre düzenleyin.

3.  **API Anahtarlarını Girin:**
    `appsettings.json` dosyasına Google ve Hugging Face API anahtarlarınızı ekleyin:
    ```json
    "GeminiSettings": {
      "ApiKey": "YOUR_GEMINI_API_KEY"
    },
    "HuggingFaceSettings": {
      "ApiKey": "YOUR_HUGGING_FACE_TOKEN"
    }
    ```

4.  **Veritabanını Oluşturun:**
    Package Manager Console üzerinden:
    ```powershell
    Update-Database
    ```

5.  **Çalıştırın:**
    Projeyi derleyin ve çalıştırın.


---
*Bu proje Web Programlama dersi kapsamında geliştirilmiştir.*
