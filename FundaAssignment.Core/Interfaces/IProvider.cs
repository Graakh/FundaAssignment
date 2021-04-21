using FundaAssignment.Core.Models;
using System.Collections.Generic;

namespace FundaAssignment.Core.Interfaces
{
    public interface IProvider
    {
        IEnumerable<Property> Get(RequestModel request);
    }
}
