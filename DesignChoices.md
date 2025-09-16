# Mimari Kararlar ve Tasarým Tercihleri

Bu belge, E-Commerce Microservice projesinin geliþtirilmesi sýrasýnda alýnan mimari kararlarý ve tasarým prensiplerini açýklamaktadýr.

---

## 1. Katmanlý Mimari (Layered Architecture)

Proje, iþlevsel sorumluluklarý ayýrmak için üç katmana ayrýlmýþtýr:

-   **ECommerceMicroservice.Core**: Ýþ mantýðý katmanýdýr. Projenin ana varlýklarýný (`Entities`) ve bu varlýklar üzerinde çalýþacak operasyonlarýn arayüzlerini (`Interfaces`) içerir. Bu katman, baþka hiçbir katmana baðýmlý deðildir.
-   **ECommerceMicroservice.Infrastructure**: Veri eriþim ve dýþ servislerle entegrasyon katmanýdýr. Entity Framework Core ile veritabaný iþlemlerini yöneten `DbContext` ve `Repository` sýnýflarýný barýndýrýr.
-   **ECommerceMicroservice.Api**: Uygulamanýn sunum katmanýdýr. HTTP isteklerini yöneten `Controllers`'larý ve baðýmlýlýk enjeksiyonu (`Dependency Injection`) ayarlarýný içerir.

Bu yapý, SOLID prensiplerinden **Sorumluluklarýn Tekliði (Single Responsibility Principle)** ve **Baðýmlýlýklarýn Tersine Çevrilmesi (Dependency Inversion Principle)**'ne uygun olarak, kodun daha esnek, test edilebilir ve sürdürülebilir olmasýný saðlar.

## 2. Repository Deseni ve Generic Yaklaþým

Veritabaný iþlemleri için **Repository Deseni** kullanýlmýþtýr.

-   **`IRepository<T>`**: Genel CRUD (Create, Read, Update, Delete) operasyonlarýný tanýmlayan bir generic arayüz oluþturulmuþtur.
-   **`RepositoryBase<T>`**: `IRepository<T>` arayüzünü uygulayan soyut (`abstract`) bir temel sýnýf oluþturulmuþtur. Bu sýnýf, tüm varlýklar için ortak olan veri eriþim mantýðýný (örneðin `AddAsync`, `GetAllAsync`) içerir ve kod tekrarýný önler.
-   **`ProductRepository` ve `CategoryRepository`**: Bu sýnýflar, `RepositoryBase<T>` sýnýfýndan miras alarak varlýða özgü sorgularýn (`.Include()` gibi) eklenebilmesini saðlamýþtýr. Bu yaklaþým, temiz bir ayrým saðlar ve `Controllers`'ýn doðrudan `DbContext`'e baðýmlý olmasýný engeller.

## 3. Baðýmlýlýk Enjeksiyonu (Dependency Injection)

Projenin baþlangýç noktasý olan `Program.cs` dosyasýnda, `DbContext` ve `Repository` servisleri baðýmlýlýk enjeksiyonu konteynerine kaydedilmiþtir. Bu sayede:

-   Üst düzey modüller (örneðin `Controllers`), alt düzey modüllerin (örneðin `Repositories`) somut uygulamalarýna deðil, arayüzlerine (`IProductRepository`) baðýmlý olmuþtur.
-   Kodun test edilebilirliði artýrýlmýþ, birim testler için sahte (`mock`) repozitoryler kolayca kullanýlabilir hale gelmiþtir.

## 4. Veri Transfer Nesneleri (DTOs) Kullanýmý

API katmanýnda, doðrudan veritabaný entity'leri yerine **DTO'lar (Data Transfer Objects)** kullanýlmýþtýr.

-   **Avantajlarý**:
    -   **Güvenlik**: Ýstemcilere gereksiz veya hassas verilerin gösterilmesi engellenir.
    -   **Esneklik**: API'nin aldýðý ve döndürdüðü veri formatý, veritabaný þemasýndan baðýmsýz hale gelir.
    -   **Performans**: `GetAll` gibi iþlemlerde, sadece ihtiyacýmýz olan alanlarý içeren hafif DTO'lar döndürülerek performans artýrýlýr.
    -   **Döngüsel Referans Sorunu**: `Category` ve `Product` entity'leri arasýndaki dairesel referans, `CategoryDto` gibi basit DTO'lar kullanýlarak çözülmüþtür.

## 5. Hata Yönetimi ve Loglama

Merkezi bir hata yönetimi ve loglama altyapýsý kurulmuþtur.

-   **`ExceptionHandlingMiddleware`**: Ýstek iþlem hattýnýn en üstüne yerleþtirilerek, uygulama genelindeki tüm beklenmeyen hatalarý (`500 Internal Server Error`) yakalamak için tasarlanmýþtýr. Bu, kod tekrarýný önler ve kullanýcýya genel, güvenli hata mesajlarý sunar.
-   **Serilog Entegrasyonu**: ASP.NET Core'un yerleþik loglama altyapýsý yerine Serilog kullanýlmýþtýr. Bu tercih, loglarýn hem konsola hem de bir dosyaya (`log.txt`) yazýlabilmesi gibi esnek bir yapýlandýrma imkaný sunar. Dairesel referans hatasý gibi tüm beklenmedik hatalar, bu altyapý aracýlýðýyla detaylý bir þekilde loglanmaktadýr.

Bu tasarým kararlarý, projenin ölçeklenebilir, bakýmý kolay ve profesyonel bir standartta olmasýný saðlamýþtýr.