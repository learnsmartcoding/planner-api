using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class PlanDates
    {
        public PlanDates()
        {
            PlanDateNotes = new HashSet<PlanDateNote>();
            PlanSchedules = new HashSet<PlanSchedule>();
        }

        [Key]
        public int PlanDateId { get; set; }
        public DateTime PlanDate { get; set; }
        public int? UserProfileId { get; set; }
        public bool? IsActive { get; set; }

        public virtual UserProfile? UserProfile { get; set; }
        public virtual ICollection<PlanDateNote> PlanDateNotes { get; set; }
        public virtual ICollection<PlanSchedule> PlanSchedules { get; set; }
    }
}
