using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.DapperSiteExample
{
    /// <summary>
    /// https://dapper-tutorial.net/parameter-anonymous
    /// </summary>
    public static class Parameters
    {
        #region Anonymous
        /// <summary>
        /// Anonymous Single
        /// </summary>
        public static int AnonymousSingle()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql, new { Name = "Name1", CategoryId = 1 });
            }
        }
        
        /// <summary>
        /// Anonymous Many
        /// </summary>
        public static int AnonymousMany()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql,
                    new[]
                    {
                        new { Name = "Name1", CategoryId = 2 },
                        new { Name = "Name2", CategoryId = 1 },
                        new { Name = "Name3", CategoryId = 2 }
                    }
                );
            }
        }
        #endregion Anonymous

        #region Dynamic
        /// <summary>
        /// Dynamic Single
        /// </summary>
        public static int DynamicSingle()
        {
            var sql = @"INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);
                    select @Id = @@IDENTITY";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                parameter.Add("@Name", "Name1", DbType.String, ParameterDirection.Input);
                parameter.Add("@CategoryId", 1, DbType.Int32, ParameterDirection.Input);

                db.Execute(sql, parameter, commandType: CommandType.Text);
                return parameter.Get<int>("@Id");
            }
        }

        /// <summary>
        /// Dynamic Many
        /// </summary>
        public static int DynamicMany()
        {
            var sql = "EXEC CustomerInsert @Name, @CategoryId";

            var parameters = new List<DynamicParameters>();
            var parameter = new DynamicParameters();
            parameter.Add("@Name", "Name1", DbType.String, ParameterDirection.Input);
            parameter.Add("@CategoryId", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add(parameter);
            parameter = new DynamicParameters();
            parameter.Add("@Name", "Name2", DbType.String, ParameterDirection.Input);
            parameter.Add("@CategoryId", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add(parameter);
            parameter = new DynamicParameters();
            parameter.Add("@Name", "Name3", DbType.String, ParameterDirection.Input);
            parameter.Add("@CategoryId", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add(parameter);

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql, parameters, commandType: CommandType.Text);
            }
        }
        #endregion Dynamic

        #region List
        /// <summary>
        /// List
        /// </summary>
        public static List<Customer> List()
        {
            var sql = "SELECT * FROM Customers WHERE CategoryId IN @CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Query<Customer>(sql, new { CategoryId = new[] { 1, 2 } }).ToList();
            }
        }
        #endregion List

        #region String
        /// <summary>
        /// String
        /// </summary>
        public static List<Category> String()
        {
            var sql = "SELECT * FROM Categories WHERE Name = @Name;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Query<Category>(sql, new { Name = new DbString { Value = "CategoryName1", IsFixedLength = false, Length = 250, IsAnsi = true } }).ToList();
            }
        }
        #endregion String

        #region Table-Valued
        /// <summary>
        /// Table-Valued
        /// </summary>
        public static int TableValued()
        {
            try
            {
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
                {
                    db.Execute(@"
                CREATE TABLE [Customer]
                (
                    [CustomerID] [INT] IDENTITY(1,1) NOT NULL,
                    [Code] [VARCHAR](20) NULL,
                    [Name] [VARCHAR](20) NULL,

                    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
                    (
                        [CustomerID] ASC
                    )
                )
                ");
                    db.Execute(@"
                    CREATE TYPE TVP_Customer AS TABLE
                    (
                        [Code] [VARCHAR](20) NULL,
                        [Name] [VARCHAR](20) NULL
                    )
                ");
                    db.Execute(@"
                    CREATE PROCEDURE Customer_Seed
                        @Customers TVP_Customer READONLY
                    AS
                    BEGIN
                        INSERT INTO Customer (Code, Name)
                        SELECT Code, Name
                        FROM @Customers
                    END
                ");
                    var dt = new DataTable();
                    dt.Columns.Add("Code");
                    dt.Columns.Add("Name");

                    for (int i = 0; i < 5; i++)
                    {
                        dt.Rows.Add("Code_" + i, "Name_" + i);
                    }

                    return db.Execute("Customer_Seed", new { Customers = dt.AsTableValuedParameter("TVP_Customer") }, commandType: CommandType.StoredProcedure);
                }
            }
            finally
            {
                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
                {
                    db.Execute(@"DROP TABLE Customer;DROP PROCEDURE Customer_Seed;DROP TYPE TVP_Customer;");
                }
            }            
        }
        #endregion Table-Valued
    }
}
