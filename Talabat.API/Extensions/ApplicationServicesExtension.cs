using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.API.Helper;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Repsitory;
using Talabat.Repsitory.Repositories;
using Talabat.Service;

namespace Talabat.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IProductService), typeof(ProductService));
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IOrderService), typeof(OrderServices));
            //Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddScoped (typeof(IPaymentService), typeof(Paymentservice)); 
            Services.AddAutoMapper(typeof(MappingProfile));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(P => P.ErrorMessage)
                                                         .ToArray();
                    var apivalidation = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(apivalidation);
                };
            });
            return Services;
        }
    }
}
