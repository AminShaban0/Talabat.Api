using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePayment(string basketId);
        Task<Order> UpdatePaymentIntentToSuccdedOrFailed(string baymentIntentId, bool isSucceded);
    }
}
