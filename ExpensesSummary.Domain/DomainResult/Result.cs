using System.Linq;

namespace ExpensesSummary.Domain.DomainResult
{
    public class Result : IResult
    {
        public bool HasError => this.Errors.Any();

        public string[] Errors { get; private set; }
    }

    public class ResultError : IResult
    {
        public bool HasError => this.Errors.Any();

        public string[] Errors { get; private set; }

        public static ResultError WithError(params string[] errors)
        {
            return new ResultError
            {
                Errors = errors
            };
        }
    }

    public class Result<T> : IResult
    {
        public bool HasError => this.Errors.Any();

        public string[] Errors { get; private set; }

        public T Data { get; private set; }

        public static Result<T> WithData(T data)
        {
            return new Result<T>
            {
                Data = data,
                Errors = new string[0]
            };
        }

        public static implicit operator Result<T>(ResultError result)
        {
            return new Result<T>
            {
                Errors = result.Errors
            };
        }
    }
}
