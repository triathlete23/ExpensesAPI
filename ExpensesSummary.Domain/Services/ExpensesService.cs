using ExpensesSummary.Domain.DomainResult;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Models.Validators;
using ExpensesSummary.Domain.Ports;
using FluentValidation;
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
        private readonly IValidator<Expense> validator;

        public ExpensesService(IExpensesRepository expensesRepository, IValidator<Expense> validator)
        {
            this.expensesRepository = expensesRepository;
            this.validator = validator;
        }

        public async Task<Result<Guid>> CreateAsync(Expense expense)
        {            
            var validation = await this.validator.ValidateAsync(expense);
            if (!validation.IsValid)
            {
                return ResultError.WithErrors(validation.Errors);
            }

            var id = await this.expensesRepository.CreateAsync(expense);
            return Result<Guid>.WithData(id);
        }

        public async Task<Result<IEnumerable<Expense>>> GetAllAsync(string userId, string sortOption = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return ResultError.WithErrors("UserId cannot be empty.");
            }

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return ResultError.WithErrors("UserId has an incorrect format.");
            }

            var expenses = await this.expensesRepository.GetAllAsync(parsedUserId);
            if (expenses == null || !expenses.Any())
            {
                return ResultError.WithErrors("There are no expenses for current user.");
            }

            if (!string.IsNullOrEmpty(sortOption) && sortOption.ToLower() == "date")
            {
                return Result<IEnumerable<Expense>>.WithData(expenses.OrderBy(el => el.Date).ToArray());
            }
            
            return Result<IEnumerable<Expense>>.WithData(expenses.OrderBy(el => el.Amount).ToArray());
        }
    }
}
