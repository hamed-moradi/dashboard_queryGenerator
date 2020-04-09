using System.Collections.Generic;
using Domain.Model;

namespace Domain.Application.BaseService
{
    public interface IBaseService<T> where T : IBaseEntity
    {
        IEnumerable<T> GetPaging(T model, out long totalCount, string orderBy = "Id", string order = "Desc", int skip = 0, int take = 10);
        IEnumerable<T> Find(T model);
        T Single(T model, bool additionalInfo = false);
        T GetById(int id, bool additionalInfo = true);
        int Insert(T model);

        int InsertWithLog(T model, int adminId, string data = "");
        int UpdateWithLog(T model, int adminId, string data = "");



        int BulkInsert(IList<T> model);
        int Update(T model);
        int Delete(T model);
        int Delete(IEnumerable<long> ids);
    }
}