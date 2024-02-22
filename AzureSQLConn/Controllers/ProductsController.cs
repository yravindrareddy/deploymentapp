using AutoMapper;
using AzureSQLConn.Database;
using AzureSQLConn.Entities;
using AzureSQLConn.Models;
using AzureSQLConn.Repositories;
using MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace AzureSQLConn.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMessageBus _messageBus;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductsController(ProductDbContext dbContext, IMessageBus messageBus, IProductRepository productRepository, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _messageBus = messageBus;
            _productRepository = productRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<IActionResult> GetAllProducts()
        {            
            var products = await _productRepository.GetAllProducts();

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);

            await _productRepository.AddProduct(product);
            
            await _messageBus.PublishMessage(productDto, _configuration["MessageBus:QueueName"]);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {            
            if (id != productDto.Id)
            {
                return BadRequest();
            }
            var existingProduct = await _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productRepository.UpdateProduct(_mapper.Map<Product>(productDto));           

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async  Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }
            _productRepository.DeleteProduct(existingProduct);
            return NoContent();
        }

        [HttpGet("Search")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ProductDto), 200)]
        public async Task<IActionResult> Search([FromQuery]string searchParam)
        {
            
            if (string.IsNullOrEmpty(searchParam))
            {
                return BadRequest();
            }

            var products = await _productRepository.Search(searchParam.ToLower());

            return Ok(_mapper.Map<List<ProductDto>>(products));
        }
    }
}
