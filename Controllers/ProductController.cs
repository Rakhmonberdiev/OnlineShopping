using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Models;
using System.Net;

namespace OnlineShopping.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext db;
        private ApiResponse response;
        public ProductController(AppDbContext db)
        {
            this.db = db;
            response = new ApiResponse();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllProducts()
        {
            response.Result = db.Products;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if(id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            Product product = db.Products.FirstOrDefault(x => x.Id == id);
            if(product == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            response.Result = product;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }
    }
}
