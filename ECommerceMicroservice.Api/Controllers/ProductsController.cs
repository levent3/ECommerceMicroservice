using ECommerceMicroservice.Api.DTOs;
using ECommerceMicroservice.Core.Entities;
using ECommerceMicroservice.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMicroservice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Belirtilmemiş"
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]       
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Belirtilmemiş"
            };

            return productDto;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId
            };

            await _productRepository.AddAsync(product);

            // Yeni oluşturulan ürün nesnesini ProductDto'ya dönüştürerek döndür
            var createdProductDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Belirtilmemiş"
            };

            return CreatedAtAction("GetProduct", new { id = createdProductDto.Id }, createdProductDto);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productRepository.UpdateAsync(product);
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product);
            return NoContent();
        }
    }
}
