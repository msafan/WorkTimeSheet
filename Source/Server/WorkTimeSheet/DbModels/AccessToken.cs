namespace WorkTimeSheet.DbModels
{
    public partial class AccessToken
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public string AppName { get; set; }
        public string ApiKey { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
    }
}
