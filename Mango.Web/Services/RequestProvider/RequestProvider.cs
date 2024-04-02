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

        //     public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        //     {
        //try
        //{
        //	HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
        //	HttpRequestMessage message = new();

        //	//message.Headers.Add("Content-Type", "application/json");

        //	message.RequestUri = new Uri(requestDto.URL!);



        //		if (requestDto.Data != null)
        //		{
        //			message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
        //		}






        //	HttpResponseMessage? apiResponse = null;



        //	message.Method = requestDto.MethodType switch
        //	{
        //		MethodType.POST => HttpMethod.Post,
        //		MethodType.DELETE => HttpMethod.Delete,
        //		MethodType.PUT => HttpMethod.Put,
        //		_ => HttpMethod.Get
        //	};




        //	apiResponse = await client.SendAsync(message);

        //	switch (apiResponse.StatusCode)
        //	{
        //		case HttpStatusCode.NotFound:
        //			return new() { IsSuccess = false, Message = "Not Found" };
        //		case HttpStatusCode.Forbidden:
        //			return new() { IsSuccess = false, Message = "Access Denied" };
        //		case HttpStatusCode.Unauthorized:
        //			return new() { IsSuccess = false, Message = "Unauthorized" };
        //		case HttpStatusCode.InternalServerError:
        //			return new() { IsSuccess = false, Message = "Internal Server Error" };
        //		default:
        //			var apiContent = await apiResponse.Content.ReadAsStringAsync();

        //                     ResponseDto responseDto = new ResponseDto()
        //                     {
        //                         IsSuccess = true,
        //				Message = "Coupons retrieved successfully",
        //				Result = apiContent
        //			};
        //			var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        //			return responseDto;
        //	}
        //}
        //catch (Exception ex)
        //{
        //	var dto = new ResponseDto
        //	{
        //		Message = ex.Message.ToString(),
        //		IsSuccess = false
        //	};
        //	return dto;
        //}
        //     }

        public async Task<TResult> PostAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            var content = new StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = await client.PostAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);
            if (httpResponse.StatusCode == HttpStatusCode.Created)
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

        public async Task<TResult> PutAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            var content = new StringContent(JsonSerializer.Serialize(requestDto.Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = await client.PutAsync(new Uri($"{requestDto.URL}"), content).ConfigureAwait(false);
            await HandleResponse(httpResponse);
            return (await httpResponse.Content?.ReadFromJsonAsync<TResult>()!)!;
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
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
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

        public async Task<TResult> GetAllAsync<TResult>(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
            HttpResponseMessage httpResponse = await client.GetAsync(requestDto.URL);
            await HandleResponse(httpResponse);
            return (await httpResponse.Content?.ReadFromJsonAsync<TResult>()!)!;
        }

        public async Task<TResult> GetByIdAsync<TResult>(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
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
