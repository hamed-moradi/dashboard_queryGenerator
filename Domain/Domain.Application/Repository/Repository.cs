using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Domain.Model;
using Domain.Application._App;
using Domain.Model._App;
using System.Linq;

namespace Domain.Application.Repository
{
    public class Repository<T> : IRepository<T> where T : IBaseEntity
    {
        public IEnumerable<T> Find(T model)
        {
            var result = QueryGenerator<T>.Select(model);
            return Query(result.Query, result.Parameters, false, null, null);
        }

        public T Single(T model, bool additionalInfo)
        {
            var result = QueryGenerator<T>.Select(model);
            return QueryFirstOrDefault(result.Query, result.Parameters, null, null);
        }

        public T GetById(int id, bool additionalInfo)
        {
            var result = QueryGenerator<T>.GetById(id, additionalInfo);
            return QueryFirstOrDefault(result.Query, result.Parameters, null, null);
        }

        public int Insert(T model)
        {
            var result = QueryGenerator<T>.Insert(model);
            return ConnectionKeeper.Instance.QueryFirstOrDefault<int>(result.Query, result.Parameters);
        }

        public int InsertTrans(T model)
        {
            var result = QueryGenerator<T>.Insert(model);
            return ExecuteTrans(result.Query, result.Parameters, null, null);
        }

        public Task<int> InsertAsync(T model)
        {
            var result = QueryGenerator<T>.Insert(model);
            return ExecuteAsync(result.Query, result.Parameters, null, null);
        }

        public int BulkInsert(IList<T> model)
        {
            var result = QueryGenerator<T>.BulkInsert(model);
            return Execute(result.Query, result.Parameters, null, null);
        }

        public int BulkInsertTrans(IList<T> model)
        {
            var result = QueryGenerator<T>.BulkInsert(model);
            return ExecuteTrans(result.Query, result.Parameters, null, null);
        }

        public Task<int> BulkInsertAsync(IList<T> model)
        {
            var result = QueryGenerator<T>.BulkInsert(model);
            return ExecuteAsync(result.Query, result.Parameters, null, null);
        }

        public int Update(T model)
        {
            var result = QueryGenerator<T>.Update(model);
            return Execute(result.Query, result.Parameters, null, null);
        }

        public int UpdateTrans(T model)
        {
            var result = QueryGenerator<T>.Update(model);
            return ExecuteTrans(result.Query, result.Parameters, null, null);
        }

        public Task<int> UpdateAsync(T model)
        {
            var result = QueryGenerator<T>.Update(model);
            return ExecuteAsync(result.Query, result.Parameters, null, null);
        }

        public int Delete(long[] ids)
        {
            var result = QueryGenerator<T>.LogicalDelete(ids);
            return Execute(result.Query, result.Parameters, null, null);
        }

        public int DeleteTrans(long[] ids)
        {
            var result = QueryGenerator<T>.LogicalDelete(ids);
            return ExecuteTrans(result.Query, result.Parameters, null, null);
        }

        public Task<int> DeleteAsync(long[] ids)
        {
            var result = QueryGenerator<T>.LogicalDelete(ids);
            return ExecuteAsync(result.Query, result.Parameters, null, null);
        }

        public T QueryFirstOrDefault(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.QueryFirstOrDefault<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public IEnumerable<T> Query(string sql, object param, bool buffered, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.Query<T>(sql, param, buffered: buffered, commandTimeout: commandTimeout, commandType: commandType);
        }

        public Task<IEnumerable<T>> QueryAsync(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public int Execute(string sql, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.Execute(sql, commandTimeout: commandTimeout, commandType: commandType);
        }

        public int Execute(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.Execute(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }


        public int ExecuteTrans(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
            connection.Open();
            var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            var result = connection.Execute(sql, param, transaction, commandTimeout, commandType);
            try
            {
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                connection.Dispose();
            }
            return result;
        }

        public Task<int> ExecuteAsync(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public object ExecuteScalar(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.ExecuteScalar(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
        public Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.ExecuteScalarAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.QueryMultiple(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param, int? commandTimeout, CommandType? commandType)
        {
            return ConnectionKeeper.Instance.QueryMultipleAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
    }
}