using System;
using System.Linq;
using ExpensesSummary.Repositories.Models;

namespace ExpensesSummary.Repositories.Context
{
    public static class ExpenseDbInitializer
    {
        public static void Initialize(ExpenseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            context.Users.Add(new User { Id = Guid.NewGuid(), Currency = "Dollar américain", Firstname = "Anthony", Lastname = "Stark" });
            context.Users.Add(new User { Id = Guid.NewGuid(), Currency = "Rouble russe", Firstname = "Natasha", Lastname = "Romanova" });

            context.SaveChanges();
        }
    }
}
