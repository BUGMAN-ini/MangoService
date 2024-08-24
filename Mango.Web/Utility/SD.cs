namespace Mango.Web.Utility
{
    public class SD  //static details
    {
        public static string CouponAPIBase { get; set; }
		public static string AuthApiBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
