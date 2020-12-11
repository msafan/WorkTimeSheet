using System.Collections.Generic;
using WorkTimeSheet.Extensions;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.Models
{
    public class Pagination : IUrlParamsConvertible
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }
        public bool? OverridePageSize { get; set; }

        public static Pagination NoPagination => new Pagination { OverridePageSize = true, PageSize = -1 };

        public string ConvertToUrlParams()
        {
            List<string> urlparams = new List<string>();

            if (PageNumber != null)
                urlparams.Add($"pageNumber={PageNumber.ToUrlParams()}");

            if (PageSize != null)
                urlparams.Add($"pageSize={PageSize.ToUrlParams()}");

            if (OverridePageSize != null)
                urlparams.Add($"overridePageSize={OverridePageSize.ToUrlParams()}");

            return string.Join("&", urlparams);
        }
    }
}