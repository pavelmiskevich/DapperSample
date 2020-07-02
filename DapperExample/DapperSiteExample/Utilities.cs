using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper.Transaction;

namespace DapperExample.DapperSiteExample
{
    /// <summary>
    /// https://dapper-tutorial.net/async
    /// </summary>
    public static class Utilities
    {
        #region Async
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        public static int ExecuteAsync()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.ExecuteAsync(sql, new { Name = "Name1", CategoryId = 1 }).Result;
            }
        }

        /// <summary>
        /// QueryAsync
        /// </summary>
        public static int QueryAsync()
        {
            string sql = "SELECT TOP 1 * FROM Customers";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QueryAsync<Customer>(sql).Result.ToList().Count;
            }
        }

        /// <summary>
        /// QueryFirstAsync
        /// </summary>
        public static Customer QueryFirstAsync()
        {
            string sql = "SELECT * FROM Customers WHERE CategoryId = @CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QueryFirstAsync<Customer>(sql, new { CategoryId = 1 }).Result;
            }
        }

        /// <summary>
        /// QueryFirstOrDefaultAsync
        /// </summary>
        public static Customer QueryFirstOrDefaultAsync()
        {
            string sql = "SELECT * FROM Customers WHERE CategoryId = @CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QueryFirstOrDefaultAsync<Customer>(sql, new { CategoryId = 1 }).Result;
            }
        }

        /// <summary>
        /// QuerySingleAsync
        /// </summary>
        public static Customer QuerySingleAsync()
        {
            string sql = "SELECT TOP 1 * FROM Customers WHERE CategoryId = @CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QuerySingleAsync<Customer>(sql, new { CategoryId = 1 }).Result;
            }
        }

        /// <summary>
        /// QuerySingleOrDefaultAsync
        /// </summary>
        public static Customer QuerySingleOrDefaultAsync()
        {
            string sql = "SELECT TOP 1 * FROM Customers WHERE CategoryId = @CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QuerySingleOrDefaultAsync<Customer>(sql, new { CategoryId = 1 }).Result;
            }
        }

        /// <summary>
        /// QueryMultipleAsync
        /// </summary>
        public static List<Customer> QueryMultipleAsync()
        {
            string sql = "SELECT TOP 1 * FROM Customers;SELECT TOP 1 * FROM Categories;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                using (var multi = db.QueryMultipleAsync(sql).Result)
                {
                    var customers = multi.Read<Customer>().ToList();
                    var category = multi.Read<Category>().ToList();
                    return customers;
                }
            }
        }
        #endregion Async

        #region Buddered
        /// <summary>
        /// Buffered
        /// </summary>
        public static int Buffered()
        {
            string sql = "SELECT TOP 1 * FROM Customers;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var customers = db.Query<Customer>(sql, buffered: false).ToList();
                return customers.Count;
            }
        }
        #endregion Buddered

        #region Transaction
        /// <summary>
        /// Transaction
        /// </summary>
        public static int Transaction()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";
            var affectedRows = 0;
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    db.Execute(sql, new { Name = "Name2", CategoryId = 2 }, transaction: transaction);
                    try
                    {
                        affectedRows = db.Execute("Update dbo.Customers set Id = 1", transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                    
                    return affectedRows;
                }
            }
        }

        /// <summary>
        /// TransactionScope
        /// </summary>
        public static int TransactionScope()
        {
            int affectedRows;
            using (var transaction = new TransactionScope())
            {
                var sql = "CustomerInsert";

                using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
                {
                    db.Open();

                    affectedRows = db.Execute(sql,
                        new { Name = "Name2", CategoryId = 2 },
                        commandType: CommandType.StoredProcedure);
                }

                transaction.Complete();
            }
            return affectedRows;
        }

        /// <summary>
        /// Dapper Transaction
        /// </summary>
        public static int DapperTransaction()
        {
            var sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";
            int affectedRows2;
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    // Dapper
                    var affectedRows1 = db.Execute(sql, new { Name = "Name4", CategoryId = 1 }, transaction: transaction);

                    // Dapper Transaction
                    affectedRows2 = transaction.Execute(sql, new { Name = "Name4", CategoryId = 2 });

                    transaction.Commit();
                }
            }
            return affectedRows2;
        }
        #endregion Transaction
    }
}
