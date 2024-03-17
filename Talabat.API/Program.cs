using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Helper;
using Talabat.API.MiddleWare;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Repsitory;
using Talabat.Repsitory.Data;
using Talabat.Repsitory.Identity;
using Talabat.Repsitory.Repositories;
using Talabat.Service;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddSwaggerServices();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<AppIdentityDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Identity")));

            builder.Services.AddSingleton<IConnectionMultiplexer>((serverprovider) =>
            {
                var connection = builder.Configuration.GetConnectionString("radis");
                return ConnectionMultiplexer.Connect(connection);
            });
            //builder.Services.AddApplicationServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontUrl"]);
                });
            });



            var app = builder.Build();
            using var Scope =app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var Dbcontext =Services.GetRequiredService<StoreDbContext>();
            var UserDbcontext = Services.GetRequiredService<AppIdentityDbContext>();

            var IloggerFactory =Services.GetRequiredService<ILoggerFactory>();
            try
            {
                await Dbcontext.Database.MigrateAsync();
                await StoreDataSeed.SeedAsync(Dbcontext);
                await UserDbcontext.Database.MigrateAsync();
                var _usermanger = Services.GetRequiredService<UserManager<AppUser> >();
                await AppIdentityDbcontextSeed.StoreIdentityAppUser(_usermanger);
            }
            catch (Exception ex)
            {

              var logger=  IloggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply the migrations");
            }
            app.UseMiddleware<ExceptionMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();




            app.Run();
        }

        private static IConfiguration ApiBehaviorOptions(Action<object> value)
        {
            throw new NotImplementedException();
        }
    }
}