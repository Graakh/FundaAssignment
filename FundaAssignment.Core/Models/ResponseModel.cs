using System.Collections.Generic;

namespace FundaAssignment.Core.Models
{
    public class ResponseModel
    {
        public List<Property> Objects { get; set; }
        public Paging Paging { get; set; }
    }
}