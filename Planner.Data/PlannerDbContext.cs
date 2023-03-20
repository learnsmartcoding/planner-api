using Microsoft.EntityFrameworkCore;
using Planner.Core.Model;

namespace Planner.Data
{
    public partial class PlannerContext : DbContext
    {
        public PlannerContext()
        {
        }

        public PlannerContext(DbContextOptions<PlannerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; } = null!;
        public virtual DbSet<PlanDates> PlanDates { get; set; } = null!;
        public virtual DbSet<PlanDateNote> PlanDateNotes { get; set; } = null!;
        public virtual DbSet<PlanSchedule> PlanSchedule { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfile { get; set; } = null!;

      
    }
}