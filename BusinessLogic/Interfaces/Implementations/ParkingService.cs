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

        public async Task<ParkingPlace> CreateParkingPlace(string name, bool disabled)
        {
            ParkingPlace parkingPlace = new ParkingPlace() { Name = name, DisabledParking = disabled };
            var created = await context.ParkingPlaces.AddAsync(parkingPlace);
            context.SaveChanges();
            return created.Entity;
        }

        public async Task<ParkingPlace> DeleteParkingPlace(ParkingPlace place)
        {
            var delete = await context.ParkingPlaces.FindAsync(place.ID);
            if(delete != null)
            {
                context.ParkingPlaces.Remove(delete);
                await context.SaveChangesAsync();
            }
            return delete;
        }

        public async Task<List<ParkingPlace>> FindByFilter(Func<ParkingPlace, bool> predicate, int limit = int.MaxValue)
        {
            var query = from parkingplace in context.ParkingPlaces
                        where predicate(parkingplace)
                        select parkingplace;

            if(query.Count() < limit)
                limit = query.Count();

            return await query.Take(limit).ToListAsync();
        }

        public async Task<List<ParkingPlace>> GetParkingPlaces(int limit = int.MaxValue)
        {
            if(context.ParkingPlaces.Count() < limit)
                limit = context.ParkingPlaces.Count();

            return await context.ParkingPlaces.Take(limit).ToListAsync();
        }

        public async Task<List<ParkingPlace>> GetParkingPlacesByName(string name, int limit = int.MaxValue)
        {
           return await FindByFilter(parkingPLace => parkingPLace.Name.Equals(name), limit);
        }

        public async Task UpdateParkingPlaces()
        {
            await context.SaveChangesAsync();
        }
    }
}
