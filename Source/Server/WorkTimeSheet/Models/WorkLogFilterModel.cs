using System;

namespace WorkTimeSheet.Models
{
    public class WorkLogFilterModel
    {
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
    }
}
