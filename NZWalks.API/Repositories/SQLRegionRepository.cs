using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _db;
        public SQLRegionRepository(NZWalksDbContext db)
        {
            _db = db;
        }
        
        public async Task<List<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(u=> u.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _db.Regions.AddAsync(region);
            await _db.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _db.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await _db.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await _db.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if(existingRegion == null)
            {
                return null;
            }
            
            _db.Regions.Remove(existingRegion);
            await _db.SaveChangesAsync();

            return existingRegion;
            
            
        }
    }
}
