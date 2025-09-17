using AutoMapper;
using ECommerceMicroservice.Api.DTOs;
using ECommerceMicroservice.Core.Entities;
using ECommerceMicroservice.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMicroservice.Api.Controllers
{
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

           var categoryDtos=_mapper.Map<IEnumerable<CategoryDto>>(categories);
        
            return Ok(categoryDtos);
        }


        [HttpGet("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.AddAsync(category);
            var createdCategoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction("GetCategory", new { id = category.Id }, createdCategoryDto);
        }
        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
