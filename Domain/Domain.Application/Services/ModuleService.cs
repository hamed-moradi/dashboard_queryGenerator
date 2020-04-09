using Domain.Application.Repository;
using System.Collections.Generic;
using Dapper;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using Asset.Infrastructure.Common;

namespace Domain.Application.Services
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        #region Constructor

        private readonly IRepository<Module> _repository;

        public ModuleService(IRepository<Module> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public IEnumerable<Module> GetByRoleId(int roleId)
        {
            var query = $"SELECT * FROM [{nameof(Module)}] WITH(NOLOCK) " +
                        $"INNER JOIN [{nameof(Role2Module)}] WITH(NOLOCK) ON [{nameof(Module)}].[{nameof(Module.Id)}]=[{nameof(Role2Module)}].[{nameof(Role2Module.ModuleId)}] " +
                        $"WHERE [{nameof(Role2Module)}].[{nameof(Role2Module.RoleId)}]=@{nameof(Role2Module.RoleId)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Role2Module.RoleId), roleId);
            return _repository.Query(query, parameters);
        }

        public IEnumerable<Module> GetByAdminId(int adminId)
        {
            var query = $"SELECT distinct [{nameof(Module)}].* FROM [{nameof(Admin)}] WITH(NOLOCK) " +
                        $"INNER JOIN [{nameof(Role2Module)}] WITH(NOLOCK) ON [{nameof(Admin)}].[{nameof(Admin.RoleId)}]=[{nameof(Role2Module)}].[{nameof(Role2Module.RoleId)}] " +
                        $"INNER JOIN [{nameof(Module)}] WITH(NOLOCK) ON [{nameof(Role2Module)}].[{nameof(Role2Module.ModuleId)}]=[{nameof(Module)}].[{nameof(Module.Id)}] " +
                        $"WHERE [{nameof(Module)}].[{nameof(Module.Status)}]!={(byte) GeneralEnums.Status.Deleted} and [{nameof(Admin)}].[{nameof(Admin.Id)}]=@AdminId";

            var parameters = new DynamicParameters();
            parameters.Add("AdminId", adminId);

            return _repository.Query(query, parameters);
        }

        public IEnumerable<Module> GetParents()
        {
            var query = $@"SELECT * FROM [{nameof(Module)}] WITH(NOLOCK) WHERE [{nameof(Module.ParentId)}]=0 Or [{nameof(Module.ParentId)}] Is Null";

            return _repository.Query(query);
        }
    }
}