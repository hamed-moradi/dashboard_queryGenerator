using Domain.Application.Repository;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System.Collections.Generic;
using Domain.Application._App;

namespace Domain.Application.Services
{
    public class AppSetting2AppSettingGroupService : BaseService<AppSetting2AppSettingGroup>, IAppSetting2AppSettingGroupService
    {
        #region Constructor

        private readonly IRepository<AppSetting2AppSettingGroup> _repository;

        public AppSetting2AppSettingGroupService(IRepository<AppSetting2AppSettingGroup> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

    }
}