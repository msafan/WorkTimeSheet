using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class CurrentWork
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartDateTime { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
