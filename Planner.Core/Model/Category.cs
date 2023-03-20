using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class Category
    {
        public Category()
        {
            TimeSlots = new HashSet<TimeSlot>();
        }

        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TimeSlot> TimeSlots { get; set; }
    }
}
