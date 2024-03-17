using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetAllProductAsync(ProductSpecParams productSpecParams)
        {
            var Spec = new ProductWithBrandandCategory(productSpecParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            return products;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            var brandes = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return brandes;
        }

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        {
            var Categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return Categories;
        }

        public async Task<int> GetCountAsync(ProductSpecParams productSpecParams)
        {
            var count = new ProductWithFilterationForCountSpecification(productSpecParams);
            var DataCount = await _unitOfWork.Repository<Product>().GetCountAsync(count);
            return DataCount;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var Spec = new ProductWithBrandandCategory(id);
            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(Spec);
            return product;
        }
    }
}
