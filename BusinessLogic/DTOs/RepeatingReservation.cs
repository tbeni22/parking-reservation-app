using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public enum RepetitionRate
    {
        Weekly, Daily
    }

    public class RepeatingReservation
    {
        public RepetitionRate Rate { get; set; }
        public ReservationDto FirstOccurence { get; set; }
        public int RepeateFor { get; set; }
    }
}
