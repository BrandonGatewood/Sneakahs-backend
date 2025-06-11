namespace Sneakahs.Application.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }

        private Result(bool success, string error, T data)
        {
            Success = success;
            Error = error;
            Data = data;
        }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(true, string.Empty, data);
        }
        public static Result<T> Fail(string error)
        {
            return new Result<T>(false, error, default!);
        }
    }
}