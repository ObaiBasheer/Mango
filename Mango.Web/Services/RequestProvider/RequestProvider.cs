using Mango.Web.Models;
using System.Net;
using System.Net.Http.Headers;
using Mango.Web.Exceptions;
using System.Text.Json;

namespace Mango.Web.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
     
        public RequestProvider(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> DeleteAsync(RequestDto requestDto, bool UseToken)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            if (UseToken)
            {
                var token = _tokenProvider.GetToken();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var result =   await client.DeleteAsync(new Uri($"{requestDto.URL}/")).ConfigureAwait(false);
            return new ResponseDto { IsSuccess= result.IsSuccessStatusCode, Message= result.ReasonPhrase };
        }

       

        public async Task<TResult> PostAsync<TResult>(RequestDto requestDto, bool UseToken)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");

            if (UseToken)
            {
                var token = _tokenProvider.GetToken();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var content = new StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);
            if (httpResponse.StatusCode == HttpStatusCode.Created || httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                // Return a default instance of TResult or null
                
                return default!;
            }
            else
            {
                // Read and deserialize the response body
                return (await httpResponse.Content?.ReadFromJsonAsync<TResult>()!)!;
            }

        }

        public async Task<TResult> PutAsync<TResult>(RequestDto requestDto, bool UseToken)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");

            if (UseToken)
            {
                var token = _tokenProvider.GetToken();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            var content = new StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = await client.PutAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);

            if (httpResponse.StatusCode == HttpStatusCode.Created || httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                // Return a default instance of TResult or null

                return default!;
            }
            else
            {
                // Read and deserialize the response body
                return (await httpResponse.Content?.ReadFromJsonAsync<TResult>()!)!;
            }
           
           
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



        public async Task<TResult> GetByCodeAsync<TResult>(RequestDto requestDto, bool UseToken)
        {
            
                HttpClient client = _httpClientFactory.CreateClient("CouponAPI");

                if (UseToken)
                {
                    var token = _tokenProvider.GetToken();

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                HttpResponseMessage httpResponse = await client.GetAsync(requestDto.URL);
                await HandleResponse(httpResponse);
                var result = await httpResponse.Content.ReadFromJsonAsync<TResult>();
                Console.WriteLine($"Deserialization failed: {result}");

                return result!;
           

        }

        public async Task<ResponseDto> GetAllAsync<TResult>(RequestDto requestDto, bool UseToken)
        {
            
                HttpClient client = _httpClientFactory.CreateClient("CouponAPI");

                if(UseToken)
                {
                    var token = _tokenProvider.GetToken();

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
                HttpResponseMessage httpResponse = await client.GetAsync(requestDto.URL);
                await HandleResponse(httpResponse);
                if (httpResponse.StatusCode == HttpStatusCode.InternalServerError && httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Return a default instance of TResult or null

                    return new ResponseDto { IsSuccess = httpResponse.IsSuccessStatusCode, Message = httpResponse.ReasonPhrase };
                }
                else
                {
                    // Read and deserialize the response body
                    return new ResponseDto { IsSuccess = httpResponse.IsSuccessStatusCode, Message=httpResponse.ReasonPhrase, Result = await httpResponse.Content?.ReadFromJsonAsync<TResult>()! };
                }
            
            
            
        }

        public async Task<TResult> GetByIdAsync<TResult>(RequestDto requestDto, bool UseToken)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("CouponAPI");

                if (UseToken)
                {
                    var token = _tokenProvider.GetToken();

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
                HttpResponseMessage httpResponse = await client.GetAsync(requestDto.URL);
                await HandleResponse(httpResponse);
                var result = await httpResponse.Content.ReadFromJsonAsync<TResult>();
                Console.WriteLine($"Deserialization failed: {result}");

                return result!;
            }
            catch (Exception ex)
            {


                Console.WriteLine($"Deserialization failed: {ex.Message}");
                throw; // Rethrow the exception to propagate it further if needed
            }
        }
    }
}
