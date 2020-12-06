using System;

namespace WorkTimeSheet.DTO
{
    public class CurrentWorkDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartDateTime { get; set; }
    }
}