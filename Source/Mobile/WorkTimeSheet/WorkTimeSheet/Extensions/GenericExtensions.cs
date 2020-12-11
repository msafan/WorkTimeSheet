using System.Web;

namespace WorkTimeSheet.Extensions
{
    public static class GenericExtensions
    {
        public static string ToUrlParams(this object obj)
        {
            return HttpUtility.UrlEncode(obj.ToString());
        }
    }
}
