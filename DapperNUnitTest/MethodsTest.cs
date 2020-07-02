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
    public class MethodsTest
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

        #region Execute
        [Test, Order(2)]
        public void ExecuteStoredProcedureSingleTest()
        {
            Assert.AreEqual(Methods.ExecuteStoredProcedureSingle(), 1);
        }

        [Test, Order(2)]
        public void ExecuteStoredProcedureManyTest()
        {
            Assert.AreEqual(Methods.ExecuteStoredProcedureMany(), 3);
        }

        [Test, Order(2)]
        public void ExecuteInsertSingleTest()
        {
            Assert.AreEqual(Methods.ExecuteInsertSingle(), 1);
        }

        [Test, Order(2)]
        public void ExecuteInsertManyTest()
        {
            Assert.AreEqual(Methods.ExecuteInsertMany(), 3);
        }

        //[TestCase(1333), Order(1)]
        //[TestCase(1334)]
        [Test, Order(3)]
        public void ExecuteUpdateSingleTest()
        {
            Assert.AreEqual(Methods.ExecuteUpdateSingle(1, "Name").Name, "Name1");
        }

        [Test, Order(3)]
        public void ExecuteUpdateManyTest()
        {
            Assert.AreEqual(Methods.ExecuteUpdateMany(2, "Name").Name, "Name2");
        }

        [Test, Order(4)]
        public void ExecuteDeleteSingleTest()
        {
            Assert.AreEqual(Methods.ExecuteDeleteSingle(1), 1);
        }

        [Test, Order(4)]
        public void ExecuteDeleteManyTest()
        {
            Assert.AreEqual(Methods.ExecuteDeleteMany(new int[] { 2, 3, 4 }), 3);
        }
        #endregion Execute

        [Test, Order(3)]
        public void ExecuteReaderTest()
        {
            Assert.GreaterOrEqual(Methods.ExecuteReader().Rows.Count, 1);
        }

        [Test, Order(3)]
        public void ExecuteReaderStoredProcedureTest()
        {
            Assert.GreaterOrEqual(Methods.ExecuteReaderStoredProcedure(1).Rows.Count, 1);
        }

        [Test, Order(3)]
        public void ExecuteScalarTest()
        {
            Assert.AreNotEqual(Methods.ExecuteScalar(), "");
        }

        #region Query
        [Test, Order(3)]
        public void QueryAnonymousTest()
        {
            Assert.IsNotNull(Methods.QueryAnonymous());
        }

        [Test, Order(3)]
        public void QueryStronglyTypedTest()
        {
            Assert.IsNotNull(Methods.QueryStronglyTyped(), "");
        }

        [Test, Order(3)]
        public void QueryMultiMappingOneToOneTest()
        {
            Assert.Greater(Methods.QueryMultiMappingOneToOne(), 0);
        }

        [Test, Order(3)]
        public void QueryMultiMappingOneToManyTest()
        {
            Assert.Greater(Methods.QueryMultiMappingOneToMany(), 0);
        }

        [Test, Order(3)]
        public void QueryMultiTypeTest()
        {            
            Assert.Greater(Methods.QueryMultiType(), 0);
        }

        [Test, Order(3)]
        public void QueryFirstQueryAnonymousTest()
        {
            Assert.NotNull(Methods.QueryFirstQueryAnonymous());
        }

        [Test, Order(3)]
        public void QueryFirstQueryStronglyTypedTest()
        {
            Assert.NotNull(Methods.QueryFirstQueryStronglyTyped());
        }

        [Test, Order(3)]
        public void QueryFirstOrDefaultQueryAnonymousTest()
        {
            Assert.NotNull(Methods.QueryFirstOrDefaultQueryAnonymous());
        }

        [Test, Order(3)]
        public void QueryFirstOrDefaultQueryStronglyTypedTest()
        {
            Assert.NotNull(Methods.QueryFirstOrDefaultQueryStronglyTyped());
        }

        [Test, Order(3)]
        public void QuerySingleQueryAnonymousTest()
        {
            Assert.NotNull(Methods.QuerySingleQueryAnonymous());
        }

        [Test, Order(3)]
        public void QuerySingleQueryStronglyTypedTest()
        {
            Assert.NotNull(Methods.QuerySingleQueryStronglyTyped());
        }

        [Test, Order(3)]
        public void QuerySingleOrDefaultQueryAnonymousTest()
        {
            Assert.NotNull(Methods.QuerySingleOrDefaultQueryAnonymous());
        }

        [Test, Order(3)]
        public void QuerySingleOrDefaultQueryStronglyTypedTest()
        {
            Assert.NotNull(Methods.QuerySingleOrDefaultQueryStronglyTyped());
        }

        [Test, Order(3)]
        public void QueryMultipleTest()
        {
            Assert.AreEqual(Methods.QueryMultiple().Id, 1);
        }
        #endregion Query

        [OneTimeTearDown]
        public void Clear()
        {
            Methods.DeleteAndCheckident(startIndex);
        }
    }
}