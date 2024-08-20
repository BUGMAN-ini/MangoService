namespace Mango.Web.Utility
{
    public class SD  //static details
    {
        public static string CouponAPIBase { get; set; }
		public static string AuthApiBase { get; set; }
		public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
