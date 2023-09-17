using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;
using System.Net;

namespace OnlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        protected ApiResponse response;
        private readonly AppDbContext _db;
        public ShoppingCartController(AppDbContext db)
        {
            _db = db;
            response = new ();
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart = await _db.ShoppingCarts
                    .Include(u => u.CartItems).ThenInclude(u => u.Product)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                }
                if (shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                {
                    shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Product.Price);
                }
                response.Result = shoppingCart;
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages
                     = new List<string>() { ex.ToString() };
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int productId, int updateQuantityBy)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            Product product = _db.Products.FirstOrDefault(u => u.Id == productId);
            if (product == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return BadRequest(response);
            }
            if (shoppingCart == null && updateQuantityBy > 0)
            {

                ShoppingCart newCart = new() { UserId = userId };
                await _db.ShoppingCarts.AddAsync(newCart);
                await _db.SaveChangesAsync();

                CartItem newCartItem = new()
                {
                    ProductId = productId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    Product = null
                };
                await _db.CartItems.AddAsync(newCartItem);
                await _db.SaveChangesAsync();
            }
            else
            {
                CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.ProductId == productId);
                if (cartItemInCart == null)
                {
                    CartItem newCartItem = new()
                    {
                        ProductId = productId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        Product = null
                    };
                    await _db.CartItems.AddAsync(newCartItem);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    int newQuantity = cartItemInCart.Quantity + updateQuantityBy;
                    if (updateQuantityBy == 0 || newQuantity <= 0)
                    {
                        _db.CartItems.Remove(cartItemInCart);
                        if (shoppingCart.CartItems.Count() == 1)
                        {
                            _db.ShoppingCarts.Remove(shoppingCart);
                        }
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cartItemInCart.Quantity = newQuantity;
                        await _db.SaveChangesAsync();
                    }
                }
            }
            return response;

        }
    }
}
