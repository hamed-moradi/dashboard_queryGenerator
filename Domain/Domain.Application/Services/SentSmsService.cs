using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class SentSmsService : BaseService<SentSms>, ISentSmsService
    {
        public SentSmsService(IRepository<SentSms> repository) : base(repository)
        {
        }
    }
}
