using System.ComponentModel.DataAnnotations;

namespace ECommerceMicroservice.Api.DTOs
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;
    }
}
