using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSheet.Exceptions;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet
{
    public class WebApiLayer : IWebApiLayer
    {
        private readonly IUserSettings _userSettings;

        public WebApiLayer(IUserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        public async Task<string> DeleteAsync(string api)
        {
            var client = GetHttpClient();
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await client.DeleteAsync(api);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
            var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            if (isSuccessStatusCode)
                return await httpResponseMessage.Content.ReadAsStringAsync();

            throw ExceptionFromResponse(httpResponseMessage);
        }

        public async Task<string> GetAsync(string api)
        {
            var client = GetHttpClient();
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await client.GetAsync(api);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
            var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            if (isSuccessStatusCode)
                return await httpResponseMessage.Content.ReadAsStringAsync();

            throw ExceptionFromResponse(httpResponseMessage);
        }

        public async Task<string> PatchAsync(string api, object parameter)
        {
            var client = GetHttpClient();
            HttpResponseMessage httpResponseMessage;

            var request = new HttpRequestMessage(new HttpMethod(Constants.HttpMethodPatch), Constants.BaseUrl + api)
            {
                Content = new StringContent(JsonConvert.SerializeObject(parameter ?? new object()), Encoding.UTF8, Constants.MediaType)
            };
            try
            {
                httpResponseMessage = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
            var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            if (isSuccessStatusCode)
                return await httpResponseMessage.Content.ReadAsStringAsync();

            throw ExceptionFromResponse(httpResponseMessage);
        }

        public async Task<string> PostAsync(string api, object parameter)
        {
            var client = GetHttpClient();
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await client.PostAsync(api, new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, Constants.MediaType));
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
            var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            if (isSuccessStatusCode)
                return await httpResponseMessage.Content.ReadAsStringAsync();

            throw ExceptionFromResponse(httpResponseMessage);
        }

        public async Task<string> PutAsync(string api, object parameter)
        {
            var client = GetHttpClient();
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await client.PutAsync(api, new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, Constants.MediaType));
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
            var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            if (isSuccessStatusCode)
                return await httpResponseMessage.Content.ReadAsStringAsync();

            throw ExceptionFromResponse(httpResponseMessage);
        }

        private HttpClient GetHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback += (requestMessage, certificate, chain, error) => true;

            var httpClient = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromMinutes(10),
                BaseAddress = new Uri(Constants.BaseUrl)
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));
            if (_userSettings.AuthorizedUser != null)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSettings.AuthorizedUser?.AccessToken);
            return httpClient;
        }

        private Exception ExceptionFromResponse(HttpResponseMessage httpResponseMessage)
        {
            var exceptionMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return new WebApiException(exceptionMessage);
        }
    }
}
