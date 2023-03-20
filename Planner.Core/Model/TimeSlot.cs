using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            PlanSchedules = new HashSet<PlanSchedule>();
        }

        [Key]
        public int TimeSlotId { get; set; }
        public int? CategoryId { get; set; }
        public string SlotName { get; set; } = null!;
        public string? Description { get; set; }
        public short? OrderId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<PlanSchedule> PlanSchedules { get; set; }
    }
}
