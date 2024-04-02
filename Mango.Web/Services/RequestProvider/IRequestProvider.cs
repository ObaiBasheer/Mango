
using Mango.Web.Models;

namespace Mango.Web.Services.RequestProvider
{
    public interface IRequestProvider
    {
		Task<TResult> GetAllAsync<TResult>(RequestDto requestDto);
		Task<TResult> GetByCodeAsync<TResult>(RequestDto requestDto);
		Task<TResult> GetByIdAsync<TResult>(RequestDto requestDto);
		Task<TResult> PostAsync<TResult>(RequestDto requestDto);
		Task<TResult> PutAsync<TResult>(RequestDto requestDto);
		Task DeleteAsync(RequestDto requestDto);
		//Task<ResponseDto?> SendAsync(RequestDto requestDto);
	}
}
