using System.Collections.Generic;

namespace Kudobox.Dto.Shared
{
    public class PagedResultDto
    {
        public int page { get; set; }
        public int pageCount { get; set; }
        public int pageSize { get; set; }
        public bool hasNext { get; set; }
        public IEnumerable<object> result { get; set; }
    }
}