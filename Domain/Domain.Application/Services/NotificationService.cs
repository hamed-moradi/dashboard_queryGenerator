using Domain.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Model.Entities;
using Domain.Model._App;

using Domain.Application.BaseService;


namespace Domain.Application.Services
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        #region Constructor

        private readonly IRepository<Notification> _repository;

        public NotificationService(IRepository<Notification> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
    }
}