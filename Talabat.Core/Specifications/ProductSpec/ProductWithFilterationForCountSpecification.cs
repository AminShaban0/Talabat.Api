using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
    public class ProductWithFilterationForCountSpecification:BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams productSpecParams) :base(P=>
        (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower() == productSpecParams.Search)&&
        (!productSpecParams.brandid.HasValue || P.BrandId == productSpecParams.brandid) &&
        (!productSpecParams.categoryid.HasValue || P.CategoryId == productSpecParams.categoryid))
        {
            
        }
    }
}
