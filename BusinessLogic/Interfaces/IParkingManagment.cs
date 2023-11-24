using BusinessLogic.DTOs;
using DataAccess.Data;

namespace BusinessLogic.Interfaces
{
    public interface IParkingManagment
    {
        Task<ParkingPlaceDto> CreateParkingPlace(String name, bool disabled);

        Task UpdateParkingPlaces();

        Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlace place);

        Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit);
        
        Task<List<ParkingPlaceDto>> GetParkingPlacesByName(String name, int limit);


    }
}
