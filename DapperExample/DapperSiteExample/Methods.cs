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
    /// https://dapper-tutorial.net/execute
    /// </summary>
    public static class Methods
    {
        #region Execute
        /// <summary>
        /// Execute Stored Procedure Single
        /// </summary>
        public static int ExecuteStoredProcedureSingle()
        {   
            string sql = "CustomerInsert";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var affectedRows = db.Execute(sql,
                    new { Name = "Name", CategoryId = 1 },
                    commandType: CommandType.StoredProcedure);

                return affectedRows;
            }
        }

        /// <summary>
        /// Execute Stored Procedure Many        
        /// </summary>
        public static int ExecuteStoredProcedureMany()
        {
            string sql = "CustomerInsert";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql,
                    new[]
                    {
                        new { Name = "Name1", CategoryId = 2 },
                        new { Name = "Name2", CategoryId = 2 },
                        new { Name = "Name3", CategoryId = 1 }
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Execute INSERT Single
        /// </summary>
        public static int ExecuteInsertSingle()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql, new { Name = "Name1", CategoryId = 1 });

                //var Customer = db.Query<Customer>("Select * FROM Customer Name = 'Name'").ToList();                
            }
        }

        /// <summary>
        /// Execute INSERT Many        
        /// </summary>
        //TODO: inspect Profiler
        public static int ExecuteInsertMany()
        {
            string sql = "INSERT INTO Customers (Name, CategoryId) Values (@Name, @CategoryId);";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql,
                    new[]
                    {
                        new { Name = "Name1", CategoryId = 1 },
                        new { Name = "Name2", CategoryId = 2 },
                        new { Name = "Name3", CategoryId = 1 }
                    }
                );
            }
        }

        /// <summary>
        /// Execute UPDATE Single
        /// </summary>
        public static Customer ExecuteUpdateSingle(int id, string name)
        {
            string sql = "UPDATE Customers SET Name = @Name WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var affectedRows = db.Execute(sql, new { Id = id, Name = name + id });

                return db.QuerySingleOrDefault<Customer>("Select * FROM Customers WHERE Id = @Id", param: new { Id = id });
            }
        }

        /// <summary>
        /// Execute UPDATE Many        
        /// </summary>
        public static Customer ExecuteUpdateMany(int id, string name)
        {
            string sql = "UPDATE Customers SET Name = @Name WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var affectedRows = db.Execute(sql, 
                    new[] 
                    {
                        new { Id = id, Name = name + id },
                        new { Id = id + 1, Name = name + id + 1 }
                    } 
                );
                return db.QueryFirstOrDefault<Customer>("Select * FROM Customers WHERE Id = @Id", param: new { Id = id });
            }
        }

        /// <summary>
        /// Execute DELETE Single
        /// </summary>
        public static int ExecuteDeleteSingle(int id)
        {
            string sql = "DELETE Customers WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Execute DELETE Many        
        /// </summary>
        public static int ExecuteDeleteMany(int[] id)
        {
            string sql = "DELETE Customers WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.Execute(sql,
                    new[]
                    {
                        new { Id = id[0] },
                        new { Id = id[1] },
                        new { Id = id[2] }
                    }
                );
            }
        }
        #endregion Execute

        /// <summary>
        /// Execute Reader
        /// https://dapper-tutorial.net/execute-reader
        /// </summary>
        public static DataTable ExecuteReader()
        {
            string sql = "SELECT * FROM Customers;";
            
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var reader = db.ExecuteReader(sql);

                DataTable table = new DataTable();
                table.Load(reader);

                return table;
            }
        }

        /// <summary>
        /// Execute Reader Stored Procedure
        /// </summary>
        public static DataTable ExecuteReaderStoredProcedure(int Id)
        {

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var reader = db.ExecuteReader("GetCustomerById", new { Id = Id}, commandType: CommandType.StoredProcedure);

                DataTable table = new DataTable();
                table.Load(reader);

                return table;
            }
        }

        /// <summary>
        /// Execute Scalar
        /// https://dapper-tutorial.net/execute-scalar
        /// </summary>
        public static string ExecuteScalar()
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {                
                return db.ExecuteScalar<string>("SELECT Name FROM Customers WHERE Id = 1;");
            }
        }

        #region Query
        /// <summary>
        /// Query Anonymous
        /// https://dapper-tutorial.net/query
        /// </summary>
        public static object QueryAnonymous()
        {
            string sql = "SELECT TOP 3 * FROM Customers";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var customer = db.Query(sql).FirstOrDefault();
                return customer;
            }
        }

        /// <summary>
        /// Query Strongly Typed
        /// </summary>
        public static int QueryStronglyTyped()
        {
            string sql = "SELECT TOP 3 * FROM Customers";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var customers = db.Query<Customer>(sql).ToList();
                return customers.Count;
            }
        }

        /// <summary>
        /// Query Multi-Mapping (One to One)
        /// </summary>
        public static int QueryMultiMappingOneToOne()
        {
            string sql = "SELECT cus.Id, cus.Name, cat.Id, cat.Name FROM Customers AS cus INNER JOIN Categories AS cat ON cat.Id = cus.CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var customers = db.Query<Customer, Category, Customer>(
                        sql,
                        (customer, category) =>
                        {
                            customer.Category = category;
                            return customer;
                        },
                        splitOn: "Id")
                    .Distinct()
                    .ToList();
                return customers.Count;
                //return 1;
            }
        }

        /// <summary>
        /// Query Multi-Mapping (One to Many)
        /// </summary>
        public static int QueryMultiMappingOneToMany()
        {
            //Need splitOn CustomerId
            //string sql = "SELECT  cat.*, cus.* FROM Categories cat INNER JOIN Customers cus ON cat.Id = cus.Id;";

            //tricks with AS ID
            string sql = "SELECT cat.Id as Id, cat.*, cus.Id as Id, cus.* FROM Categories cat INNER JOIN Customers cus ON cat.Id = cus.CategoryId;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var categoryDictionary = new Dictionary<int, Category>();


                var list = db.Query<Category, Customer, Category>(
                sql,
                (category, customer) =>
                {
                    Category categoryEntry;

                    if (!categoryDictionary.TryGetValue(category.Id, out categoryEntry))
                    {
                        categoryEntry = category;
                        categoryEntry.Customers = new List<Customer>();
                        categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                    }

                    categoryEntry.Customers.Add(customer);
                    return categoryEntry;
                },
                splitOn: "Id")
                .Distinct()
                .ToList();
                return list.Count;
            }
        }
        
        /// <summary>
        /// Query Multi-Type
        /// </summary>
        public static int QueryMultiType()
        {
            string sql = "SELECT Id, Name, CategoryId, CAST(FLOOR((RAND() * 2)) as int) as Wealth FROM Customers";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var customers = new List<Customer>();

                using (var reader = db.ExecuteReader(sql))
                {
                    var cat1CustomerParser = reader.GetRowParser<RichCustomer>();
                    var cat2CustomerParser = reader.GetRowParser<PoorCustomer>();

                    while (reader.Read())
                    {
                        Customer customer;

                        switch (reader.GetInt32(reader.GetOrdinal("Wealth")))
                        {
                            case 0:
                                customer = cat1CustomerParser(reader);
                                break;
                            case 1:
                                customer = cat2CustomerParser(reader);
                                break;
                            default:
                                throw new Exception();
                        }
                        customers.Add(customer);
                    }
                }
                return customers.Count;
            }            
        }

        /// <summary>
        /// QueryFirst Query Anonymous
        /// https://dapper-tutorial.net/queryfirst
        /// </summary>
        public static object QueryFirstQueryAnonymous()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var category = db.QueryFirst(sql, new { Id = 1 });

                return category;
            }
        }

        /// <summary>
        /// QueryFirst Query Strongly Typed
        /// </summary>
        public static Category QueryFirstQueryStronglyTyped()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QueryFirst<Category>(sql, new { Id = 1 });
            }
        }

        /// <summary>
        /// QueryFirstOrDefault Query Anonymous
        /// https://dapper-tutorial.net/queryfirstordefault
        /// </summary>
        public static object QueryFirstOrDefaultQueryAnonymous()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var category = db.QueryFirstOrDefault(sql, new { Id = 1 });

                return category;
            }
        }

        /// <summary>
        /// QueryFirstOrDefault Query Strongly Typed
        /// </summary>
        public static Category QueryFirstOrDefaultQueryStronglyTyped()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QueryFirstOrDefault<Category>(sql, new { Id = 1 });
            }
        }

        /// <summary>
        /// QuerySingle Query Anonymous
        /// https://dapper-tutorial.net/querysingle
        /// </summary>
        public static object QuerySingleQueryAnonymous()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var category = db.QuerySingle(sql, new { Id = 1 });

                return category;
            }
        }

        /// <summary>
        /// QuerySingle Query Strongly Typed
        /// </summary>
        public static Category QuerySingleQueryStronglyTyped()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QuerySingle<Category>(sql, new { Id = 1 });
            }
        }

        /// <summary>
        /// QuerySingleOrDefault Query Anonymous
        /// https://dapper-tutorial.net/querysingleordefault
        /// </summary>
        public static object QuerySingleOrDefaultQueryAnonymous()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var category = db.QuerySingleOrDefault(sql, new { Id = 1 });

                return category;
            }
        }

        /// <summary>
        /// QuerySingleOrDefault Query Strongly Typed
        /// </summary>
        public static Category QuerySingleOrDefaultQueryStronglyTyped()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                return db.QuerySingleOrDefault<Category>(sql, new { Id = 1 });
            }
        }

        /// <summary>
        /// QueryMultiple
        /// </summary>
        public static Category QueryMultiple()
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id; SELECT * FROM Customers WHERE CategoryId = @Id;";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                using (var multi = db.QueryMultiple(sql, new { Id = 1 }))
                {
                    var category = multi.Read<Category>().First();
                    var customers = multi.Read<Customer>().ToList();
                    return category;
                }
            }
        }
        #endregion Query

        #region Service operation
        /// <summary>
        /// Category Init
        /// </summary>
        public static int CategoryInit()
        {
            string sql = "CategoryInsert";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var affectedRows = db.Execute(sql,
                    new[]
                    {
                        new { Name = "CategoryName1" },
                        new { Name = "CategoryName2" },
                    },                    
                    commandType: CommandType.StoredProcedure);

                //return affectedRows;
            }

            sql = "CustomerInsert";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var affectedRows = db.Execute(sql,
                    new[]
                    {
                        new { Name = "Name1", CategoryId = 1 },
                        new { Name = "Name2", CategoryId = 2 },
                    },
                    commandType: CommandType.StoredProcedure);

                return affectedRows;
            }
        }

        /// <summary>
        /// DBCC CHECKIDENT (Customers, RESEED, 1332)
        /// </summary>
        public static void DeleteAndCheckident(int id)
        {
            string sql = @"TRUNCATE TABLE [dbo].[Customers];DBCC CHECKIDENT('Customers', RESEED, 1);
                        ALTER table Customers DROP constraint FK_CUSTOMERS_CATEGORIES;
                        TRUNCATE TABLE [dbo].[Categories];DBCC CHECKIDENT('Categories', RESEED, 1);
                        ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMERS_CATEGORIES] FOREIGN KEY([CategoryId])
                        REFERENCES [dbo].[Categories] ([Id])";

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                db.Execute(sql);
            }            
        }
        #endregion Service operation
    }
}
