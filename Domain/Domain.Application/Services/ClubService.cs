using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;

namespace Domain.Application.Services
{
    public class ClubService : BaseService<Club>, IClubService
    {
        public ClubService(IRepository<Club> repository) : base(repository)
        {
        }
    }
}
