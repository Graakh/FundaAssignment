using System.Collections.Generic;
using FundaAssignment.Core.Models;

namespace FundaAssignment.Core.Interfaces
{
    public interface IService
    {
        IEnumerable<Makelaar> GetTop10Agencies(RequestModel requestModel);
    }
}
