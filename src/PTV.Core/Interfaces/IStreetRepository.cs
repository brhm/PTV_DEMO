using NetTopologySuite.Geometries;
using PTV.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Core.Interfaces
{
    public interface IStreetRepository
    {
        Task<IEnumerable<Street>> GetAllAsync();
        Task<Street> GetByIdAsync(Guid id);
        Task AddAsync(Street street);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Street street);
        Task AddPointUsingPostGis(Guid streetId, Coordinate newPoint);
    }
}
