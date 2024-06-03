namespace Core.DTOs.Shared
{
    public class PaginationParameter
    {
        private int pageNumber;
        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value < 1 ? 1 : value; }
        }

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value < 10 ? 10 : value; }
        }
    }
}
