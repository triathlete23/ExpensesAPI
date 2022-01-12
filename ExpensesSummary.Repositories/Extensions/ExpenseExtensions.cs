using ExpensesSummary.Repositories.Models;
using System;

namespace ExpensesSummary.Repositories.Extensions
{
    public static class ExpenseExtensions
    {
        public static Expense ToEntity(this Domain.Models.Expense model)
        {
            if (model == null)
            {
                return null;
            }

            return new Expense
            {
                Id = Guid.NewGuid(),
                Amount = model.Amount,
                Comment = model.Comment,
                Currency = model.Currency,
                Date = model.Date,
                Nature = model.Nature.ToString(),
                UserId = model.User.Id
            };
        }

        public static Domain.Models.Expense ToDomainModel(this Expense entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Domain.Models.Expense
            {
                Amount = entity.Amount,
                Comment = entity.Comment,
                Currency = entity.Currency,
                Date = entity.Date,
                Nature = entity.Nature switch
                {
                    "Restaurant" => Domain.Enums.Nature.Restaurant,
                    "Hotel" => Domain.Enums.Nature.Hotel,
                    "Misc" => Domain.Enums.Nature.Misc,
                    "" => throw new ArgumentOutOfRangeException(nameof(entity.Nature)),
                },
                Id = entity.Id
            };
        }
    }
}
