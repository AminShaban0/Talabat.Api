using Talabat.API.Dtos;

namespace Talabat.API.Helper
{
    public class Pagination<T>
    {
        

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageindex, int pageSize,int count, IReadOnlyList<T> data)
        {
            PageIndex = pageindex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }
    }
}
