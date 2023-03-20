using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Core.ViewModels
{
    public class TimeSlotDetails
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TimeSlotId { get; set; }
        public string SlotName { get; set; }
        public string SlotDescription { get; set; }
        public short SlotOrderId { get; set; }
    }
}
