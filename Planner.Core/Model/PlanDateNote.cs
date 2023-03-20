using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class PlanDateNote
    {
        [Key]
        public int PlanDateNoteId { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
        public int? PlanDateId { get; set; }

        public virtual PlanDates? PlanDate { get; set; }
    }
}
