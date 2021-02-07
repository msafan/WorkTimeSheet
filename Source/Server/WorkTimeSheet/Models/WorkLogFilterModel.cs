using System;

namespace WorkTimeSheet.Models
{
    public class WorkLogFilterModel
    {
        public string[] Names { get; set; }
        public string[] ProjectNames { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[] UserIds { get; set; }
        public int[] ProjectIds { get; set; }
    }
}
