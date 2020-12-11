using System;

namespace WorkTimeSheet.Models
{
    public class WorkLogReport
    {
        public long TotalTime { get; set; }
        public PaginatedResults<WorkLog> PaginatedResults { get; set; }
    }
}
