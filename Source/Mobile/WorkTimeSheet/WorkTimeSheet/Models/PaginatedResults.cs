using System.Collections.Generic;

namespace WorkTimeSheet.Models
{
    public class PaginatedResults<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Items { get; set; }
    }
}
