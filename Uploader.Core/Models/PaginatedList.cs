using System.Collections.Generic;

namespace Uploader.Core.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items {  get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int ItemsCount { get; set; }
    }
}
