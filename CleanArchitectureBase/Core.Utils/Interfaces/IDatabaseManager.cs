using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Core.Utils.Interfaces
{
    public interface IDatabaseManager
    {
        Task<SqlConnection> OpenConnectionAsync();
        Task CloseConnectionAsync();
        Task<string> ExecuteScalarAsync(string query);
        Task<string> ExecuteScalarAsync(string tableName, string columnName, string clauseName, string clauseValue);
        Task<int> ExecuteNonQueryAsync(string query);
        Task<DataSet> ExecuteQueryForDataSetAsync(string query);
        Task<DataTable> ExecuteQueryForDataTableAsync(string query);
        Task<int> ExecuteNonQueryAsync(string tableName, string columnName, object columnValue, string clauseName, string clauseValue);
    }
}
