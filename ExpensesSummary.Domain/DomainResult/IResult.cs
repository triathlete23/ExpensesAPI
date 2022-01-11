namespace ExpensesSummary.Domain.DomainResult
{
    public interface IResult
    {
        bool HasError { get; }
        string Error { get; }
    }
}
