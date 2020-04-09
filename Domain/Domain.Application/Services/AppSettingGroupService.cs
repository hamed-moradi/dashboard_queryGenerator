using Domain.Application.Repository;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System.Collections.Generic;
using Domain.Application._App;

namespace Domain.Application.Services
{
    public class AppSettingGroupService : BaseService<AppSettingGroup>, IAppSettingGroupService
    {
        #region Constructor

        private readonly IRepository<AppSettingGroup> _repository;

        public AppSettingGroupService(IRepository<AppSettingGroup> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion        

    }
}