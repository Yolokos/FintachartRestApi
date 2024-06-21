namespace FintachartRestApi.ThirdPartyServices.Responses.Base
{
	public class BaseResponse : IBaseResponse
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
	}
}
