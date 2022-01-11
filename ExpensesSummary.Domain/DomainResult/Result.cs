namespace ExpensesSummary.Domain.DomainResult
{
    public class Result : IResult
    {
        public bool HasError => !string.IsNullOrWhiteSpace(this.Error);

        public string ErrorCode { get; private set; }

        public string Error { get; private set; }
    }

    public class ResultError : IResult
    {
        public bool HasError => !string.IsNullOrWhiteSpace(this.Error);

        public string Error { get; private set; }

        public static ResultError WithError(string error)
        {
            return new ResultError
            {
                Error = error
            };
        }
    }

    public class Result<T> : IResult
    {
        public bool HasError => !string.IsNullOrWhiteSpace(this.Error);

        public string Error { get; private set; }

        public T Data { get; private set; }

        public static Result<T> WithData(T data)
        {
            return new Result<T>
            {
                Data = data,
                Error = null
            };
        }

        public static implicit operator Result<T>(ResultError result)
        {
            return new Result<T>
            {
                Error = result.Error
            };
        }
    }
}
