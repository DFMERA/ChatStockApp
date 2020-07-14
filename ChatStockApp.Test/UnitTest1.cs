using NUnit.Framework;
using ChatStockApp.Controllers;
using System.IO;
using System.Threading.Tasks;

namespace ChatStockApp.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TrueIsTrue()
        {
            Assert.True(true);
        }

        [Test]
        public async Task GetSymbolResponse()
        {

            decimal response = await Services.UtilServices.GetStockValueTest("EURUSD");

            Assert.IsTrue(response > 0);
        }

        [Test]
        public async Task GetBotResponse()
        {

            string path = @"..\..\..\TestConfig.txt";
            string response = "";
            bool testResult = true;
            using (StreamReader jsonStream = File.OpenText(path))
            {
                string urlTest = jsonStream.ReadToEnd();
                while (!jsonStream.EndOfStream)
                {
                    var strSymbol = jsonStream.ReadToEnd();

                    response = await new HomeController(null, null, null, null).ListenTest(strSymbol, urlTest);
                    if (response.Length<=0)
                    {
                        testResult = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(testResult);
        }
    }
}