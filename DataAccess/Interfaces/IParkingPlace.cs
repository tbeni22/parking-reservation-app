using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    internal interface IParkingPlace
    {
        void NewParkingPlace(ParkingPlace pp);
        ParkingPlace GetParkingPlace(int ID);
        void DeleteParkingPlace(int ID);
        void UpdateParkingPlace(ParkingPlace pp);
    }
}
