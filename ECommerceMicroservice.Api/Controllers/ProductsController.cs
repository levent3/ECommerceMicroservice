using AutoMapper;
using ECommerceMicroservice.Api.DTOs;
using ECommerceMicroservice.Core.Entities;
using ECommerceMicroservice.Core.Interfaces;
using ECommerceMicroservice.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMicroservice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();


            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productDtos);
        }

        [HttpGet("{id}")]       
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("ID'si {ProductId} olan ürün bulunamadı.", id);
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            await _productRepository.AddAsync(product);

            // Yeni oluşturulan ürün nesnesini ProductDto'ya dönüştürerek döndür
            var createdProductDto = _mapper.Map<ProductDto>(product);

            return CreatedAtAction("GetProduct", new { id = createdProductDto.Id }, createdProductDto);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id,[FromBody] CreateProductDto product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                _logger.LogWarning("Güncellenecek Product bulunamadı: ID {ProductId}", id);
                return NotFound();
            }


            _mapper.Map(product, existingProduct);

            await _productRepository.UpdateAsync(existingProduct);

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Silinecek ürün bulunamadı: ID {ProductId}", id);
                return NotFound();
            }

            try
            {
                await _productRepository.DeleteAsync(product);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Ürün silme işlemi başarısız oldu: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
