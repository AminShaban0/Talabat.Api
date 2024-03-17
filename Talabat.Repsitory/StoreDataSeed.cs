using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Repsitory.Data;

namespace Talabat.Repsitory
{
    public static class StoreDataSeed
    {
        public async static Task SeedAsync(StoreDbContext _dbContext)
        {
            if (_dbContext.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Repsitory/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
                if (Categories?.Count() > 0)
                {
                    foreach (var Category in Categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(Category);
                    }
                   await _dbContext.SaveChangesAsync();
                } 
            }
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var BrandData = File.ReadAllText("../Talabat.Repsitory/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count() > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(Brand);
                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }
            if(_dbContext.Products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Repsitory/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if(Products?.Count() > 0)
                {
                    foreach(var Product in Products)
                        _dbContext.Set<Product>().Add(Product);
                   await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var deliverymeth = File.ReadAllText("../Talabat.Repsitory/Data/DataSeed/delivery.json");
                var deliverys = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymeth);
                if (deliverys?.Count() > 0)
                {
                    foreach (var item in deliverys)
                        _dbContext.Set<DeliveryMethod>().Add(item);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
