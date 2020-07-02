using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DapperExample
{
    public static class DataService
    {        
        public static List<Category> GetAllCategory()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                return db.Query<Category>("select Id as Id, Name as Name from Categoryes").ToList();
            }
        }

        public static List<Photo> GetPhotoByCategoryId(int categoryId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                return db.Query<Photo>("GetPhotoByCategoryId", new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure ).ToList();
            }
        }

        public static async Task<List<Category>> GetAllCategoryAsync()
        {
            IEnumerable<Category> categories;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                IEnumerable<Category> x = await db.QueryAsync<Category>("select Id as Id, Name as Name from Categoryes");
                
                return x.ToList();
            }
        }

        public static async Task GetPhotoByCategoryIdAsync(int categoryId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                await db.QueryAsync<Photo>("GetPhotoByCategoryId", new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
