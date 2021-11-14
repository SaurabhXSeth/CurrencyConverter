namespace Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Error;

        public Result(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
