using ExpensesSummary.Domain.Models;
using System.Linq;

namespace ExpensesSummary.Repositories.Extensions
{
    public static class UserExtensions
    {
        public static User ToDomainModel(this Models.User user)
        {
            if (user == null)
            {
                return null;
            }

            return new User
            {
                Id = user.Id,
                Currency = user.Currency,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Expenses = user.Expenses.Select(el => el.ToDomainModel()).ToList()
            };
        }
    }
}
