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
            var entity = created.Entity;
            return new ParkingPlaceDto() { ID = entity.ID, Name = entity.Name, DisabledParking = entity.DisabledParking, Reservations = new List<Reservation>(entity.Reservations)};
        }

        public async Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlaceDto place)
        {
            var entity = await context.ParkingPlaces.FindAsync(place.ID);
            if(entity != null)
            {
                context.ParkingPlaces.Remove(entity);
                await context.SaveChangesAsync();
            }
            return new ParkingPlaceDto() { ID = entity.ID, Name = entity.Name, DisabledParking = entity.DisabledParking, Reservations = new List<Reservation>(entity.Reservations) }; ;
        }


        public async Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit = int.MaxValue)
        {
            if(context.ParkingPlaces.Count() < limit)
                limit = context.ParkingPlaces.Count();
            var entities = await context.ParkingPlaces.Take(limit).ToListAsync();
            List<ParkingPlaceDto> result = new List<ParkingPlaceDto>();
            foreach (var entity in entities)
                result.Add(new ParkingPlaceDto() { ID = entity.ID, Name = entity.Name, DisabledParking = entity.DisabledParking, Reservations = new List<Reservation>(entity.Reservations) });
            return result;
        }

        public async Task<List<ParkingPlaceDto>> GetParkingPlacesByName(string name, int limit = int.MaxValue)
        {
            var query = from parkingplace in context.ParkingPlaces
                        where parkingplace.Name.Equals(name)
                        select parkingplace;

            if (query.Count() < limit)
                limit = query.Count();

            var entities = await query.Take(limit).ToListAsync();
            List<ParkingPlaceDto> result = new List<ParkingPlaceDto>();
            foreach (var entity in entities)
                result.Add(new ParkingPlaceDto() { ID = entity.ID, Name = entity.Name, DisabledParking = entity.DisabledParking, Reservations = new List<Reservation>(entity.Reservations) });
            return result;
        }

        public async Task UpdateParkingPlaces()
        {
            await context.SaveChangesAsync();
        }
    }
}
