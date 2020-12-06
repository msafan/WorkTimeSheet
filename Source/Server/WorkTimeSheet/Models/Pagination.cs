using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.Models
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool OverridePageSize { get; set; }

        public static Pagination NoPagination => new Pagination { OverridePageSize = true, PageSize = -1 };
        public static Pagination Default => new Pagination { PageNumber = 1, PageSize = 25 };

        public bool HasPagination()
        {
            if (OverridePageSize && PageSize == -1)
                return false;

            return true;
        }

        public bool IsValid()
        {
            if (OverridePageSize && PageSize == -1)
                return true;

            if (PageNumber <= 0)
                return false;

            if (PageSize <= 0)
                return false;

            return true;
        }
    }
}
