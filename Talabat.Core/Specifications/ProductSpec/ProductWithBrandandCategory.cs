using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
    public class ProductWithBrandandCategory:BaseSpecification<Product>
    {
        public ProductWithBrandandCategory(ProductSpecParams productSpecParams) :base(P=> 
                        (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower() == productSpecParams.Search)&&
                        (!productSpecParams.brandid.HasValue|| P.BrandId == productSpecParams.brandid)&&
                        (!productSpecParams.categoryid.HasValue || P.CategoryId ==productSpecParams.categoryid)
        )
        {

           
            if (!string.IsNullOrEmpty(productSpecParams.sort))
                switch (productSpecParams.sort)
                {
                    case "priceAsc":
                        AddOrderByAsc(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderByAsc(P => P.Name); break;
                }
            else
                AddOrderByAsc(P => P.Name);
            ApllyPagination(productSpecParams.PageSize, (productSpecParams.Pageindex - 1) * productSpecParams.PageSize);
            AddIncludes();
        }

        public ProductWithBrandandCategory(int id):base(P=>P.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
