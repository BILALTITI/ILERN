
namespace Assigment2.Models
{
    public class PagedResult
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public PagedResult() { }

        public PagedResult(int totalItems, int currentPage, int pageSize = 3)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage - 9 > 0)
                {
                    startPage = endPage - 9;
                }
            }

            this.TotalItems = totalItems;
            this.CurrentPage = currentPage; // Fixed incorrect property assignment
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
            this.StartPage = startPage;
            this.EndPage = endPage;
        }

    }

}
