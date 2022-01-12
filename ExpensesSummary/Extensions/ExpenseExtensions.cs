using ExpensesSummary.Api.Models;
using System;

namespace ExpensesSummary.Api.Extensions
{
    public static class ExpenseExtensions
    {
        public static Expense ToApiResponse(this Domain.Models.Expense expense)
        {
            if (expense == null)
            {
                return null;
            }

            return new Expense
            {
                Amount = expense.Amount,
                Comment = expense.Comment,
                Currency = expense.Currency,
                Date = expense.Date,
                Nature = Enum.GetName(typeof(Domain.Enums.Nature), expense.Nature),
                User = $"{expense.User.Firstname} {expense.User.Lastname}"                
            };
        }
    }
}
