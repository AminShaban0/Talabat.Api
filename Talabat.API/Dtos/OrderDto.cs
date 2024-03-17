using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.API.Dtos
{
    public class OrderDto
    {
        
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; }
    }
}
