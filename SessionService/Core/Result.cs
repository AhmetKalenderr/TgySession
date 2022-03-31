namespace SessionService.Core
{
    public class Result<T>
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public T data { get; set; }
    }
}
