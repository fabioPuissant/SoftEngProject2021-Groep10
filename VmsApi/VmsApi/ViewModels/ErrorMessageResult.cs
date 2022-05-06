namespace VmsApi.ViewModels
{
    public class ErrorMessageResult
    {

        public ErrorMessageResult(string message = "", string url="")
        {
            Message = message;
            URL = url;
        }

        public string Message { get; set; }
        public string URL { get; set; }
    }
}
