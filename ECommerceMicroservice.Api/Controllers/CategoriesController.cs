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

        public CategoriesController(ICategoryRepository categoryRepository)
        {
          _categoryRepository = categoryRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return Ok(categoryDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            // Category entity nesnesini CategoryDto'ya dönüştürüyoruz
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return categoryDto;

           
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            await _categoryRepository.AddAsync(category);
            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CreateCategoryDto categoryDto)
        {

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"ID'si {id} olan kategori bulunamadı.");
            }

            // DTO'dan gelen veriyi mevcut entity nesnesine eşle
            existingCategory.Name = categoryDto.Name;

            await _categoryRepository.UpdateAsync(existingCategory);

            return NoContent();
         
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(category);
            return NoContent();
        }

    }
}
