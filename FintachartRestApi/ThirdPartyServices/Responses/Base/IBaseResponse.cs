namespace FintachartRestApi.ThirdPartyServices.Responses.Base
{
	public interface IBaseResponse
	{
		int StatusCode { get; set; }
		string Message { get; set; }
	}
}
