using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Dtos.Order;
using OnlineShopping.Models;
using OnlineShopping.Utility;
using System.Net;

namespace OnlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponse _rs;
        public OrderController(AppDbContext appDbContext)
        {
            _db = appDbContext;
            _rs = new ApiResponse();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string userId)
        {
            try
            {
                var orderHeaders = _db.OrderHeaders.Include(u=>u.OrderDetails).ThenInclude(u=>u.Product).OrderByDescending(u=>u.OrderHeaderId);
                if(!string.IsNullOrEmpty(userId))
                {
                    _rs.Result = orderHeaders.Where(u=>u.ApplicationUserId == userId);
                }
                else
                {
                    _rs.Result = orderHeaders;
                }
                _rs.StatusCode = HttpStatusCode.OK;
                return Ok(_rs);
            }
            catch (Exception ex)
            {
                _rs.IsSuccess = false;
                _rs.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _rs;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrders(int id)
        {
            try
            {
                if (id == 0)
                {
                    _rs.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_rs);
                }


                var orderHeaders = _db.OrderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Product)
                    .Where(u => u.OrderHeaderId == id);
                if (orderHeaders == null)
                {
                    _rs.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_rs);
                }
                _rs.Result = orderHeaders;
                _rs.StatusCode = HttpStatusCode.OK;
                return Ok(_rs);
            }
            catch (Exception ex)
            {
                _rs.IsSuccess = false;
                _rs.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _rs;
        }

        [HttpPost] 
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto orderHeaderDTO)
        {
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                    PickupEmail = orderHeaderDTO.PickupEmail,
                    PickupName = orderHeaderDTO.PickupName,
                    PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                    OrderTotal = orderHeaderDTO.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentID = orderHeaderDTO.StripePaymentIntentID,
                    TotalItems = orderHeaderDTO.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.status_pending : orderHeaderDTO.Status,
                };

                if (ModelState.IsValid)
                {
                    _db.OrderHeaders.Add(order);
                    _db.SaveChanges();
                    foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDto)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            ItemName = orderDetailDTO.ItemName,
                            ProductId = orderDetailDTO.ProductId,
                            Price = orderDetailDTO.Price,
                            Quantity = orderDetailDTO.Quantity,
                        };
                        _db.OrderDetails.Add(orderDetails);
                    }
                    _db.SaveChanges();
                    _rs.Result = order;
                    order.OrderDetails = null;
                    _rs.StatusCode = HttpStatusCode.Created;
                    return Ok(_rs);
                }
            }
            catch (Exception ex)
            {
                _rs.IsSuccess = false;
                _rs.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _rs;
        }


    }
}
