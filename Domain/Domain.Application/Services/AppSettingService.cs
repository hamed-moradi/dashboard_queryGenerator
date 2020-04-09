using Domain.Application.Repository;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System.Collections.Generic;
using Domain.Application._App;
using System;

namespace Domain.Application.Services
{
    public class AppSettingService : BaseService<AppSetting>, IAppSettingService
    {
        #region Constructor

        private readonly IRepository<AppSetting> _repository;

        public AppSettingService(IRepository<AppSetting> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public IEnumerable<AppSetting> UpdateByKey(AppSetting model)
        {
            var result = QueryGenerator<AppSetting>.UpdateByKey(model);
            var response = _repository.Query(result.Query, result.Parameters);
            return response;
        }
        
    }
}