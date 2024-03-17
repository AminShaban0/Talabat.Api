using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecifications :BaseSpecification<Order>
    {
        public OrderSpecifications(string buyeremail):base(o=>o.BuyerEmail == buyeremail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);

        }
        public OrderSpecifications(int id , string email):base(o=>o.BuyerEmail==email&&  o.Id ==id) 
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
