using Mango.Web.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Mango.Web.Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Mango.Web.Utility;

namespace Mango.Web.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        //We can use this Approach For Simply Initialization and For Lazy Initialization when need the object
        //LazyThreadSafetyMode.ExecutionAndPublication mode provides some thread safety.
        //private readonly Lazy<HttpClient> _httpClient =
        //    new(() =>
        //    {
        //        var httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        return httpClient;
        //    },
        //        LazyThreadSafetyMode.ExecutionAndPublication);

        

        public RequestProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task DeleteAsync(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            await client.DeleteAsync(new Uri($"{requestDto.URL}/")).ConfigureAwait(false);
        }

        public async Task<TResult> GetAllAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            
            var url = new Uri(requestDto.URL);
            HttpResponseMessage  httpResponse= await client.GetAsync(new Uri($"{url}"));
                await HandleResponse(httpResponse);
            TResult result = await httpResponse.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
            return result!;
        }

        public async Task<TResult> PostAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            var content = new  StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.Add("Content-Type", "application/json"); //OR  content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            HttpResponseMessage httpResponse = await client.PostAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);
            TResult result = await httpResponse.Content?.ReadFromJsonAsync<TResult>();
            return result!;
        }

        public async Task<TResult> PutAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            var content = new StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = await client.PutAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);
            TResult result = await httpResponse.Content?.ReadFromJsonAsync<TResult>();
            return result!;
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        

        public async Task<TResult> GetByCodeAsync<TResult>(RequestDto requestDto)
        {
            var url = new RequestDto { URL = $"{SD.CouponURLBase}/items/byCode/" };
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            HttpResponseMessage httpResponse = await client.GetAsync(url.URL);
            await HandleResponse(httpResponse);
            TResult result = await httpResponse.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
            return result!;
        }

        public async Task<TResult> GetByIdAsync<TResult>(RequestDto requestDto)
        {
            var url = new RequestDto { URL = $"{SD.CouponURLBase}/items/byId/" };
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            HttpResponseMessage httpResponse = await client.GetAsync(url.URL);
            await HandleResponse(httpResponse);
            TResult result = await httpResponse.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
            return result!;
        }
    }
}
