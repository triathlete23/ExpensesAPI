using ExpensesSummary.Domain.DomainResult;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository expensesRepository;

        public ExpensesService(IExpensesRepository expensesRepository)
        {
            this.expensesRepository = expensesRepository;
        }

        public async Task<Result<ICollection<Guid>>> CreateAsync(ICollection<Expense> expenses)
        {
            if (expenses == null || !expenses.Any())
            {
                return ResultError.WithError("No expenses to create.");
            }

            foreach (var expense in expenses)
            {
                if (expense.Date >= DateTime.Now)
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and the date {expense.Date} must be in the past.");
                }

                if (expense.Date < DateTime.Today.AddMonths(-3))
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and the date {expense.Date} should be done during the last 3 months.");
                }

                if (string.IsNullOrEmpty(expense.Comment))
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and the date {expense.Date} should contain a comment.");
                }

                var doesExpenseExist = await this.expensesRepository.ContainsAsync(expense.Amount, expense.Date);
                if (doesExpenseExist)
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and for the date {expense.Date} has been already created.");
                }

                var user = await this.expensesRepository.GetUserAsync(expense.User.Lastname, expense.User.Firstname);
                if (user == null)
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and for the date {expense.Date} has been made by an unknown user.");
                }
                expense.User.Id = user.Id;

                if (expense.Currency != user.Currency)
                {
                    return ResultError.WithError($"Expense with the amount {expense.Amount} and for the date {expense.Date} has the currency that is different to its user's currency.");
                }
            }

            var ids = await this.expensesRepository.CreateAsync(expenses);

            if (ids == null)
            {
                return ResultError.WithError("There is an error during expenses' creation in DB.");
            }

            return Result<ICollection<Guid>>.WithData(ids);
        }

        public async Task<Result<IEnumerable<Expense>>> GetAllAsync(User user, string sortOption = null)
        {
            if (user == null || string.IsNullOrEmpty(user.Lastname) || string.IsNullOrEmpty(user.Firstname))
            {
                return ResultError.WithError("User's firstname and lastname has to be specified.");
            }

            var expenses = await this.expensesRepository.GetAllAsync(user);
            if (expenses == null || !expenses.Any())
            {
                return ResultError.WithError("There are no expenses for current user.");
            }

            if (!string.IsNullOrEmpty(sortOption) && sortOption.ToLower() == "date")
            {
                return Result<IEnumerable<Expense>>.WithData(expenses.OrderBy(el => el.Date).ToArray());
            }
            
            return Result<IEnumerable<Expense>>.WithData(expenses.OrderBy(el => el.Amount).ToArray());
        }
    }
}
