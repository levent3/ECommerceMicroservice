using System.ComponentModel.DataAnnotations;

namespace ECommerceMicroservice.Api.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 100000.00, ErrorMessage = "Fiyat 0 ile 100,000 arasında olmalıdır.")]

        public decimal Price { get; set; }
        [Required(ErrorMessage = "Stok bilgisi zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok bilgisi 0 veya daha büyük bir sayı olmalıdır.")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "Kategori ID'si zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir Kategori ID'si girilmelidir.")]
        public int CategoryId { get; set; }
    }
}
