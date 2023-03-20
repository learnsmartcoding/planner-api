using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class PlanSchedule
    {
        [Key]
        public int PlanScheduleId { get; set; }
        [MaxLength(255)]
        public string PlanName { get; set; }
        [MaxLength(500)]
        public string? PlanDescription { get; set; }
        public bool? IsActive { get; set; }
        public int? TimeSlotId { get; set; }
        public short TaskPriority { get; set; }
        public bool? IsDone { get; set; }
        public int? PlanDateId { get; set; }

        public virtual PlanDates? PlanDate { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
