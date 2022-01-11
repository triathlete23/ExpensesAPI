using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpensesSummary.Tests
{
    public class ExpensesServiceTests_CreateAsync
    {
        private readonly IExpensesService service;

        private readonly Mock<IExpensesRepository> expensesRepository;
        private readonly Mock<IUsersRepository> usersRepository;

        private readonly List<Expense> expenses;

        public ExpensesServiceTests_CreateAsync()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.usersRepository = new Mock<IUsersRepository>();
            this.service = new ExpensesService(this.expensesRepository.Object, this.usersRepository.Object);

            this.expenses = new List<Expense>
            {
                new Expense
                {
                    Amount = 66.8,
                    Currency = Currency.Dollar,
                    Comment = "Some comment",
                    Date = DateTime.Parse("01/01/2022"),
                    Nature = Domain.Enums.Nature.Restaurant,
                    User = new User()
                    {
                        Currency = Currency.Dollar,
                        Firstname = "Anthony",
                        Lastname = "Stark"
                    }
                }
            };
        }

        [Fact]
        public async void ReturnErrorIfNoExpenses()
        {
            var result = await this.service.CreateAsync(null);

            Assert.Equal("No expenses to create.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfDateIsInFuture()
        {
            this.expenses[0].Date = DateTime.Now.AddDays(1);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Expense's date must be in the past.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfDateIsNotInLastThreeMonths()
        {
            this.expenses.First().Date = DateTime.Now.AddMonths(-4);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Expense should be done during the last 3 months.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfCommentIsEmpty()
        {
            this.expenses[0].Comment = "";

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Comment should not be empty.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfExpenseHasBeenAlreadyCreated()
        {
            this.expensesRepository.Setup(mock => mock.ContainsAsync(this.expenses[0].Amount, this.expenses[0].Date))
                .ReturnsAsync(true);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Expense has been already created.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfExpenseHasBeenDoneByUnknownUser()
        {
            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[0].User.Lastname, this.expenses[0].User.Firstname))
                .ReturnsAsync((User)null);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Expense has been made by an unknown user.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfExpenseCurrencyDiffersToUserCurrency()
        {
            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[0].User.Lastname, this.expenses[0].User.Firstname))
                .ReturnsAsync(new User { Currency = Currency.Rouble });

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("Expense's currency is different to user's currency.", result.Error);
        }

        [Fact]
        public async void CreateAnExpense()
        {
            var id = Guid.NewGuid();
            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[0].User.Lastname, this.expenses[0].User.Firstname))
                .ReturnsAsync(new User { Currency = Currency.Dollar });
            this.expensesRepository.Setup(mock => mock.CreateAsync(It.Is<ICollection<Expense>>(el =>
                el.All(el1 => el1.Amount == this.expenses[0].Amount &&
                el1.User.Lastname == this.expenses[0].User.Lastname &&
                el1.User.Firstname == this.expenses[0].User.Firstname))))
                .ReturnsAsync(new List<Guid> { id });

            var result = await this.service.CreateAsync(this.expenses);

            Assert.False(result.HasError);
            Assert.Equal(id, result.Data.Single());
        }

        [Fact]
        public async void ReturnErrorIfExpensesAreNotCreatedInDb()
        {
            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[0].User.Lastname, this.expenses[0].User.Firstname))
                .ReturnsAsync(new User { Currency = Currency.Dollar });
            this.expensesRepository.Setup(mock => mock.CreateAsync(It.Is<ICollection<Expense>>(el =>
                el.All(el1 => el1.Amount == this.expenses[0].Amount &&
                el1.User.Lastname == this.expenses[0].User.Lastname &&
                el1.User.Firstname == this.expenses[0].User.Firstname))))
                .ReturnsAsync((ICollection<Guid>)null);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.Equal("There is an error during expenses' creation in DB.", result.Error);
        }

        [Fact]
        public async void CreateMultipleExpense()
        {
            this.expenses.Add(
                new Expense
                {
                    Amount = 20,
                    Currency = Currency.Rouble,
                    Comment = "Some comment",
                    Date = DateTime.Parse("08/01/2022"),
                    Nature = Domain.Enums.Nature.Misc,
                    User = new User()
                    {
                        Currency = Currency.Rouble,
                        Firstname = "Natasha",
                        Lastname = "Romanova"
                    }
                });

            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[0].User.Lastname, this.expenses[0].User.Firstname))
                .ReturnsAsync(new User { Currency = Currency.Dollar });

            this.usersRepository.Setup(mock => mock.GetAsync(this.expenses[1].User.Lastname, this.expenses[1].User.Firstname))
                .ReturnsAsync(new User { Currency = Currency.Rouble });

            var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            this.expensesRepository.Setup(mock => mock.CreateAsync(It.IsAny<ICollection<Expense>>())).ReturnsAsync(ids);

            var result = await this.service.CreateAsync(this.expenses);

            Assert.False(result.HasError);
            Assert.Equal(2, result.Data.Count());
            Assert.Equal(ids[0], result.Data.First());
            Assert.Equal(ids[1], result.Data.Last());
        }
    }
}
