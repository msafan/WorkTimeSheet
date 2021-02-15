using System;
using System.Text;

namespace WorkTimeSheet
{
    public class Constants
    {
        public const string UserRoleOwner = "Owner";
        public const string UserRoleProjectManager = "Project Manager";
        public const string UserRoleMember = "Member";
        public const string Authorization = "Authorization";
        public const string ApiKey = "ApiKey";
        public const string Bearer = "Bearer ";

        public static TimeSpan AccessTokenTimeOut => TimeSpan.FromHours(1);
        public static byte[] JwtTokenKey => Encoding.ASCII.GetBytes("123456789qwertyuiop!@#$%^&*()");
    }
}
