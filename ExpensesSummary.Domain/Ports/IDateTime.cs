using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesSummary.Domain.Ports
{
    public interface IDateTime
    {
        DateTime GetNow();
    }
}
