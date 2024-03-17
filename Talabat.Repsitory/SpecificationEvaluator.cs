using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repsitory
{
    public static class SpecificationEvaluator<Tentity> where Tentity : BaseEntity
    {
        public static IQueryable<Tentity> GetQuery(IQueryable<Tentity> Inputquery ,ISpecification<Tentity> spec )
        {
            var query = Inputquery;
            if (spec.Critria is not null)
               query = query.Where(spec.Critria);
            if(spec.OrderAsc is not null)
                query = query.OrderBy(spec.OrderAsc);
            else if(spec.OrderDesc is not null)
                query = query.OrderByDescending(spec.OrderDesc);
            if(spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query ,(currentquery , Includes)=> currentquery.Include(Includes));
            return query;

        }
    }
}
