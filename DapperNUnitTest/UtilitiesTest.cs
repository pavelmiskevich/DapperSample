using NUnit.Framework;
using DapperExample.DapperSiteExample;

namespace Tests
{
    [TestFixture]
    public class UtilitiesTest
    {
        private int startIndex = 1;

        [OneTimeSetUp]
        public void Setup()
        {
            //TODO: change to init DB
            Methods.DeleteAndCheckident(startIndex);
            Methods.CategoryInit();
        }

        #region Async
        [Test, Order(2)]
        public void ExecuteAsyncTest()
        {
            Assert.AreEqual(Utilities.ExecuteAsync(), 1);
        }

        [Test, Order(4)]
        public void QueryAsyncTest()
        {
            Assert.Greater(Utilities.QueryAsync(), 0);
        }

        [Test, Order(4)]
        public void QueryFirstAsyncTest()
        {
            Assert.IsNotNull(Utilities.QueryFirstAsync());
        }

        [Test, Order(4)]
        public void QueryFirstOrDefaultAsyncTest()
        {
            Assert.IsNotNull(Utilities.QueryFirstOrDefaultAsync());
        }

        [Test, Order(4)]
        public void QuerySingleAsyncTest()
        {
            Assert.IsNotNull(Utilities.QuerySingleAsync());
        }

        [Test, Order(4)]
        public void QuerySingleOrDefaultAsyncTest()
        {
            Assert.IsNotNull(Utilities.QuerySingleOrDefaultAsync());
        }

        [Test, Order(4)]
        public void QueryMultipleAsyncTest()
        {
            Assert.Greater(Utilities.QueryMultipleAsync().Count, 0);
        }
        #endregion Async

        #region Buddered
        [Test, Order(4)]
        public void BufferedTest()
        {
            Assert.Greater(Utilities.Buffered(), 0);
        }
        #endregion Buddered

        #region Transaction
        [Test, Order(2)]
        public void TransactionTest()
        {
            Assert.AreEqual(Utilities.Transaction(), 0);
        }

        [Test, Order(2)]
        public void TransactionScopeTest()
        {
            Assert.AreEqual(Utilities.TransactionScope(), 1);
        }

        [Test, Order(2)]
        public void DapperTransactionTest()
        {
            Assert.AreEqual(Utilities.DapperTransaction(), 1);
        }
        #endregion Transaction

        [OneTimeTearDown]
        public void Clear()
        {
            Methods.DeleteAndCheckident(startIndex);
        }
    }
}