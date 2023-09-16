using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Dtos.ProductDtos;
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

        [HttpGet("{id:int}", Name = "GetProductById")]
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateProduct([FromForm] ProductCreateDto productDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Product product = new()
                    {
                        Name = productDto.Name,
                        Price = productDto.Price,
                        Category = productDto.Category,
                        SpecialTag = productDto.SpecialTag,
                        Description = productDto.Description,
                        Image = productDto.Image,
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                    response.Result = product;
                    response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetProductById", new { id = product.Id }, response);
                     

                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return response;
        }
    }
}
