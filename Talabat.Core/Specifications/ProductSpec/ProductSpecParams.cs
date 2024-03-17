using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpec
{
    public class ProductSpecParams
    {
        private int pagesize = 5 ;
        private const int MaxPageSize =10 ;
        private string? search;


        public string? sort { get; set; }
        public int? brandid { get; set; }
        public int? categoryid { get; set; }
        public int Pageindex { get; set; } = 1;
        

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxPageSize ? MaxPageSize : value; }
        }

    }
}
