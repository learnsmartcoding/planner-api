using System.ComponentModel.DataAnnotations;

namespace Planner.Core.ViewModels
{
    public class UserPlanSchedules
    {
        public string Notes { get; set; }
        public int PlanDateNoteId { get; set; }
        public List<UserDayPlanSchedule> UserDayPlanSchedules { get; set; }
        public UserPlanSchedules()
        {
            UserDayPlanSchedules = new List<UserDayPlanSchedule>();
        }
    }

    public class UserDayPlanSchedule
    {
        public short? OrderId { get; set; }
        public short TimeSlotId { get; set; }
        public int CategoryId { get; set; }
        public string SlotName { get; set; }
        public int PlanScheduleId { get; set; }
        public int TaskPriority { get; set; }
        public bool IsDone { get; set; }
        public int PlanDateId { get; set; }
        public DateTime PlanDate { get; set; }
        public int UserProfileId { get; set; }

        [MaxLength(255)]
        public string PlanName { get; set; }
        [MaxLength(500)]
        public string PlanDescription { get; set; }
    }

    public class UserPlanDate
    {
        public int PlanDateId { get; set; }
        public DateTime PlanDate { get; set; }
    }
}
