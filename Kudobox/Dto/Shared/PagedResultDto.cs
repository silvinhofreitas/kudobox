using System.Collections.Generic;

namespace Kudobox.Dto.Shared
{
    public class PagedResultDto
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int TotalItemsCount { get; set; }
        public IEnumerable<object> Items { get; set; }
    }
}