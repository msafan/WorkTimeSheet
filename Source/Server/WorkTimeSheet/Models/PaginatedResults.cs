using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.Models
{
    public class PaginatedResults<T> where T : class
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
