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
    /// Other examples
    /// </summary>
    public static class Others
    {
        #region Async
        /// <summary>
        /// InsertDataSet
        /// </summary>
        public static int InsertDataSet()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("CategoryID", typeof(int));

            dt.Rows.Add("Name1", 1);
            dt.Rows.Add("Name2", 2);
            dt.Rows.Add("Name3", 1);            

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
            {
                var p = new
                {
                    table = dt.AsTableValuedParameter("BasicUDT")                    
                };
                return db.Execute("CustomerInsertUDT", p, commandType: CommandType.StoredProcedure);
            }
        }        
        #endregion Async        
    }
}
