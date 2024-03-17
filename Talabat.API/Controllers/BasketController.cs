using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.API.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket =await _basketRepository.GetBasketAsync(id);
            return Ok(basket?? new CustomerBasket(id));
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mapedbasket = _mapper.Map<CustomerBasketDto ,CustomerBasket>(basket);
          var basketcreated = await _basketRepository.UpdateBasketAsync(mapedbasket);
            if (basketcreated is null) return BadRequest(new ApiResponse(400));
            return Ok(basketcreated);
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
           await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
