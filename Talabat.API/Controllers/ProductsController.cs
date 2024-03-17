using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Helper;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        ///private readonly IGenericRepository<Product> _productrepo;
        ///private readonly IGenericRepository<ProductBrand> _brandRepo;
        ///private readonly IGenericRepository<ProductCategory> _categoriesRepo;

        public ProductsController( IMapper mapper, IProductService productService)
        {
            
            _mapper = mapper;
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProduct([FromQuery]ProductSpecParams productSpecParams)
        {

            var products = await _productService.GetAllProductAsync(productSpecParams);
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var DataCount = await _productService.GetCountAsync(productSpecParams);
            return Ok(new Pagination<ProductToReturnDto>(productSpecParams.Pageindex , productSpecParams .PageSize ,DataCount, Data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<Product , ProductToReturnDto>(product));
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brandes = await _productService.GetBrandsAsync();
            return Ok(brandes);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> Getategories()
        {
            var Categories = await _productService.GetCategoriesAsync();
            return Ok(Categories);
        }


    }
}
