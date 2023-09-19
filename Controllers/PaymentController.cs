using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;
using OnlineShopping.Utility;
using Stripe;
using System.Net;

namespace OnlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected ApiResponse _rs;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        public PaymentController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _db = appDbContext;
            _rs = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts
                .Include(x => x.CartItems)
                .ThenInclude(x=>x.Product).FirstOrDefault(u=>u.UserId == userId);
            if(shoppingCart == null || shoppingCart.CartItems==null) 
            {
                _rs.StatusCode = HttpStatusCode.BadRequest;
                _rs.IsSuccess = false;
                return BadRequest();
            }

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Product.Price);
            var options = new PaymentIntentCreateOptions
            {
                Amount = (int)(shoppingCart.CartTotal*100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card"},

            };
            PaymentIntentService service = new();
            PaymentIntent response = service.Create(options);
            shoppingCart.StripePaymentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            _rs.Result = shoppingCart;
            _rs.StatusCode = HttpStatusCode.OK;
            return Ok(_rs);
        }
    }
}