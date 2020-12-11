using System.Collections.Generic;
using WorkTimeSheet.Extensions;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.Models
{
    public class ProjectFilterModel : IUrlParamsConvertible
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

        public string ConvertToUrlParams()
        {
            List<string> urlParams = new List<string>();

            if (!string.IsNullOrEmpty(Name))
                urlParams.Add($"name={Name.ToUrlParams()}");

            if (!string.IsNullOrEmpty(Description))
                urlParams.Add($"description={Description.ToUrlParams()}");

            if (UserId != null)
                urlParams.Add($"userId={UserId.ToUrlParams()}");

            return string.Join("&", urlParams);
        }
    }
}
