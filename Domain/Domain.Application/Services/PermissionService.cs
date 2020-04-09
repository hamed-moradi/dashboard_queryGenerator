using Domain.Application.Repository;
using System.Collections.Generic;
using System.Linq;

using Dapper;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System;
using Asset.Infrastructure.Common;

namespace Domain.Application.Services
{
    public class PermissionService : BaseService<Role2Module>, IPermissionService
    {
        #region Constructor

        private readonly IRepository<Role2Module> _repository;

        public PermissionService(IRepository<Role2Module> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public IEnumerable<Role2Module> GetByRoleId(int roleId)
        {
            var query = $"SELECT * FROM [{nameof(Role2Module)}] WITH(NOLOCK) WHERE [{nameof(Role2Module.RoleId)}]=@{nameof(Role2Module.RoleId)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Role2Module.RoleId), roleId);
            var result = _repository.Query(query, parameters);
            return result;
        }

        public override int BulkInsert(IList<Role2Module> listModel)
        {
            if (listModel.Any(item => !item.RoleId.HasValue || !item.ModuleId.HasValue))
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
            var query = $"Insert [{nameof(Role2Module)}]([{nameof(Role2Module.RoleId)}],[{nameof(Role2Module.ModuleId)}]) VALUES(@{nameof(Role2Module.RoleId)},@{nameof(Role2Module.ModuleId)})";
            var result= _repository.Execute(query, listModel);
            return result;
        }

        public int DeleteByRoleId(int roleId)
        {
            var query = $"DELETE FROM [{nameof(Role2Module)}] WHERE [{nameof(Role2Module.RoleId)}]=@{nameof(Role2Module.RoleId)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Role2Module.RoleId), roleId);
            var result = _repository.Execute(query, parameters);
            return result;
        }
    }
}