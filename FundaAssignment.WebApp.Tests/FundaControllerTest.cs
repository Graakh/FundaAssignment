using FundaAssignment.Core.Providers;
using FundaAssignment.Core.Services;
using FundaAssignment.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FundaAssignment.WebApp.Tests
{
    [TestClass]
    public class FundaControllerTest
    {
        private FundaController _fundaController;

        [TestInitialize]
        public void TestInit()
        {
            var provider = new FundaProvider();
            var service = new FundaService(provider);
            _fundaController = new FundaController(service);
        }

        [TestMethod]
        public void TestResponseWithCorrectRequest()
        {
            var result = _fundaController.Get("koop", "amsterdam", "tuin");

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void TestResponseWithIncorrectRequest()
        {
            var result = _fundaController.Get("koop", "null", "tuin");

            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
