namespace Mango.Web.Utility
{
    public class SD
    {
        public static string? CouponURLBase {  get; set; }
        public static string? AuthAPIBase {  get; set; }
        public enum MethodType
        {
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}
