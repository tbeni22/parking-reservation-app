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
        Task DeleteParkingPlace(int ID);
        Task<ParkingPlaceDto> UpdateParkingPlace(ParkingPlaceDto pp);
    }
}
