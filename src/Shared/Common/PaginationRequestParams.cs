namespace Shared.Common
{
    public class PaginationRequestParams
    {
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? ((value > 50) ? 50 : value) : 1;
        }
    }
}
