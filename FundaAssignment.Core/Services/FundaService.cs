using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace FundaAssignment.Core.Services
{
    public class FundaService: IService
    {
        private IProvider _provider;

        public FundaService(IProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<Makelaar> GetTop10Agencies(RequestModel requestModel)
        {
            IEnumerable<Makelaar> result = null;
            var properties = _provider.Get(requestModel);

            var responsesForSale = properties.Where(p => !p.IsVerkocht);

            result =
                properties
                    .GroupBy(p => p.MakelaarId)
                    .Select(group =>
                        new
                        {
                            Makelaar = new Makelaar() { MakelaarId = group.Key, MakelaarNaam = group.FirstOrDefault().MakelaarNaam },
                            Count = group.Count()
                        })
                     .OrderByDescending(m => m.Count).Take(10).Select(a => a.Makelaar);

            return result;
        }
    }
}
