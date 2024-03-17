using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{
    
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_de01b48692bc748b7437b7799f6b2826b3497b74169b147dbbb1cba448cac58a";

        public PaymentsController(IPaymentService paymentService , ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
       public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket =await _paymentService.CreateOrUpdatePayment(basketId);
            if (basket is null) return BadRequest(new ApiResponse(404 , "An Error With Your Basket"));
            else return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var payment =(PaymentIntent) stripeEvent.Data.Object;
                Order order;
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order= await _paymentService.UpdatePaymentIntentToSuccdedOrFailed(payment.Id, true);
                        _logger.LogInformation("Payment Is Succeeded" , payment.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order= await _paymentService.UpdatePaymentIntentToSuccdedOrFailed(payment.Id, false);
                        _logger.LogInformation("Payment Is Failed", payment.Id);
                        break;
                        
                        
                }

                return Ok();
            
        }
    }
}
