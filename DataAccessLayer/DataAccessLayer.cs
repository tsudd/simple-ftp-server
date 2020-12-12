using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ConfigProvider;
using Model;

namespace DataAccessLayer
{
    public class DataAccessLayer
    {
        private readonly SqlConnection connection;

        private readonly IParser parser;

        public DataAccessLayer(Model.DataManagerOptions.ConnectionOptions options, IParser parserArg)
        {
            parser = parserArg;

            string connectionString = $"Data Source={options.DataSource};" +
                $" Database={options.Database};" +
                $" User={options.User};" +
                $" Password={options.Password};" +
                $" Integrated Security={options.IntegratedSecurity}";
            using (TransactionScope scope = new TransactionScope())
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                scope.Complete();
            }
        }
    }
}
