using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain.Model;

namespace Domain.Application.Repository
{
    public interface IRepository<T> where T : IBaseEntity
    {
        IEnumerable<T> Find(T model);
        T Single(T model, bool additionalInfo = true);
        T GetById(int id, bool additionalInfo = true);
        int Insert(T model);
        int InsertTrans(T model);
        Task<int> InsertAsync(T model);
        int BulkInsert(IList<T> model);
        int BulkInsertTrans(IList<T> model);
        Task<int> BulkInsertAsync(IList<T> model);
        int Update(T model);
        int UpdateTrans(T model);
        Task<int> UpdateAsync(T model);
        int Delete(long[] ids);
        int DeleteTrans(long[] ids);
        Task<int> DeleteAsync(long[] ids);
        T QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> QueryFirstOrDefaultAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        IEnumerable<T> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<IEnumerable<T>> QueryAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        int Execute(string sql, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        int ExecuteTrans(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        object ExecuteScalar(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout, CommandType? commandType);
        SqlMapper.GridReader QueryMultiple(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
    }
}