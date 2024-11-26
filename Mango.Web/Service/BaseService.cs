using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly ITokerProvider _provider;
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory, ITokerProvider provider)
        {
            _httpClientFactory = httpClientFactory;
            _provider = provider;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO request, bool withbearer = true)
        {
            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            if (withbearer)
            {
                var token = _provider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }
            
            //Token
            message.RequestUri = new Uri(request.Url);

            if(request.ContentType == ContentType.MultiPartFormData)
            {
                var content = new MultipartFormDataContent();

                foreach(var prop in request.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(request.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if(file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                }
            }
            else
            {
                if(request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
            }





            if(request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage? response = null;

            switch(request.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            response = await client.SendAsync(message);

            try
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    default:
                        var apicontent = await response.Content.ReadAsStringAsync();
                        var apiresponsedto = JsonConvert.DeserializeObject<ResponseDTO>(apicontent);
                        return apiresponsedto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDTO
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
