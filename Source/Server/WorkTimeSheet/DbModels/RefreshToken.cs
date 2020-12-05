using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime IssueDateTime { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}
