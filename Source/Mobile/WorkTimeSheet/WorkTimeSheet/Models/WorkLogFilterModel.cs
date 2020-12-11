using System.Collections.Generic;
using WorkTimeSheet.Extensions;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.Models
{
    public class WorkLogFilterModel: IUrlParamsConvertible
    {
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }

        public string ConvertToUrlParams()
        {
            List<string> urlParams = new List<string>();

            if (!string.IsNullOrEmpty(Name))
                urlParams.Add($"name={Name.ToUrlParams()}");

            if (!string.IsNullOrEmpty(ProjectName))
                urlParams.Add($"projectname={ProjectName.ToUrlParams()}");

            if (UserId != null)
                urlParams.Add($"userId={UserId.ToUrlParams()}");

            if (ProjectId != null)
                urlParams.Add($"projectId={ProjectId.ToUrlParams()}");

            return string.Join("&", urlParams);
        }
    }
}
