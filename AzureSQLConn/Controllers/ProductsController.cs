using AzureSQLConn.Database;
using AzureSQLConn.Entities;
using AzureSQLConn.Models;
using MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;

namespace AzureSQLConn.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMessageBus _messageBus;

        public ProductsController(ProductDbContext dbContext, IMessageBus messageBus)
        {
            _dbContext = dbContext;
            _messageBus = messageBus;
        }

        [HttpGet("GetProducts", Name = "GetProducts")]
        public IActionResult GetProducts()
        {
            if(!Request.Headers.TryGetValue("SecurityToken", out StringValues headerValue))
            {
                return Unauthorized("APIM Token missing");
            }

            if(headerValue.FirstOrDefault() != "pass1234")
            {
                return Unauthorized("APIM Token missing");
            }
            var products = _dbContext.Products.ToList();

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create([FromBody] ProductDto product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var maxId = _dbContext.Products.Max(x => x.Id);
            var newProduct = new Product()
            {               
                Name = product.Name,
                Description = product.Description,
                AvailableStock = product.AvailableStock,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            _dbContext.Products.Add(newProduct);
            _dbContext.SaveChanges();
            await _messageBus.PublishMessage(newProduct, "productqueue");
            return Ok(newProduct);
        }

        [HttpPut("{id}", Name = "Update")]
        public IActionResult Update(int id, [FromBody] ProductDto productUpdate)
        {
            var product = _dbContext.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            product.Name = productUpdate.Name;
            product.Description = productUpdate.Description;
            product.AvailableStock = productUpdate.AvailableStock;
            product.Price = productUpdate.Price;
            product.CategoryId = productUpdate.CategoryId;
            
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "Delete")]
        public IActionResult Delete(int id)
        {
            var product = _dbContext.Products.SingleOrDefault(_ => _.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpGet("Search", Name = "Search")]
        public IActionResult Search([FromQuery]string searchParam)
        {
            
            if (string.IsNullOrEmpty(searchParam))
            {
                return Ok(_dbContext.Products.ToList());
            }

            var searchResults = _dbContext.Products.Where(p => p.Name.ToLower().Contains(searchParam.ToLower())).ToList();

            return Ok(searchResults);
        }
    }
}
