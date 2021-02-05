using IdeaSoft.Test.Desktop.UI.Extensions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdeaSoft.Test.Desktop.UI.Services
{
    public class ServiceHandler
    {
        protected async Task<T> CustomDeserializeObjectResponseAsync<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool ErrorHandler(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
