using Domain.Application.Repository;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Asset.Infrastructure.Common;
using Dapper;
using Domain.Model;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System;
using Domain.Application._App;

namespace Domain.Application.Services
{
    public class AdminService : BaseService<Admin>, IAdminService
    {
        #region Constructor

        private readonly IRepository<Admin> _repository;

        public AdminService(IRepository<Admin> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        //public override IEnumerable<Admin> GetPaging(Admin model, out long totalCount, string orderBy, string order, int skip, int take)
        //{
        //    var result = QueryGenerator<Admin>.GetPaging(model, orderBy, order, skip, take);
        //    result.Query += " AND [Admin].id <> 1";
        //    var response = _repository.Query(result.Query, result.Parameters);
        //    totalCount = response.Any() ? response.First().RowsCount : 0;
        //    return response;
        //}

        public Admin GetByIdIncludeRole(int id)
        {
            var query = $"SELECT [{nameof(Admin)}].*,[{nameof(Role)}].[{nameof(Role.Title)}] AS RoleTitle FROM [{nameof(Admin)}] WITH(NOLOCK) " +
                        $"INNER JOIN [{nameof(Role)}] WITH(NOLOCK) ON [{nameof(Admin)}].[{nameof(Admin.RoleId)}]=[{nameof(Role)}].[{nameof(Role.Id)}] " +
                        $"WHERE [{nameof(Admin)}].[{nameof(Admin.Id)}]=@{nameof(Admin.Id)} and [{nameof(Admin)}].[{nameof(Admin.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.Id), id);
            return _repository.QueryFirstOrDefault(query, parameters);
        }

        public Admin GetByCellPhone(string cellPhone)
        {
            var query = $"SELECT * FROM [{nameof(Admin)}] WITH(NOLOCK) WHERE [{nameof(Admin.Phone)}]=@{nameof(Admin.Phone)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.Phone), cellPhone);
            return _repository.QueryFirstOrDefault(query, parameters);
        }

        public Admin SignIn(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
            var query = $"SELECT * FROM [{nameof(Admin)}] WITH(NOLOCK)" +
                        $"WHERE [{nameof(Admin.Username)}]=@{nameof(Admin.Username)} AND" +
                        $"[{nameof(Admin.Password)}]=@{nameof(Admin.Password)} AND" +
                        $"[{nameof(Admin.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.Username), username);
            parameters.Add(nameof(Admin.Password), password);
            var result = _repository.Query(query, parameters);
            return result.Any() ? result.SingleOrDefault() : null;
        }

        public int ChangePassword(Admin model)
        {
            if (!model.Id.HasValue || string.IsNullOrWhiteSpace(model.Password))
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
            var query = $"Update [{nameof(Admin)}] Set " +
                        $"[{nameof(Admin.Password)}]=@{nameof(Admin.Password)} " +
                        $"WHERE [{nameof(Admin.Id)}]=@{nameof(Admin.Id)}";
            var parameters = new DynamicParameters();
            //parameters.Add(nameof(Admin.UpdaterId), model.UpdaterId);
            //parameters.Add(nameof(Admin.UpdatedAt), DateTime.Now);
            parameters.Add(nameof(Admin.Password), model.Password);
            parameters.Add(nameof(Admin.Id), model.Id);
            var result = _repository.Execute(query, parameters);
            return result;
        }

        public int SetLoginDate(Admin model)
        {
            if (!model.Id.HasValue)
            {
                throw new Exception(GeneralMessages.DefectiveEntry, new Exception { Source = GeneralMessages.ExceptionSource });
            }
            var query = $"Update [{nameof(Admin)}] Set [{nameof(Admin.LastLogin)}]=@{nameof(Admin.LastLogin)} WHERE [Id]=@{nameof(Admin.Id)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Admin.LastLogin), DateTime.Now);
            parameters.Add(nameof(Admin.Id), model.Id);
            var result = _repository.Execute(query, parameters);
            return result;
        }
    }
}