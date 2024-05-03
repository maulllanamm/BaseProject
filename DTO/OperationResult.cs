namespace DTO
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public T Data { get; }

        private OperationResult(bool isSuccess, string errorMessage, T data)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>(true, null, data);
        }

        public static OperationResult<T> Failure(string errorMessage)
        {
            return new OperationResult<T>(false, errorMessage, default);
        }
    }
}
