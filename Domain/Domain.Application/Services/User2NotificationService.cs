using Domain.Application.Repository;
using Dapper;
using Domain.Application.BaseService;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class User2NotificationService : BaseService<User2Notification>, IUser2NotificationService
    {
        #region Constructor

        private readonly IRepository<User2Notification> _repository;

        public User2NotificationService(IRepository<User2Notification> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
    }
}