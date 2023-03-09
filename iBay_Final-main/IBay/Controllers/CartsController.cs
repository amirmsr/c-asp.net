using System;
using IBay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartsController : ControllerBase
    {
        private Cart GetOrCreateCartForUser()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            var cart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        private readonly IBayDbContext _context;

        public CartsController(IBayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            var cart = GetOrCreateCartForUser();
            return Ok(cart.Products);
        }

        [HttpPost("{productId}")]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var cart = GetOrCreateCartForUser();
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found");
            }

            if (product.Available == false)
            {
                return NotFound($"{product.Name} is not available for the moment");
            }

            cart.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cart = GetOrCreateCartForUser();
            var product = cart.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found in cart");
            }

            cart.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("total")]
        [Authorize]
        public async Task<ActionResult<decimal>> GetTotal()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            var cart = await _context.Carts.Include(c => c.Products).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var totalPrice = cart.Products.Sum(p => p.Price);

            return Ok(totalPrice);
        }

        [HttpPost("pay")]
        [Authorize]
        public async Task<IActionResult> Pay()
        {
            return Ok("Payment successfull");
        }
    }

}