using NUnit.Framework;
using DapperExample.DapperSiteExample;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using DapperExample;

namespace Tests
{
    [TestFixture]
    public class ParametersTest
    {
        private int startIndex = 1;

        [OneTimeSetUp]
        public void Setup()
        {
            //for identity .config file name
            //string s1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            //string s = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
            //TODO: change to init DB
            Methods.DeleteAndCheckident(startIndex);
            Methods.CategoryInit();
        }

        #region Anonymous
        [Test, Order(2)]
        public void AnonymousSingleTest()
        {
            Assert.AreEqual(Parameters.AnonymousSingle(), 1);
        }

        [Test, Order(2)]
        public void AnonymousManyTest()
        {
            Assert.AreEqual(Parameters.AnonymousMany(), 3);
        }
        #endregion Anonymous

        #region Dynamic
        [Test, Order(2)]
        public void DynamicSingleTest()
        {
            Assert.AreNotEqual(Parameters.DynamicSingle(), 0);
        }

        [Test, Order(2)]
        public void DynamicManyTest()
        {
            Assert.AreEqual(Parameters.DynamicMany(), 3);
        }
        #endregion Dynamic

        #region List
        [Test, Order(3)]
        public void ListTest()
        {
            Assert.Greater(Parameters.List().Count, 0);
        }
        #endregion Dynamic

        #region String
        [Test, Order(3)]
        public void StringTest()
        {
            Assert.Greater(Parameters.String().Count, 0);
        }
        #endregion String

        #region Table-Valued
        [Test, Order(3)]
        public void TableValuedTest()
        {
            Assert.AreEqual(Parameters.TableValued(), 5);
        }
        #endregion Table-Valued

        [OneTimeTearDown]
        public void Clear()
        {
            Methods.DeleteAndCheckident(startIndex);
        }
    }
}