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

        Task UpdateParkingPlaces();

        Task<ParkingPlace> DeleteParkingPlace(ParkingPlace place);

        Task<List<ParkingPlace>> GetParkingPlaces(int limit);
        
        Task<List<ParkingPlace>> GetParkingPlacesByName(String name, int limit);

        Task<List<ParkingPlace>> FindByFilter(Func<ParkingPlace, bool> predicate, int limit);

    }
}
