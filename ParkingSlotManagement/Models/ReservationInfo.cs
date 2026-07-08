using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSlotManagement.Models
{
    public class ReservationInfo
    {
        public int Id {get; set;}
        public int SlotId {get; set;}

        public string VehicleNo {get; set;}
        public DateTime TimeIn { get; set; }   
        public DateTime? TimeOut { get; set; }
        
    }
}