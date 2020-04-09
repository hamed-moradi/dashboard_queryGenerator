using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class ReceivedSmsService : BaseService<ReceivedSms>, IReceivedSmsService
    {
        public ReceivedSmsService(IRepository<ReceivedSms> repository) : base(repository)
        {
        }
    }
}
