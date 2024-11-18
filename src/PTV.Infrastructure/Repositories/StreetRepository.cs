using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PTV.Core.Entities;
using PTV.Core.Interfaces;
using PTV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Infrastructure.Repositories
{
    public class StreetRepository : IStreetRepository
    {
        private readonly AppDbContext _context;

        public StreetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Street>> GetAllAsync() => await _context.Streets.ToListAsync();

        public async Task<Street> GetByIdAsync(Guid id) => await _context.Streets.FindAsync(id);

        public async Task AddAsync(Street street)
        {
            await _context.Streets.AddAsync(street);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var street = await _context.Streets.FindAsync(id);
            if (street != null)
            {
                _context.Streets.Remove(street);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Street street)
        {
            _context.Streets.Update(street);
            await _context.SaveChangesAsync();
        }
        public async Task AddPointUsingPostGis(Guid streetId, Coordinate newPoint)
        {
            var query = @"
                UPDATE Streets
                SET Geometry = ST_AddPoint(Geometry, ST_MakePoint(@x, @y))
                WHERE Id = @streetId;
            ";
            await _context.Database.ExecuteSqlRawAsync(query, new { x = newPoint.X, y = newPoint.Y, streetId });
        }
    }
}
