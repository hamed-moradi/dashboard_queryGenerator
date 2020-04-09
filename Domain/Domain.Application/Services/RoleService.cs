using Domain.Application.Repository;
using System.Collections.Generic;
using System.Transactions;

using Dapper;
using Domain.Model;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System;
using Asset.Infrastructure.Common;

namespace Domain.Application.Services
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        #region Constructor

        private readonly IRepository<Role> _repository;
        private readonly IPermissionService _permissionService;

        public RoleService(IRepository<Role> repository, IPermissionService permissionService) : base(repository)
        {
            _repository = repository;
            _permissionService = permissionService;
        }

        #endregion

        public Role GetByIdIncludePermissions(int id)
        {
            var query = $"SELECT * FROM [{nameof(Role)}] WITH(NOLOCK) " +
                        $"WHERE [{nameof(Role.Id)}]=@{nameof(Role.Id)} and [{nameof(Role.Status)}]!={(byte) GeneralEnums.Status.Deleted}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Entity.Id), id);
            var result = _repository.QueryFirstOrDefault(query, parameters);
            if (result != null)
            {
                result.Role2Modules = _permissionService.GetByRoleId(id);
            }
            return result;
        }

        public int SavePermissions(int roleId, IList<Role2Module> permissions)
        {
            var theRole = GetById(roleId,false);
            if (theRole == null)
                throw new Exception(GeneralMessages.NotFound, new Exception { Source = GeneralMessages.ExceptionSource });
            if (permissions != null && permissions.Count >= 0)
            {
                using (var transactionScope = new TransactionScope())
                {
                    _permissionService.DeleteByRoleId(roleId);
                    var result = _permissionService.BulkInsert(permissions);
                    transactionScope.Complete();
                    return result;
                }
            }
            return _permissionService.DeleteByRoleId(roleId);
        }
    }
}