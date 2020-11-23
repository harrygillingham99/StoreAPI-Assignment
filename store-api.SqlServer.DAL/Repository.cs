using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace store_api.SqlServer.DAL
{
    public abstract class Repository
    {
        private readonly string _connectionString;
        private readonly ILogger<Repository> _logger;

        protected Repository(string connectionString, ILogger<Repository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        protected async Task<T> Do<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return await getData(connection);
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Do experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }
    }
}