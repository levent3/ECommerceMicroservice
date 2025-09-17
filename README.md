Mimari Kararlar ve Tasarım Tercihleri
Bu belge, E-Commerce Microservice projesinin geliştirilmesi sırasında alınan mimari kararları ve tasarım prensiplerini açıklamaktadır.

1. Katmanlı Mimari (Layered Architecture)
Proje, işlevsel sorumlulukları ayırmak için üç katmana ayrılmıştır:

ECommerceMicroservice.Core: İş mantığı katmanıdır. Projenin ana varlıklarını (Entities) ve bu varlıklar üzerinde çalışacak operasyonların arayüzlerini (Interfaces) içerir. Bu katman, başka hiçbir katmana bağımlı değildir.
ECommerceMicroservice.Infrastructure: Veri erişim ve dış servislerle entegrasyon katmanıdır. Entity Framework Core ile veritabanı işlemlerini yöneten DbContext ve Repository sınıflarını barındırır.
ECommerceMicroservice.Api: Uygulamanın sunum katmanıdır. HTTP isteklerini yöneten Controllers'ları ve bağımlılık enjeksiyonu (Dependency Injection) ayarlarını içerir.
Bu yapı, SOLID prensiplerinden Sorumlulukların Tekliği (Single Responsibility Principle) ve Bağımlılıkların Tersine Çevrilmesi (Dependency Inversion Principle)'ne uygun olarak, kodun daha esnek, test edilebilir ve sürdürülebilir olmasını sağlar.

2. Repository Deseni ve Generic Yaklaşım
Veritabanı işlemleri için Repository Deseni kullanılmıştır.

IRepository<T>: Genel CRUD (Create, Read, Update, Delete) operasyonlarını tanımlayan bir generic arayüz oluşturulmuştur.
RepositoryBase<T>: IRepository<T> arayüzünü uygulayan soyut (abstract) bir temel sınıf oluşturulmuştur. Bu sınıf, tüm varlıklar için ortak olan veri erişim mantığını (örneğin AddAsync, GetAllAsync) içerir ve kod tekrarını önler.
ProductRepository ve CategoryRepository: Bu sınıflar, RepositoryBase<T> sınıfından miras alarak varlığa özgü sorguların (.Include() gibi) eklenebilmesini sağlamıştır. Bu yaklaşım, temiz bir ayrım sağlar ve Controllers'ın doğrudan DbContext'e bağımlı olmasını engeller.
3. Bağımlılık Enjeksiyonu (Dependency Injection)
Projenin başlangıç noktası olan Program.cs dosyasında, DbContext ve Repository servisleri bağımlılık enjeksiyonu konteynerine kaydedilmiştir. Bu sayede:

Üst düzey modüller (örneğin Controllers), alt düzey modüllerin (örneğin Repositories) somut uygulamalarına değil, arayüzlerine (IProductRepository) bağımlı olmuştur.
Kodun test edilebilirliği artırılmış, birim testler için sahte (mock) repozitoryler kolayca kullanılabilir hale gelmiştir.
4. Veri Transfer Nesneleri (DTOs) Kullanımı
API katmanında, doğrudan veritabanı entity'leri yerine DTO'lar (Data Transfer Objects) kullanılmıştır.

Avantajları:
Güvenlik: İstemcilere gereksiz veya hassas verilerin gösterilmesi engellenir.
Esneklik: API'nin aldığı ve döndürdüğü veri formatı, veritabanı şemasından bağımsız hale gelir.
Performans: GetAll gibi işlemlerde, sadece ihtiyacımız olan alanları içeren hafif DTO'lar döndürülerek performans artırılır.
Döngüsel Referans Sorunu: Category ve Product entity'leri arasındaki dairesel referans, CategoryDto gibi basit DTO'lar kullanılarak çözülmüştür.
5. Hata Yönetimi ve Loglama
Merkezi bir hata yönetimi ve loglama altyapısı kurulmuştur.

ExceptionHandlingMiddleware: İstek işlem hattının en üstüne yerleştirilerek, uygulama genelindeki tüm beklenmeyen hataları (500 Internal Server Error) yakalamak için tasarlanmıştır. Bu, kod tekrarını önler ve kullanıcıya genel, güvenli hata mesajları sunar.
Serilog Entegrasyonu: ASP.NET Core'un yerleşik loglama altyapısı yerine Serilog kullanılmıştır. Bu tercih, logların hem konsola hem de bir dosyaya (log.txt) yazılabilmesi gibi esnek bir yapılandırma imkanı sunar. Dairesel referans hatası gibi tüm beklenmedik hatalar, bu altyapı aracılığıyla detaylı bir şekilde loglanmaktadır.
Bu tasarım kararları, projenin ölçeklenebilir, bakımı kolay ve profesyonel bir standartta olmasını sağlamıştır.
