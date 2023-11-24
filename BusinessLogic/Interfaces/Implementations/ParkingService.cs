using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces.Implementations
{
    public class ParkingService : IParkingManagment
    {
        private ParkingContext context;

        public ParkingService(ParkingContext context)
        {
            this.context = context;
        }

        public async Task<ParkingPlaceDto> CreateParkingPlace(string name, bool disabled)
        {
            ParkingPlace parkingPlace = new ParkingPlace() { Name = name, DisabledParking = disabled };
            var created = await context.ParkingPlaces.AddAsync(parkingPlace);
            context.SaveChanges();
            return created.Entity;
        }

        public async Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlace place)
        {
            var delete = await context.ParkingPlaces.FindAsync(place.ID);
            if(delete != null)
            {
                context.ParkingPlaces.Remove(delete);
                await context.SaveChangesAsync();
            }
            return ParkingPlaceDto.FromDataEntity(delete);
        }

        public Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlaceDto place)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ParkingPlaceDto>> FindByFilter(Func<ParkingPlace, bool> predicate, int limit = int.MaxValue)
        {
            var query = from parkingplace in context.ParkingPlaces
                        where predicate(parkingplace)
                        select parkingplace;

            if(query.Count() < limit)
                limit = query.Count();

            var parkingPlaces = await query.Take(limit).ToListAsync();
           
            return parkingPlaces.Select(parkingPlace => ParkingPlaceDto.FromDataEntity(parkingPlace)).ToList();
        }

        public Task<List<ParkingPlaceDto>> FindByFilter(Func<ParkingPlaceDto, bool> predicate, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit = int.MaxValue)
        {
            if(context.ParkingPlaces.Count() < limit)
                limit = context.ParkingPlaces.Count();

            var parkingPlaces = await context.ParkingPlaces.Take(limit).ToListAsync();
            return parkingPlaces.Select(parkingPlace => ParkingPlaceDto.FromDataEntity(parkingPlace)).ToList();
        }

        public async Task<List<ParkingPlaceDto>> GetParkingPlacesByName(string name, int limit = int.MaxValue)
        {
           return await FindByFilter(parkingPlace => parkingPlace.Name.Equals(name), limit);
        }


        public Task<ParkingPlaceDto> UpdateParkingPlaces(ParkingPlaceDto place)
        {
            throw new NotImplementedException();
        }
    }
}
