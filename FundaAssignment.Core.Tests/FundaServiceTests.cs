using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;
using FundaAssignment.Core.Providers;
using FundaAssignment.Core.Services;
using FundaAssignment.Core.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace FundaAssignment.Core.Tests
{
    [TestClass]
    public class FundaServiceTests
    {
        private readonly List<Property> _properties = new List<Property>();

        [TestInitialize]
        public void TestInit()
        {
            var generationList = new List<PropertiesGeneratorItem>
            {
                new PropertiesGeneratorItem() { PropertiesCount = 100, SoldPropertiesCount = 10, MakelaarId = 1 },
                new PropertiesGeneratorItem() { PropertiesCount = 50, SoldPropertiesCount = 5, MakelaarId = 2 }
            };
            Enumerable.Range(3, 10).ToList().ForEach(i => generationList.Add(new PropertiesGeneratorItem() { PropertiesCount = 10, SoldPropertiesCount = 1, MakelaarId = i }));
            _properties.AddRange(TestHelper.GenerateProviderResponse(generationList));
        }

        [TestMethod]
        public void TestCorrectAgenciesOrder()
        {
            var providerMock = new Mock<IProvider>();
            providerMock.Setup(m => m.Get(It.IsAny<RequestModel>())).Returns(_properties);

            IService fundaService = new FundaService(providerMock.Object);

            var top10Agencies = fundaService.GetTop10Agencies(new RequestModel()).ToList();

            
            Assert.AreEqual(top10Agencies[1].MakelaarId, 2);
        }


        [TestMethod]
        public void TestIncorrectRequest()
        {
            var fundaProvider = new FundaProvider();
            IService fundaService = new FundaService(fundaProvider);

            var request = new RequestModel()
            {
                Type = "koop",
                Stad = "null"
            };

            var top10Agencies = fundaService.GetTop10Agencies(request).ToList();

            Assert.IsFalse(top10Agencies.Any());
        }
    }
}
