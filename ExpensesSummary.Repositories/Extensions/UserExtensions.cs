using ExpensesSummary.Domain.Models;

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
                Lastname = user.Lastname
            };
        }
    }
}
