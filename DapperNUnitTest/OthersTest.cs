using NUnit.Framework;
using DapperExample.DapperSiteExample;

namespace Tests
{
    [TestFixture]
    public class OthersTest
    {
        private int startIndex = 1;

        [OneTimeSetUp]
        public void Setup()
        {
            //TODO: change to init DB
            Methods.DeleteAndCheckident(startIndex);
            Methods.CategoryInit();
        }

        #region Others
        [Test, Order(2)]
        public void InsertDataSetTest()
        {
            Assert.AreEqual(Others.InsertDataSet(), 3);
        }
        #endregion Others

        [OneTimeTearDown]
        public void Clear()
        {
            Methods.DeleteAndCheckident(startIndex);
        }
    }
}