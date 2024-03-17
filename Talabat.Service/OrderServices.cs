using System;
using System.Collections;
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
    public class OrderServices : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderServices(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productrepo = productrepo;
            //_deliverymethodrepo = deliverymethodrepo;
            //_orderrepo = orderrepo;
        }

        public async Task<Order> GetOrderAsync(string buyeremail, string basketId, int deliverymethodid, Address address)
        {
            ///var basket = await _basketRepo.GetBasketAsync(basketId);
            ///var orderitems = new List<OrderItem>();
            ///if (basket?.Items?.Count > 0)
            ///{
            ///    var productrepo = _unitOfWork.Repository<Product>();
            ///    foreach (var item in basket.Items)
            ///    {
            ///        var product = await productrepo.GetAsync(item.Id);
            ///        ProductItemOrder orders = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);
            ///        OrderItem orderItem = new OrderItem(orders, product.Price, item.Quantity);
            ///        orderitems.Add(orderItem);
            ///    }
            ///}
            ///var subtotal = orderitems.Sum(o => o.Price * o.Quantity);
            ///var delivery = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliverymethodid);
            ///Order order = new Order(buyeremail, address, delivery, orderitems, subtotal);
            ///await _unitOfWork.Repository<Order>().AddAsync(order);
            ///var result = await _unitOfWork.CompleteAsync();
            ///if (result <= 0) return null;
            ///return order;
            ///

            var basket = await _basketRepo.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                var productspec = _unitOfWork.Repository<Product>();


                foreach (var item in basket.Items)
                {
                    var product = await productspec.GetAsync(item.Id);
                    ProductItemOrder productorder = new ProductItemOrder(item.Id, item.ProductName, item.PictureUrl);
                    OrderItem orderItem = new OrderItem(productorder, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }

            }
            var subtotal = orderItems.Sum(O => O.Price * O.Quantity);
            var deliverymethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliverymethodid);
            var orderSpec = new OrderSpecWithPaymentIntent(basket.PaymentIntentId);
            var orderExsiting = await _unitOfWork.Repository<Order>().GetWithSpecAsync(orderSpec);
            if (orderExsiting is not null)
            {
                _unitOfWork.Repository<Order>().Delete(orderExsiting);
                await _paymentService.CreateOrUpdatePayment(basketId);
            }

            Order order = new Order(buyeremail, address, deliverymethod, orderItems, subtotal, basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public Task<Order?> GetOrderByIdFromUserAsync(int orderid, string buyeremail)
        {

            var orderspec = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(orderid, buyeremail);
            var order = orderspec.GetWithSpecAsync(spec);
            return order;

        }

        public Task<IReadOnlyList<Order>> GetOrdersFromUserAsync(string buyeremail)
        {
            var orderrepo = _unitOfWork.Repository<Order>();
            var orderspec = new OrderSpecifications(buyeremail);
            var order = orderrepo.GetAllWithSpecAsync(orderspec);
            return order;
        }
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryrepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliverys = deliveryrepo.GetAllAsync();
            return deliverys;

        }

    }
}
