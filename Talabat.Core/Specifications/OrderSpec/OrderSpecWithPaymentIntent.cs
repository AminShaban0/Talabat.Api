using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecWithPaymentIntent :BaseSpecification<Order>
    {
        public OrderSpecWithPaymentIntent(string paymentIntentId):base(O=>O.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
