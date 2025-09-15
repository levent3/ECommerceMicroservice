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
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
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
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            await _categoryRepository.UpdateAsync(category);
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
