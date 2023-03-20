using Planner.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Data
{
    public interface IPlanScheduleRepository
    {
        Task<List<TimeSlotDetails>> GetTimeSlotDetailsAsync();
        Task<UserPlanSchedules> GetUserPlanSchedulesAsync(string userId, DateTime day);
        Task<UserPlanSchedules> GetUserPlanSchedulesAsync(string userId, int planId);
        Task<List<UserPlanDate>> GetUserPlanSchedulesAsync(string userId, DateTime fromDate, DateTime toDate);
        Task<List<string>> ValidatePlanScheduleAsync(PlanScheduleModel model, string userId, bool isUpdateValidation = false);
        Task<int> SavePlanSchedule(PlanScheduleModel model, string userId);
        Task<bool> UpdatePlanSchedule(PlanScheduleModel model, string userId);
        Task<bool> PlanDateExistsAsync(string userId, int planId);
        Task<bool> DeletePlanDateRecordsAsync(int planId);
    }
}
