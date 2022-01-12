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
    public class ExpensesServiceTests_GetAllAsync
    {
        private readonly IExpensesService service;

        private readonly Mock<IExpensesRepository> expensesRepository;

        private readonly User user;
        private readonly ICollection<Expense> expenses;

        public ExpensesServiceTests_GetAllAsync()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.service = new ExpensesService(this.expensesRepository.Object);

            this.user = new User
            {
                Currency = Currency.Rouble,
                Firstname = "Natasha",
                Lastname = "Romanova"
            };

            this.expenses = new List<Expense>
            {
                new Expense
                {
                    Amount = 66.8,
                    Currency = Currency.Rouble,
                    Comment = "Some comment",
                    Date = DateTime.Parse("13/12/2021"),
                    Nature = Domain.Enums.Nature.Restaurant,
                    User = user
                },
                new Expense
                {
                    Amount = 20,
                    Currency = Currency.Rouble,
                    Comment = "Some comment",
                    Date = DateTime.Parse("08/01/2022"),
                    Nature = Domain.Enums.Nature.Misc,
                    User = user
                },
                new Expense
                {
                    Amount = 148.8,
                    Currency = Currency.Rouble,
                    Comment = "Another comment",
                    Date = DateTime.Parse("07/01/2022"),
                    Nature = Domain.Enums.Nature.Hotel,
                    User = user
                }
            };
        }

        [Fact]
        public async void ReturnErrorIfUserIsNull()
        {
            var result = await this.service.GetAllAsync(null);

            Assert.Equal("User's firstname and lastname has to be specified.", result.Error);
        }

        [Theory]
        [InlineData("", "Natasha")]
        [InlineData("Romanova", "")]
        public async void ReturnErrorIfFirstnameOrLastnameIsEmpty(string firstname, string lastname)
        {
            var result = await this.service.GetAllAsync(new User { Firstname = firstname, Lastname = lastname });

            Assert.Equal("User's firstname and lastname has to be specified.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfThereAreNoExpensesForUser()
        {
            var result = await this.service.GetAllAsync(user);

            Assert.Equal("There are no expenses for current user.", result.Error);
        }

        [Fact]
        public async Task GetAllSortedByAmount()
        {
            this.expensesRepository.Setup(mock => mock.GetAllAsync(It.Is<User>(el => el.Firstname == "Natasha" && el.Lastname == "Romanova")))
                .ReturnsAsync(expenses);

            var result = await this.service.GetAllAsync(user);

            Assert.Equal(20, result.Data.ElementAt(0).Amount);
            Assert.Equal(66.8, result.Data.ElementAt(1).Amount);
            Assert.Equal(148.8, result.Data.ElementAt(2).Amount);
        }

        [Fact]
        public async Task GetAllSortedByDate()
        {
            this.expensesRepository.Setup(mock => mock.GetAllAsync(It.Is<User>(el => el.Firstname == "Natasha" && el.Lastname == "Romanova")))
                .ReturnsAsync(expenses);

            var result = await this.service.GetAllAsync(user, "date");

            Assert.Equal(DateTime.Parse("13/12/2021"), result.Data.ElementAt(0).Date);
            Assert.Equal(DateTime.Parse("07/01/2022"), result.Data.ElementAt(1).Date);
            Assert.Equal(DateTime.Parse("08/01/2022"), result.Data.ElementAt(2).Date);
        }
    }
}
