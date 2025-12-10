namespace OnOffBack
{
    public class ResponseGenericApi<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Error { get; set; }
        public string Message { get; set; }

        public ResponseGenericApi(T data, bool success)
        {
            Data = data;
            Success = success; 
        }
        public ResponseGenericApi(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
        public ResponseGenericApi(string error, bool success)
        {
            Error = error;
            Success = success;
        }
    }
}
