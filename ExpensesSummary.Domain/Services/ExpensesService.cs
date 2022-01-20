﻿using ExpensesSummary.Domain.DomainResult;
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

        public async Task<Result<Guid>> CreateAsync(Expense expense)
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

            var user = await this.expensesRepository.GetUserAsync(expense.UserId);
            if (user == null)
            {
                return ResultError.WithError($"Expense with the amount {expense.Amount} and for the date {expense.Date} has been made by an unknown user.");
            }

            if (expense.Currency != user.Currency)
            {
                return ResultError.WithError($"Expense with the amount {expense.Amount} and for the date {expense.Date} has the currency that is different to its user's currency.");
            }

            var id = await this.expensesRepository.CreateAsync(expense);
            return Result<Guid>.WithData(id);
        }

        public async Task<Result<IEnumerable<Expense>>> GetAllAsync(string userId, string sortOption = null)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return ResultError.WithError("UserId has an incorrect format.");
            }

            var expenses = await this.expensesRepository.GetAllAsync(parsedUserId);
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
