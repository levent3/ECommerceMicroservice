using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceMicroservice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Kategorileri ekle
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Elektronik')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Kitap')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Ev & Yaşam')");

            // Ürünleri ekle
            // Bu örnek, CategoryId'yi doğru şekilde almak için daha karmaşık bir SQL sorgusu gerektirir.
            // Basitlik adına, elle değerleri girebilirsiniz veya id'yi almak için JOIN kullanabilirsiniz.
            migrationBuilder.Sql("INSERT INTO Products (Name, Price, Stock, CategoryId) VALUES ('Laptop', 15000, 50, (SELECT Id FROM Categories WHERE Name = 'Elektronik'))");
            migrationBuilder.Sql("INSERT INTO Products (Name, Price, Stock, CategoryId) VALUES ('Akıllı Telefon', 8000, 100, (SELECT Id FROM Categories WHERE Name = 'Elektronik'))");
            migrationBuilder.Sql("INSERT INTO Products (Name, Price, Stock, CategoryId) VALUES ('Dostoyevski - Suç ve Ceza', 50, 200, (SELECT Id FROM Categories WHERE Name = 'Kitap'))");
            migrationBuilder.Sql("INSERT INTO Products (Name, Price, Stock, CategoryId) VALUES ('Süpürge', 1200, 30, (SELECT Id FROM Categories WHERE Name = 'Ev & Yaşam'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma (rollback) işlemi için verileri sil
            migrationBuilder.Sql("DELETE FROM Products");
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}
