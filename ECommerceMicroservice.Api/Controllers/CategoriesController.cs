using AutoMapper;
using ECommerceMicroservice.Api.DTOs;
using ECommerceMicroservice.Core.Entities;
using ECommerceMicroservice.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMicroservice.Api.Controllers
{
    /// <summary>
    /// Kategori yönetimi için kullanılan API endpoint'lerini içerir.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository categoryRepository,IMapper mapper, ILogger<CategoriesController> logger)
        {
          _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Sistemdeki tüm kategorileri listeler.
        /// </summary>
        /// <returns>Tüm kategorilerin listesi.</returns>
        /// <response code="200">Kategoriler başarıyla listelendi.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

           var categoryDtos=_mapper.Map<IEnumerable<CategoryDto>>(categories);
        
            return Ok(categoryDtos);
        }


        /// <summary>
        /// Belirtilen ID'ye sahip bir kategoriyi getirir.
        /// </summary>
        /// <param name="id">Kategorinin ID'si.</param>
        /// <returns>Belirtilen kategori.</returns>
        /// <response code="200">Kategori başarıyla bulundu.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("ID'si {CategoryId} olan kategori bulunamadı.", id);
                return NotFound();
            }

            var categoryDto=_mapper.Map<CategoryDto>(category);
         

            return categoryDto;

           
        }
        /// <summary>
        /// Yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="categoryDto">Oluşturulacak kategorinin verileri.</param>
        /// <returns>Oluşturulan kategori.</returns>
        /// <response code="201">Kategori başarıyla oluşturuldu.</response>
        /// <response code="400">Geçersiz veri girişi.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.AddAsync(category);
            var createdCategoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction("GetCategory", new { id = category.Id }, createdCategoryDto);
        }
        /// <summary>
        /// Mevcut bir kategoriyi günceller.
        /// </summary>
        /// <param name="id">Güncellenecek kategorinin ID'si.</param>
        /// <param name="categoryDto">Yeni kategori verileri.</param>
        /// <response code="204">Kategori başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz veri girişi veya ID uyuşmazlığı.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CreateCategoryDto categoryDto)
        {

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                _logger.LogWarning("Güncellenecek kategori bulunamadı: ID {CategoryId}", id);
                return NotFound();
            }

            
            _mapper.Map(categoryDto, existingCategory);

            await _categoryRepository.UpdateAsync(existingCategory);

            return NoContent();
         
        }

        /// <summary>
        /// Belirtilen ID'ye sahip bir kategoriyi siler.
        /// </summary>
        /// <param name="id">Silinecek kategorinin ID'si.</param>
        /// <response code="204">Kategori başarıyla silindi.</response>
        /// <response code="400">Kategoriye bağlı ürünler olduğu için silme işlemi başarısız oldu.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Silinecek kategori bulunamadı: ID {CategoryId}", id);
                return NotFound();
            }

            try
            {
                await _categoryRepository.DeleteAsync(category);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Kategori silme işlemi başarısız oldu: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
