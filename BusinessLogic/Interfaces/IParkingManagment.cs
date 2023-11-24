using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IParkingManagment
    {
        Task<ParkingPlaceDto> CreateParkingPlace(String name, bool disabled);

        Task<ParkingPlaceDto> UpdateParkingPlaces(ParkingPlaceDto place);

        Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlaceDto place);

        Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit);
        
        Task<List<ParkingPlaceDto>> GetParkingPlacesByName(String name, int limit);

        Task<List<ParkingPlaceDto>> FindByFilter(Func<ParkingPlaceDto, bool> predicate, int limit);

    }
}
