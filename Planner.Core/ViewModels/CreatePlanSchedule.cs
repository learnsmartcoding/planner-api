using System.ComponentModel.DataAnnotations;

namespace Planner.Core.ViewModels
{
    public class PlanSchdule
    {
        [Required]
        public int TimeSlotId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? PlanName { get; set; }

        [MaxLength(500)]
        public string? PlanDescription { get; set; }

        [Required]
        public short TaskPriority { get; set; }
        [Required]
        public bool IsDone { get; set; }
        [Required]
        public int PlanDateId { get; set; }
       
        public int CategoryId { get; set; }
        public int PlanScheduleId { get; set; }
    }
    public class PlanScheduleModel
    {

        public List<PlanSchdule> PlanSchdules { get; set; }
        public string? Notes { get; set; }
        [Required]
        public DateTime PlanDate { get; set; }
        public int PlanDateId { get; set; } = 0;
    }

  
}
