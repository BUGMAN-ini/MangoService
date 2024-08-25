namespace Mango.Web.Service.IService
{
    public interface ITokerProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
