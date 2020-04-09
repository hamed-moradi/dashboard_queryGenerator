using Asset.Infrastructure._App;
using Dapper;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model;
using Domain.Model._App;
using Domain.Model.Entities;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Domain.Application.Services
{
    public class MatchGroupService : BaseService<MatchGroup>, IMatchGroupService
    {
        public MatchGroupService(IRepository<MatchGroup> repository) : base(repository)
        {
        }
    }
}
