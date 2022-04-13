using SessionService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface ISegmentRepository : IGenericRepository<Segment>
    {
        Task<Segment> GetByName(string name);

        //Task<Segment> GetById(int id);

        //Task<List<Segment>> GetAll();

        //Task Add(Segment segment);

        //Task Update(int id, Segment segment);

        //Task Delete(int id);

    }
}
