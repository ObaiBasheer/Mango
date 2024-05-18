
using Mango.Web.Models;

namespace Mango.Web.Services.RequestProvider
{
    public interface IRequestProvider
    {
		Task<ResponseDto> GetAllAsync<TResult>(RequestDto requestDto, bool UseToken = true);
		Task<TResult> GetByCodeAsync<TResult>(RequestDto requestDto, bool UseToken =true );
		Task<TResult> GetByIdAsync<TResult>(RequestDto requestDto, bool UseToken = true);
		Task<ResponseDto> PostAsync<TResult>(RequestDto requestDto, bool UseToken = true);
		Task<TResult> PutAsync<TResult>(RequestDto requestDto, bool UseToken = true);
		Task<ResponseDto> DeleteAsync(RequestDto requestDto, bool UseToken = true);
		//Task<ResponseDto?> SendAsync(RequestDto requestDto);
	}
}
