using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class EventService : BaseService<Event>, IEventService
    {
        public EventService(IRepository<Event> repository) : base(repository)
        {
        }
    }
}
