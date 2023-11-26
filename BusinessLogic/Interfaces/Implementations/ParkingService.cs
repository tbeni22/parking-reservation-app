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
    public class ParkingService : IParkingPlace
    {
        private ParkingContext context;

        public ParkingService(ParkingContext context)
        {
            this.context = context;
        }

        public async Task<ParkingPlaceDto> NewParkingPlace(ParkingPlaceDto dto)
        {
            ParkingPlace parkingPlace = new ParkingPlace() { Name = dto.Name, DisabledParking = dto.DisabledParking };
            var created = await context.ParkingPlaces.AddAsync(parkingPlace);
            context.SaveChanges();
            var entity = created.Entity;
            return new ParkingPlaceDto()
            {
                ID = entity.ID,
                Name = entity.Name,
                DisabledParking = entity.DisabledParking,
                Reservations = entity.Reservations != null
                 ? entity.Reservations.Select(ReservationDto.FromReservation).ToList()
                 : new List<ReservationDto>()
            };
        }

        public async Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlaceDto place)
        {
            return await DeleteParkingPlace(place.ID);
        }


        public async Task<ParkingPlaceDto> DeleteParkingPlace(int ID)
        {
            var entity = await context.ParkingPlaces.FindAsync(ID);
            if (entity != null)
            {
                context.ParkingPlaces.Remove(entity);
                await context.SaveChangesAsync();
            }
            return new ParkingPlaceDto()
            {
                ID = entity.ID,
                Name = entity.Name,
                DisabledParking = entity.DisabledParking,
                Reservations = entity.Reservations.Select(ReservationDto.FromReservation).ToList()
            }; ;
        }


        public async Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit = int.MaxValue)
        {
            if (context.ParkingPlaces.Count() < limit)
                limit = context.ParkingPlaces.Count();
            var entities = await context.ParkingPlaces.Take(limit).ToListAsync();
            List<ParkingPlaceDto> result = new List<ParkingPlaceDto>();
            foreach (var entity in entities)
            {
                result.Add(new ParkingPlaceDto()
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    DisabledParking = entity.DisabledParking,
                    Reservations = entity.Reservations != null
                 ? entity.Reservations.Select(ReservationDto.FromReservation).ToList()
                 : new List<ReservationDto>()
                });
            }
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
                result.Add(new ParkingPlaceDto()
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    DisabledParking = entity.DisabledParking,
                    Reservations = entity.Reservations != null
                 ? entity.Reservations.Select(ReservationDto.FromReservation).ToList()
                 : new List<ReservationDto>()
                });
            return result;
        }

        public async Task<ParkingPlaceDto> GetParkingPlace(int id)
        {
            var query = from parkingPlace in context.ParkingPlaces
                        where parkingPlace.ID == id
                        select parkingPlace;
            var entity = await query.FirstOrDefaultAsync();
            if (entity != null)
                return new ParkingPlaceDto()
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    DisabledParking = entity.DisabledParking,
                    Reservations = entity.Reservations != null
                 ? entity.Reservations.Select(ReservationDto.FromReservation).ToList()
                 : new List<ReservationDto>()
                };
            else return null;
        }

        public async Task<ParkingPlaceDto> GetParkingPlace(ParkingPlaceDto parkingPlaceDto)
        {
            return await GetParkingPlace(parkingPlaceDto.ID);
        }

        public async Task<ParkingPlace> GetParkingPlaceEntity(int id)
        {
            var query = from parkingPlace in context.ParkingPlaces
                        where parkingPlace.ID == id
                        select parkingPlace;
            return await query.FirstOrDefaultAsync();

        }

        public async Task<ParkingPlaceDto> UpdateParkingPlace(ParkingPlaceDto dto)
        {
            var dao = await GetParkingPlaceEntity(dto.ID);
            if (dao != null)
            {
                dao.DisabledParking = dto.DisabledParking;
                dao.Name = dto.Name;
                //dao.Reservations = new List<Reservation>(dto.Reservations.Select(ReservationDto.ToReservation()));
                var entry = context.Attach(dao);
                entry.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return dto;
            }
            else
            {
                throw new UpdateException();
            }
        }

    }

    public class UpdateException : Exception { }
}