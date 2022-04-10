using Microsoft.EntityFrameworkCore;
using SessionService.Entities;
using SessionService.Interfaces;
using SessionService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionService.Repository
{
    public class SegmentRepository : GenericRepository<Segment>, ISegmentRepository
    {
        public SegmentRepository(TgyDatabaseContext context): base(context)
        {

        }

        //public async Task Add(Segment segment)
        //{
        //    databaseContext.Segments.Add(segment);
        //    await databaseContext.SaveChangesAsync();
        //}

        //public async Task Delete(int id)
        //{
        //    databaseContext.Segments.Remove(databaseContext.Segments.Find(id));
        //    await databaseContext.SaveChangesAsync();
        //}

        //public async Task<List<Segment>> GetAll()
        //{
        //    return await databaseContext.Segments.ToListAsync();
        //}

        //public async Task<Segment> GetById(int id)
        //{
        //    return await databaseContext.Segments.FindAsync(id);
        //}

        //public async Task<Segment> GetByName(string name)
        //{
        //    return await databaseContext.Segments.FirstOrDefaultAsync(c => c.Code == name);
        //}

        //public async Task Update(int id, Segment segment)
        //{
        //    databaseContext.Entry(segment).State = EntityState.Modified;
        //    await databaseContext.SaveChangesAsync();
        //}
    }
}
