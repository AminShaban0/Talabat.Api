namespace Talabat.API.Errors
{
    public class ApiException:ApiResponse
    {
        public string? Details { get; set; }
        public ApiException(int Statuscode , string? massage = null , string? details = null):base(Statuscode , massage )
        {
            Details = details;
        }
    }
}
