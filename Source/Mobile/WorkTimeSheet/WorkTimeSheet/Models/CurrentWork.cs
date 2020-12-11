using System;

namespace WorkTimeSheet.Models
{
    public class CurrentWork
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartDateTime { get; set; }
    }
}
