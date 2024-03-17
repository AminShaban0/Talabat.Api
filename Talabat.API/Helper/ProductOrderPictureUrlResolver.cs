using AutoMapper;
using Talabat.API.Dtos;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.API.Helper
{
    public class ProductOrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductOrderPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["BaseApiUrl"]}/{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
        
    }
}
