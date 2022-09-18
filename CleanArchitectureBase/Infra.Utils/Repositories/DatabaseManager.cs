using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Utils.Repositories
{
    public class DatabaseManager<T> : IDatabaseManager where T : DbContext
    {
        public SqlConnection _dbConnection;
        public T _dbContext;
        int timeOut;

        public DatabaseManager(T dbContext)
        {
            _dbContext = dbContext;
            _dbConnection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
            timeOut = 360000;
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                await _dbConnection.OpenAsync();
            }
            return _dbConnection;
        }

        public async Task CloseConnectionAsync()
        {
            if (_dbConnection.State == ConnectionState.Open)
            {
                await _dbConnection.CloseAsync();
            }
        }


        public async Task<string> ExecuteScalarAsync(string query)
        {
            try
            {
                //LogHelper.WriteDbSelectInfo(query);

                _dbConnection = await OpenConnectionAsync();
                var command = new SqlCommand()
                {
                    Connection = _dbConnection,
                    CommandTimeout = timeOut,
                    CommandType = CommandType.Text,
                    CommandText = query
                };

                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                //LogHelper.WriteDbException(exception);
                return "";
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<string> ExecuteScalarAsync(string tableName, string columnName, string clauseName, string clauseValue)
        {
            var query = $"SELECT {columnName} FROM {tableName} WHERE {clauseName} = '{clauseValue}'";
            var result = await ExecuteScalarAsync(query);
            return result;
        }

        public async Task<string> ExecuteScalarAsyncForInClause(string tableName, string columnName, string clauseName, string clauseValue)
        {
            string clauseString = "";

            var stringList = clauseValue.Split(",");
            foreach (var item in stringList)
            {
                clauseString = $"'{item}',";
            }
            clauseString = clauseString.Substring(0, clauseString.Length - 1);

            var query = $"SELECT {columnName} FROM {tableName} WHERE {clauseName} IN ({clauseString})";
            var result = await ExecuteScalarAsync(query);
            return result;
        }

        public async Task<DataTable> ExecuteQueryForDataTableAsync(string query)
        {
            var dataTable = new DataTable();
            try
            {
                //LogHelper.WriteDbSelectInfo(query);

                _dbConnection = await OpenConnectionAsync();
                var command = new SqlCommand()
                {
                    Connection = _dbConnection,
                    CommandTimeout = timeOut,
                    CommandType = CommandType.Text,
                    CommandText = query
                };

                var dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception exception)
            {
                throw exception;
                //LogHelper.WriteDbException(exception);
                //return null;
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<DataSet> ExecuteQueryForDataSetAsync(string query)
        {
            try
            {
                //LogHelper.WriteDbSelectInfo(query);

                var dataSet = new DataSet();
                _dbConnection = await OpenConnectionAsync();
                var command = new SqlCommand()
                {
                    Connection = _dbConnection,
                    CommandTimeout = timeOut,
                    CommandType = CommandType.Text,
                    CommandText = query
                };

                var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception exception)
            {
                //LogHelper.WriteDbException(exception);
                return null;
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string tableName, string columnName, object columnValue, string clauseName, string clauseValue)
        {
            //string clauseString = "";

            //var stringList = clauseValue.Split(",");
            //foreach (var item in stringList)
            //{
            //    clauseString = $"'{item}',";
            //}
            //clauseString = clauseString.Substring(0, clauseString.Length - 1);

            var query = $"UPDATE {tableName} SET  {columnName} = {columnValue} WHERE {clauseName} IN ({clauseValue})";
            return await ExecuteNonQueryAsync(query);
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            try
            {
                //LogHelper.WriteDbInsertInfo(query);

                _dbConnection = await OpenConnectionAsync();
                var command = new SqlCommand()
                {
                    Connection = _dbConnection,
                    CommandTimeout = timeOut,
                    CommandType = CommandType.Text,
                    CommandText = query
                };

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                //LogHelper.WriteDbException(exception);
                return 0;
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }
    }
}
