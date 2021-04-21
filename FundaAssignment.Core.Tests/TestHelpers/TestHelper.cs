using FundaAssignment.Core.Models;
using System;
using System.Collections.Generic;

namespace FundaAssignment.Core.Tests.TestHelpers
{
    internal static class TestHelper
    {
        static Random _random = new Random(1000);

        public static IEnumerable<Property> GenerateProperties(int propertiesCount, int soldPropertiesCount, int makelaarId = 0)
        {
            var result = new List<Property>();
            var randomAgency = GenerateAgency(makelaarId);
            for (var i = 1; i <= propertiesCount; i++)
            {
                result.Add(
                    new Property() 
                    { 
                        GlobalId = _random.Next(),
                        IsVerkocht = i <= soldPropertiesCount,
                        MakelaarId = randomAgency.MakelaarId,
                        MakelaarNaam = randomAgency.MakelaarNaam
                    });
            }

            return result;
        }

        public static Makelaar GenerateAgency(int id = 0) 
        {
            if (id == 0)
                id = _random.Next();
            
            return new Makelaar() { MakelaarId = id, MakelaarNaam = $"Makelaar{id}" };
        }

        public static IEnumerable<Property> GenerateProviderResponse(List<PropertiesGeneratorItem> items)
        {
            var result = new List<Property>();
            foreach (var item in items)
            {
                result.AddRange(GenerateProperties(item.PropertiesCount, item.SoldPropertiesCount, item.MakelaarId));
            }

            return result;
        }
    }

    internal class PropertiesGeneratorItem
    {
        public int MakelaarId { get; set; }
        public int PropertiesCount { get; set; }
        public int SoldPropertiesCount { get; set; }
    }
}
