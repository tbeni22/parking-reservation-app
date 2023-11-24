using DataAccess.Data;
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
        Task<ParkingPlace> CreateParkingPlace(String name, bool disabled);

        Task updateParkingPlaces();

        Task<ParkingPlace> DeleteParkingPlace(ParkingPlace place);

        Task<List<ParkingPlace>> GetParkingPlaces(int limit = int.MaxValue);
        
        Task<List<ParkingPlace>> GetParkingPlacesByName(String name, int limit = int.MaxValue);

        Task<List<ParkingPlace>> FindByFilter(Func<ParkingPlace, bool> predicate);

    }
}
