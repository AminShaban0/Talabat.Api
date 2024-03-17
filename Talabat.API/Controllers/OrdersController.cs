using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderdto)
        {
            var addressmapped = _mapper.Map<AddressDto, Address>(orderdto.shipToAddress);
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderAsync(buyeremail, orderdto.BasketId, orderdto.DeliveryMethodId, addressmapped);
            if (order == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersFromUser()
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrdersFromUserAsync(buyeremail);
            var ordermap = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(order);
            return Ok(ordermap);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderFromUser(int id)
        {

            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdFromUserAsync(id, buyeremail);
            if (order == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        [HttpGet ("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var deliverymethods=await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliverymethods);
        }
    }
}
