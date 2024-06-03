namespace DTOs.Shared.Responses
{
    public class PagedResponse<T> : Response<T>
    {
        public PagedResponse(T data, int pageNumber, int pageSize, long pgTotal = 0)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
            PgTotal = pgTotal;
        }

        public PagedResponse(string message) : base(message)
        {

        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long PgTotal { get; set; }
    }
}
