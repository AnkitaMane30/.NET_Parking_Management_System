using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSlotManagement.Models
{
    public class SlotInfo
    {
        public int id { get; set; }
        public string slotNumber { get; set;}
        public string location { get; set; }
        public string status {get; set;}
    }
}