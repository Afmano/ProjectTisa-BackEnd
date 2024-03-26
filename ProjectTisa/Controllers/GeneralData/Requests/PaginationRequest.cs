using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Requests
{
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
                _pageSize = (value > ControllerConts.MAX_PAGE_SIZE) ? ControllerConts.MAX_PAGE_SIZE : value;
            }
        }
        public List<T> ApplyRequest<T>(List<T> list)
        {
            return list.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
