using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace ExpensesSummary.Tests
{
    public class ExpensesServiceTests_CreateAsync
    {
        private readonly IExpensesService service;

        private readonly Mock<IExpensesRepository> expensesRepository;
        private readonly Mock<IValidator<Expense>> validator;

        private readonly Expense expense;

        public ExpensesServiceTests_CreateAsync()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.validator = new Mock<IValidator<Expense>>();
            this.service = new ExpensesService(this.expensesRepository.Object, this.validator.Object);

            this.expense = new Expense
            {
                Amount = 66.8,
                Currency = Currency.Dollar,
                Comment = "Some comment",
                Date = DateTime.Parse("01/01/2022"),
                Nature = Domain.Enums.Nature.Restaurant,
                UserId = Guid.NewGuid()
            };            
        }

        [Fact]
        public async void CreateAnExpense()
        {
            var id = Guid.NewGuid();
            this.validator.Setup(mock => mock.ValidateAsync(It.Is<Expense>(el => el.Id == this.expense.Id), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            this.expensesRepository.Setup(mock => mock.CreateAsync(It.Is<Expense>(el => el.Id == this.expense.Id)))
                .ReturnsAsync(id);

            var result = await this.service.CreateAsync(this.expense);

            Assert.False(result.HasError);
            Assert.Equal(id, result.Data);
        }
    }
}
