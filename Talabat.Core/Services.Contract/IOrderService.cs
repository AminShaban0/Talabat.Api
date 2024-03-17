using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> GetOrderAsync(string buyeremail, string basketId, int deliverymethodid, Address address);
        Task<IReadOnlyList<Order>> GetOrdersFromUserAsync(string buyeremail);
        Task<Order?> GetOrderByIdFromUserAsync(int orderid, string buyeremail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
