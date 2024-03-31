using Microsoft.EntityFrameworkCore;
using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Requests
{
    /// <summary>
    /// Request for Get methods in controllers to paginate answer collection.
    /// </summary>
    public class PaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > ValidationConst.MAX_PAGE_SIZE) ? ValidationConst.MAX_PAGE_SIZE : value;
            }
        }
        public async Task<List<T>> ApplyRequest<T>(IQueryable<T> collection)
        {
            return await collection.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
        }
    }
}
