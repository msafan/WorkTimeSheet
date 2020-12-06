using System;

namespace WorkTimeSheet.DTO
{
    public class WorkLogDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Remarks { get; set; }
        public long TimeInSeconds { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
    }
}