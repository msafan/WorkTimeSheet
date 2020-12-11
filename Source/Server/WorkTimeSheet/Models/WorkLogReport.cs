using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeSheet.DTO;

namespace WorkTimeSheet.Models
{
    public class WorkLogReport
    {
        public long TotalTime { get; set; }
        public PaginatedResults<WorkLogDTO> PaginatedResults { get; set; }
    }
}
