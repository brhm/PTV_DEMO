using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Distance;
using PTV.Application.DTOs;
using PTV.Core.Entities;
using PTV.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Application.Services
{
    public class StreetService
    {
        private readonly IFeatureFlagService _featureFlagService;
        private readonly IStreetRepository _streetRepository;

        public StreetService(IStreetRepository streetRepository, IFeatureFlagService featureFlagService)
        {
            _streetRepository = streetRepository;
            _featureFlagService = featureFlagService;
        }
        public async Task<IEnumerable<StreetDto>> GetAllAsync()
        {
            var result= await _streetRepository.GetAllAsync();
            var resultDTO= result.Select(p=>new StreetDto()
            {
                Id = p.Id,
                Name = p.Name,
                Geometry=p.Geometry?.ToString()

            }).ToList();
            return resultDTO;
        }

        public async Task AddStreetAsync(AddStreetRequest request)
        {
            if (request.StartPoint == null) throw new Exception("StartPoint is not be null");
            if (request.EndPoint == null) throw new Exception("EndPoint is not be null");

            var startPointCoordinate = new Coordinate(request.StartPoint.CoordinateX, request.StartPoint.CoordinateY);
            var endPointCoordinate = new Coordinate(request.EndPoint.CoordinateX, request.EndPoint.CoordinateY);

            var newCoordinates = new Coordinate[] { startPointCoordinate, endPointCoordinate };

            if (newCoordinates == null) throw new Exception("newCoordinates not found");
            if (newCoordinates.Length<2) throw new Exception("newCoordinates muss be >= 2");

            var newLineString = new LineString(newCoordinates);
           
            var street = new Street
            {
                Name = request.Name,
                Geometry= newLineString,
                Capacity = request.Capacity
            };
            await _streetRepository.AddAsync(street);
        }

        public async Task DeleteStreetAsync(Guid id)
        {
            await _streetRepository.DeleteAsync(id);
        }

        public async Task AddPointToStreet(Guid streetId, AddPointRequest request)
        {
            if (request == null)
                throw new Exception("AddPointRequest not found");

            var newPointCoordinate = new Coordinate(request.CoordinateX, request.CoordinateY);
            if (_featureFlagService.IsFeatureEnabled(Core.Enums.FeatureFlag.UseDatabaseForGeometry))
            {
                await _streetRepository.AddPointUsingPostGis(streetId, newPointCoordinate);
            }
            else
            {
                await AddPointUsingBackend(streetId, newPointCoordinate);
            }

        }

        private async Task AddPointUsingBackend(Guid streetId, Coordinate newPointCoordinate)
        {
            // Get Street Record
            var street = await _streetRepository.GetByIdAsync(streetId);
            if (street == null || street.Geometry == null || street.Geometry.IsEmpty)
                throw new Exception("Street not found or has no valid geometry.");

            if (!(street.Geometry is LineString currentLineString))
                throw new Exception("Street geometry must be a LineString.");

            // Mevcut LineString'in koordinatlarını kontrol et
            if (currentLineString.Coordinates == null || currentLineString.Coordinates.Length < 2)
                throw new Exception("Invalid LineString: Must have at least two coordinates.");

            // Başlangıç ve bitiş noktalarını al
            var start = currentLineString.Coordinates.First();
            var end = currentLineString.Coordinates.Last();

            // Yeni noktanın başlangıca ve bitişe uzaklığını hesapla
            var distanceToStart = newPointCoordinate.Distance(start);
            var distanceToEnd = newPointCoordinate.Distance(end);

            // Daha yakın olan tarafa ekle
            if (distanceToStart < distanceToEnd)
            {
                // add to head
                var updatedCoordinates = new[] { newPointCoordinate }
                    .Concat(currentLineString.Coordinates)
                    .ToArray();
                street.Geometry = new LineString(updatedCoordinates);
            }
            else
            {
                // Add to end 
                var updatedCoordinates = currentLineString.Coordinates
                    .Concat(new[] { newPointCoordinate })
                    .ToArray();
                street.Geometry = new LineString(updatedCoordinates);
            }


            // Update DB
            await _streetRepository.UpdateAsync(street);
        }

        public async Task RemovePointToStreet(Guid streetId)
        {
            var street = await _streetRepository.GetByIdAsync(streetId);
            if (street == null) throw new Exception("Street not found.");

            street.Geometry = null;
            await _streetRepository.UpdateAsync(street);
        }
        
    }

}
