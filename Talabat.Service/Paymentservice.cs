using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Service
{
    public class Paymentservice : IPaymentService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public Paymentservice(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePayment(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingPrice = 0m;
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Core.Entities.Product>().GetAsync(item.Id);
                    if (product.Price != item.Price)
                        item.Price = product.Price;

                }
                if (basket.DeliveryMethodId.HasValue)
                {
                    var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                    shippingPrice = deliveryMethod.Cost;
                    basket.ShippingPrice = shippingPrice;
                }
            }
            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await paymentIntentService.CreateAsync(option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Updateoption = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)shippingPrice * 100,

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, Updateoption);
            }
           await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSuccdedOrFailed(string baymentIntentId, bool isSucceded)
        {
            var orderSpec = new OrderSpecWithPaymentIntent(baymentIntentId);
            var order =await _unitOfWork.Repository<Order>().GetWithSpecAsync(orderSpec);
            if (isSucceded)
            {
                order.Status = OrderStatus.PaymentSucceded;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;

        }
    }
}
