using SessionService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionService.Interfaces
{
    public interface ISegmentRepository
    {
        Task<Segment> GetById(int id);

        Task<List<Segment>> GetAll();

        Task Add(Segment segment);

        Task Update(int id, Segment segment);

        Task Delete(int id);

        Task<Segment> GetByName(string name);
    }
}
