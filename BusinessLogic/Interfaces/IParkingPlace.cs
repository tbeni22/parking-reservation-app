using BusinessLogic.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IParkingPlace
    {
        Task<ParkingPlaceDto> NewParkingPlace(ParkingPlaceDto pp);
        Task<ParkingPlaceDto> GetParkingPlace(int ID);
        Task<ParkingPlaceDto> GetParkingPlace(ParkingPlaceDto parkingPlaceDto);

        Task<List<ParkingPlaceDto>> GetParkingPlacesByName(string name, int limit = int.MaxValue);

        Task<List<ParkingPlaceDto>> GetParkingPlaces(int limit = int.MaxValue);

        Task<ParkingPlaceDto> DeleteParkingPlace(int ID);
        Task<ParkingPlaceDto> DeleteParkingPlace(ParkingPlaceDto place);

        Task<ParkingPlaceDto> UpdateParkingPlace(ParkingPlaceDto pp);
    }
}
