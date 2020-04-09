using Domain.Application.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Domain.Application.BaseService;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class PageService : BaseService<Page>, IPageService
    {
        #region Constructor

        private readonly IRepository<Page> _repository;

        public PageService(IRepository<Page> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
    }
}