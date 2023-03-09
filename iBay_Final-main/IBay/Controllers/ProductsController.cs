using System;
using System.Data;
using System.Security.Claims;
using IBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IBay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
	{
        private readonly IBayDbContext _context;

        public ProductsController(IBayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int limit = 10, [FromQuery] string search = null, [FromQuery] string sort = null)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            switch (sort?.ToLower())
            {
                case "date":
                    query = query.OrderByDescending(p => p.AddedTime);
                    break;
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
            }

            query = query.Take(limit);

            return await query.ToListAsync();
        }

        [HttpGet("owning")]
        [Authorize(Roles = "admin,seller")]
        public async Task<ActionResult<IEnumerable<Product>>> GetOwnProducts()
        {
            var id = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            var products = await _context.Products.Where(p => p.SellerId == id).ToListAsync();

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Created($"User {product.Name} created succesfully", product.Name);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            if (product.SellerId != userId) return Unauthorized();

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            if (product.SellerId != userId) return Unauthorized();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok($"{product.Name} deleted succesfully");
        }
    }
}

