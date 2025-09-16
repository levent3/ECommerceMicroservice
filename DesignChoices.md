# Mimari Kararlar ve Tasar�m Tercihleri

Bu belge, E-Commerce Microservice projesinin geli�tirilmesi s�ras�nda al�nan mimari kararlar� ve tasar�m prensiplerini a��klamaktad�r.

---

## 1. Katmanl� Mimari (Layered Architecture)

Proje, i�levsel sorumluluklar� ay�rmak i�in �� katmana ayr�lm��t�r:

-   **ECommerceMicroservice.Core**: �� mant��� katman�d�r. Projenin ana varl�klar�n� (`Entities`) ve bu varl�klar �zerinde �al��acak operasyonlar�n aray�zlerini (`Interfaces`) i�erir. Bu katman, ba�ka hi�bir katmana ba��ml� de�ildir.
-   **ECommerceMicroservice.Infrastructure**: Veri eri�im ve d�� servislerle entegrasyon katman�d�r. Entity Framework Core ile veritaban� i�lemlerini y�neten `DbContext` ve `Repository` s�n�flar�n� bar�nd�r�r.
-   **ECommerceMicroservice.Api**: Uygulaman�n sunum katman�d�r. HTTP isteklerini y�neten `Controllers`'lar� ve ba��ml�l�k enjeksiyonu (`Dependency Injection`) ayarlar�n� i�erir.

Bu yap�, SOLID prensiplerinden **Sorumluluklar�n Tekli�i (Single Responsibility Principle)** ve **Ba��ml�l�klar�n Tersine �evrilmesi (Dependency Inversion Principle)**'ne uygun olarak, kodun daha esnek, test edilebilir ve s�rd�r�lebilir olmas�n� sa�lar.

## 2. Repository Deseni ve Generic Yakla��m

Veritaban� i�lemleri i�in **Repository Deseni** kullan�lm��t�r.

-   **`IRepository<T>`**: Genel CRUD (Create, Read, Update, Delete) operasyonlar�n� tan�mlayan bir generic aray�z olu�turulmu�tur.
-   **`RepositoryBase<T>`**: `IRepository<T>` aray�z�n� uygulayan soyut (`abstract`) bir temel s�n�f olu�turulmu�tur. Bu s�n�f, t�m varl�klar i�in ortak olan veri eri�im mant���n� (�rne�in `AddAsync`, `GetAllAsync`) i�erir ve kod tekrar�n� �nler.
-   **`ProductRepository` ve `CategoryRepository`**: Bu s�n�flar, `RepositoryBase<T>` s�n�f�ndan miras alarak varl��a �zg� sorgular�n (`.Include()` gibi) eklenebilmesini sa�lam��t�r. Bu yakla��m, temiz bir ayr�m sa�lar ve `Controllers`'�n do�rudan `DbContext`'e ba��ml� olmas�n� engeller.

## 3. Ba��ml�l�k Enjeksiyonu (Dependency Injection)

Projenin ba�lang�� noktas� olan `Program.cs` dosyas�nda, `DbContext` ve `Repository` servisleri ba��ml�l�k enjeksiyonu konteynerine kaydedilmi�tir. Bu sayede:

-   �st d�zey mod�ller (�rne�in `Controllers`), alt d�zey mod�llerin (�rne�in `Repositories`) somut uygulamalar�na de�il, aray�zlerine (`IProductRepository`) ba��ml� olmu�tur.
-   Kodun test edilebilirli�i art�r�lm��, birim testler i�in sahte (`mock`) repozitoryler kolayca kullan�labilir hale gelmi�tir.

## 4. Veri Transfer Nesneleri (DTOs) Kullan�m�

API katman�nda, do�rudan veritaban� entity'leri yerine **DTO'lar (Data Transfer Objects)** kullan�lm��t�r.

-   **Avantajlar�**:
    -   **G�venlik**: �stemcilere gereksiz veya hassas verilerin g�sterilmesi engellenir.
    -   **Esneklik**: API'nin ald��� ve d�nd�rd��� veri format�, veritaban� �emas�ndan ba��ms�z hale gelir.
    -   **Performans**: `GetAll` gibi i�lemlerde, sadece ihtiyac�m�z olan alanlar� i�eren hafif DTO'lar d�nd�r�lerek performans art�r�l�r.
    -   **D�ng�sel Referans Sorunu**: `Category` ve `Product` entity'leri aras�ndaki dairesel referans, `CategoryDto` gibi basit DTO'lar kullan�larak ��z�lm��t�r.

## 5. Hata Y�netimi ve Loglama

Merkezi bir hata y�netimi ve loglama altyap�s� kurulmu�tur.

-   **`ExceptionHandlingMiddleware`**: �stek i�lem hatt�n�n en �st�ne yerle�tirilerek, uygulama genelindeki t�m beklenmeyen hatalar� (`500 Internal Server Error`) yakalamak i�in tasarlanm��t�r. Bu, kod tekrar�n� �nler ve kullan�c�ya genel, g�venli hata mesajlar� sunar.
-   **Serilog Entegrasyonu**: ASP.NET Core'un yerle�ik loglama altyap�s� yerine Serilog kullan�lm��t�r. Bu tercih, loglar�n hem konsola hem de bir dosyaya (`log.txt`) yaz�labilmesi gibi esnek bir yap�land�rma imkan� sunar. Dairesel referans hatas� gibi t�m beklenmedik hatalar, bu altyap� arac�l���yla detayl� bir �ekilde loglanmaktad�r.

Bu tasar�m kararlar�, projenin �l�eklenebilir, bak�m� kolay ve profesyonel bir standartta olmas�n� sa�lam��t�r.