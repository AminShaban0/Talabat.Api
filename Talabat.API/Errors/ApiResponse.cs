namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Massage { get; set; }

        public ApiResponse(int Status , string? Mass = null)
        {
            StatusCode = Status;
            Massage = Mass ?? GetDefaultMassage(Status);
        }
        private string? GetDefaultMassage(int Status)
        {
            return StatusCode switch
            {
                400 => "A bad request , you have made ",
                401 => "Authorized , you are not",
                404 => "Resource was not found",
                500 => "Error are the path to the dark side . Error lead to anger .Anger leads to hat",
                _ => null 
            };
        }
    }
}
