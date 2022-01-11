using ExpensesSummary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Ports
{
    public interface IUsersRepository
    {
        public Task<User> GetAsync(string lastname, string firstname);
    }
}
