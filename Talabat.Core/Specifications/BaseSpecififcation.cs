using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>> Critria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderAsc { get; set; }
        public Expression<Func<T, object>> OrderDesc { get; set ; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnabled { get ; set ; }

        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> expression)
        {
            Critria = expression;
        }
        public void AddOrderByAsc(Expression<Func<T, object>> expression)
        {
            OrderAsc = expression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> expression)
        {
            OrderDesc = expression;
        }
        public void ApllyPagination(int take , int skip)
        {
            IsPaginationEnabled = true;
            Take = take;
            Skip = skip;
        }
    }
}
