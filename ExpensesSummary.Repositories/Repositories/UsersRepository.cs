using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using System.Threading.Tasks;

namespace ExpensesSummary.Repositories.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<User> GetAsync(string lastname, string firstname)
        {
            throw new System.NotImplementedException();
        }
    }
}
