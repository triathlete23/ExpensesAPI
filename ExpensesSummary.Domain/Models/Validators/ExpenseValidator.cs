using ExpensesSummary.Domain.Ports;
using FluentValidation;
using System;

namespace ExpensesSummary.Domain.Models.Validators
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator(IExpensesRepository repository)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Date)
                .ExclusiveBetween(DateTime.Today.AddMonths(-3), DateTime.Now)
                .WithMessage("Expense should be done during the last 3 months.");
            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Expense should contain a comment.");
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Expense's amount should be a positive number.");
            RuleFor(x => x)
                .MustAsync(async (expense, _) =>
                {
                    var user = await repository.GetUserAsync(expense.UserId);
                    return user?.Currency == expense.Currency;
                })
                .WithMessage("Expense's user is unknown or expense's currency is different to user's currency.")
                .MustAsync(async (expense, _) =>
                {
                    var result = await repository.ContainsAsync(expense);
                    return !result;
                })
                .WithMessage("Expense with same date and amount has been already created.");
        }
    }
}
