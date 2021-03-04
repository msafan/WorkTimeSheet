using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.Extensions;
using WorkTimeSheet.Interfaces;
using Xamarin.Forms.Internals;

namespace WorkTimeSheet.Models
{
    public class WorkLogFilterModel : IUrlParamsConvertible
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[] UserIds { get; set; }
        public int[] ProjectIds { get; set; }

        public string ConvertToUrlParams()
        {
            List<string> urlParams = new List<string>();

            if (StartDate != null)
            {
                var startDate = StartDate.Value.ToUniversalTime().ToString("ddd, dd MMM yyy HH:mm:ss").ToUrlParams() + " GMT".ToUrlParams();
                urlParams.Add($"startDate={startDate}");
            }

            if (EndDate != null)
            {
                var endDate = EndDate.Value.ToUniversalTime().ToString("ddd, dd MMM yyy HH:mm:ss").ToUrlParams() + " GMT".ToUrlParams();
                urlParams.Add($"endDate={endDate}");
            }

            if (UserIds != null && UserIds.Any())
                UserIds.ForEach(x => urlParams.Add($"userIds={x.ToUrlParams()}"));

            if (ProjectIds != null && ProjectIds.Any())
                ProjectIds.ForEach(x => urlParams.Add($"projectIds={x.ToUrlParams()}"));

            return string.Join("&", urlParams);
        }
    }
}
