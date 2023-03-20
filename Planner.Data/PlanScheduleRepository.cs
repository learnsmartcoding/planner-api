using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Planner.Core.Model;
using Planner.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Planner.Data
{
    public class PlanScheduleRepository : IPlanScheduleRepository
    {
        private readonly PlannerContext context;

        public PlanScheduleRepository(PlannerContext context)
        {
            this.context = context;
        }

        public async Task<bool> DeletePlanDateRecordsAsync(int planId)
        {
            var entityToDelete = await context.PlanDates.Include(i => i.PlanSchedules)
                 .Include(i => i.PlanDateNotes).Where(w => w.PlanDateId == planId).SingleOrDefaultAsync();

            context.PlanDates.Remove(entityToDelete);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<TimeSlotDetails>> GetTimeSlotDetailsAsync()
        {
            var model = await (from category in context.Category
                               join slots in context.TimeSlots on category.CategoryId equals slots.CategoryId
                               select new TimeSlotDetails()
                               {
                                   CategoryId = category.CategoryId,
                                   CategoryName = category.Name,
                                   SlotDescription = slots.Description ?? string.Empty,
                                   SlotName = slots.SlotName,
                                   SlotOrderId = Convert.ToInt16(slots.OrderId),
                                   TimeSlotId = slots.TimeSlotId

                               }).ToListAsync();

            return model;
        }

        public async Task<UserPlanSchedules> GetUserPlanSchedulesAsync(string userId, DateTime day)
        {
            var data = await (from timeSlots in context.TimeSlots
                              join planSchedule in context.PlanSchedule on timeSlots.TimeSlotId equals planSchedule.TimeSlotId
                              join planDates in context.PlanDates on planSchedule.PlanDateId equals planDates.PlanDateId
                              join planDateNotes in context.PlanDateNotes on planDates.PlanDateId equals planDateNotes.PlanDateId
                              join userProfile in context.UserProfile on planDates.UserProfileId equals userProfile.UserProfileId
                              where userProfile.AdObjectId.ToString().Equals(userId) && planDates.PlanDate.Date == day.Date
                              select new UserDayPlanSchedule()
                              {
                                  PlanName = planSchedule.PlanName,
                                  PlanDescription = planSchedule.PlanDescription,
                                  CategoryId = Convert.ToInt32(timeSlots.CategoryId),
                                  IsDone = Convert.ToBoolean(planSchedule.IsDone),
                                  TimeSlotId = Convert.ToInt16(timeSlots.TimeSlotId),
                                  SlotName = timeSlots.SlotName,
                                  TaskPriority = planSchedule.TaskPriority,
                                  PlanScheduleId = planSchedule.PlanScheduleId,
                                  PlanDateId = planDates.PlanDateId,
                                  PlanDate = Convert.ToDateTime(planDates.PlanDate),
                                  UserProfileId = Convert.ToInt32(planDates.UserProfileId),
                                  OrderId = timeSlots.OrderId
                              }
                         ).ToListAsync();

            var user = await context.UserProfile.SingleOrDefaultAsync(s => s.AdObjectId.ToString().Equals(userId));

            var profileId = user == null ? 0 : user?.UserProfileId;

            var entityPlanDate = await context.PlanDates.Include(i => i.PlanDateNotes)
                .FirstOrDefaultAsync(d => d.PlanDate.Date == day.Date &&
                    d.UserProfileId == profileId);


            var model = entityPlanDate==null? null:  new UserPlanSchedules()
            {
                UserDayPlanSchedules = data,
                Notes = entityPlanDate.PlanDateNotes.FirstOrDefault()?.Notes,
                PlanDateNoteId = entityPlanDate.PlanDateNotes.FirstOrDefault().PlanDateNoteId
            };

            return model;
        }

        public async Task<UserPlanSchedules> GetUserPlanSchedulesAsync(string userId, int planId)
        {
            var data = await (from timeSlots in context.TimeSlots
                              join planSchedule in context.PlanSchedule on timeSlots.TimeSlotId equals planSchedule.TimeSlotId
                              join planDates in context.PlanDates on planSchedule.PlanDateId equals planDates.PlanDateId
                              join planDateNotes in context.PlanDateNotes on planDates.PlanDateId equals planDateNotes.PlanDateId
                              join userProfile in context.UserProfile on planDates.UserProfileId equals userProfile.UserProfileId
                              where userProfile.AdObjectId.ToString().Equals(userId) && planDates.PlanDateId == planId
                              select new UserDayPlanSchedule()
                              {
                                  PlanName = planSchedule.PlanName,
                                  PlanDescription = planSchedule.PlanDescription,
                                  CategoryId = Convert.ToInt32(timeSlots.CategoryId),
                                  IsDone = Convert.ToBoolean(planSchedule.IsDone),
                                  TimeSlotId = Convert.ToInt16(timeSlots.TimeSlotId),
                                  SlotName = timeSlots.SlotName,
                                  TaskPriority = planSchedule.TaskPriority,
                                  PlanScheduleId = planSchedule.PlanScheduleId,
                                  PlanDateId = planDates.PlanDateId,
                                  PlanDate = Convert.ToDateTime(planDates.PlanDate),
                                  UserProfileId = Convert.ToInt32(planDates.UserProfileId),
                                  OrderId = timeSlots.OrderId
                              }
                         ).ToListAsync();

            var user = await context.UserProfile.SingleOrDefaultAsync(s => s.AdObjectId.ToString().Equals(userId));

            var profileId = user == null ? 0 : user?.UserProfileId;

            var entityPlanDate = await context.PlanDates.Include(i => i.PlanDateNotes)
                .FirstOrDefaultAsync(d => d.PlanDateId == planId &&
                    d.UserProfileId == profileId);


            var model = entityPlanDate == null ? null : new UserPlanSchedules()
            {
                UserDayPlanSchedules = data,
                Notes = entityPlanDate.PlanDateNotes.FirstOrDefault()?.Notes,
                PlanDateNoteId = entityPlanDate.PlanDateNotes.FirstOrDefault().PlanDateNoteId
            };

            return model;
        }
        public Task<List<UserPlanDate>> GetUserPlanSchedulesAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            var data = (from up in context.UserProfile
                        join pd in context.PlanDates on up.UserProfileId equals pd.UserProfileId
                        where (pd.PlanDate.Date >= fromDate.Date && pd.PlanDate.Date <= toDate.Date) 
                        && up.AdObjectId.ToString().Equals(userId)
                        select new UserPlanDate()
                        {
                            PlanDate = pd.PlanDate,
                            PlanDateId = pd.PlanDateId
                        }).ToListAsync();

            return data;
        }

        public async Task<bool> PlanDateExistsAsync(string userId, int planId)
        {
            var data = await (from up in context.UserProfile
                              join pd in context.PlanDates on up.UserProfileId equals pd.UserProfileId
                              where pd.PlanDateId == planId
                              && up.AdObjectId.ToString().Equals(userId)
                              select pd.PlanDateId).FirstOrDefaultAsync();

            return data > 0;
        }

        public async Task<int> SavePlanSchedule(PlanScheduleModel model, string userId)
        {
            var userProfile = await context.UserProfile.SingleOrDefaultAsync(s => s.AdObjectId.ToString().Equals(userId));
            var entityPlanDate = new PlanDates()
            {
                PlanDate = model.PlanDate,
                UserProfileId = Convert.ToInt32(userProfile?.UserProfileId),
                IsActive = true
            };

            var entitiesPlanSchedules = model.PlanSchdules.Select(s => new PlanSchedule()
            {
                PlanName = s.PlanName ?? string.Empty,
                PlanDescription = s.PlanDescription ?? string.Empty,
                IsActive = true,
                IsDone = s.IsDone,
                PlanDateId = entityPlanDate.PlanDateId,
                TaskPriority = s.TaskPriority,
                TimeSlotId = s.TimeSlotId
            }).ToList();

            var entityPlanNote = new PlanDateNote() { Notes = model.Notes, PlanDateId = entityPlanDate.PlanDateId };
            entityPlanDate.PlanDateNotes.Add(entityPlanNote);
            entitiesPlanSchedules.ForEach(f =>
            {
                entityPlanDate.PlanSchedules.Add(f);
            });

            context.PlanDates.Add(entityPlanDate);
            var rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0 ? entityPlanDate.PlanDateId : 0;
        }

        public async Task<bool> UpdatePlanSchedule(PlanScheduleModel model, string userId)
        {
            var userProfile = await context.UserProfile.SingleOrDefaultAsync(s => s.AdObjectId.ToString().Equals(userId));
            if (userProfile == null) throw new ApplicationException("User profile not found");

            var profileId = userProfile == null ? 0 : userProfile?.UserProfileId;

            var entityPlanDate = await context.PlanDates.Include(i => i.PlanDateNotes).Include(i => i.PlanSchedules)
                .FirstOrDefaultAsync(d => d.PlanDate.Date == model.PlanDate.Date &&
                    d.UserProfileId == profileId);

            if (entityPlanDate == null) throw new ApplicationException("No date found");

            entityPlanDate.PlanDateNotes.FirstOrDefault().Notes = model.Notes;

            entityPlanDate.PlanSchedules.ToList().ForEach(p =>
            {
                var updatedSlotDetail = model.PlanSchdules.Distinct().SingleOrDefault(s => s.PlanScheduleId == p.PlanScheduleId);
                if (updatedSlotDetail != null)
                {
                    p.PlanDescription = updatedSlotDetail.PlanDescription ?? string.Empty;
                    p.PlanName = updatedSlotDetail.PlanName ?? string.Empty;
                    p.IsDone = updatedSlotDetail.IsDone;
                    p.TaskPriority=updatedSlotDetail.TaskPriority;
                }
            });

            model.PlanSchdules.Where(w => w.PlanScheduleId == 0).ToList().ForEach(f =>
            {
                var toAdd = new PlanSchedule()
                {
                    IsActive = true,
                    IsDone = f.IsDone,
                    PlanDateId = f.PlanDateId,
                    PlanDescription = f.PlanDescription,
                    PlanName = f.PlanName,
                    TaskPriority = f.TaskPriority,
                    TimeSlotId = f.TimeSlotId
                };
                entityPlanDate.PlanSchedules.Add(toAdd); 
            });


            var rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<List<string>> ValidatePlanScheduleAsync(PlanScheduleModel model, string userId, bool isUpdateValidation = false)
        {
            var res = new List<string>();
            var timeSlots = await context.TimeSlots.ToListAsync();
            var creationTimeSlots = model.PlanSchdules.Select(s => s.TimeSlotId).ToList();
            var dbSlotIds = timeSlots.Select(s => s.TimeSlotId).ToList();
            var allowedPriority = new List<int>() { 1, 2, 3 };
            model.PlanSchdules.ForEach(p =>
            {
                if (!allowedPriority.Contains(p.TaskPriority))
                {
                    res.Add("Priority should be 1 or 2 or 3. 1 for High, 2 for Medium and 3 for low");
                }
            });

            creationTimeSlots.ForEach(f =>
            {
                if (!dbSlotIds.Contains(f))
                {
                    res.Add($"Timeslot id {f} is not allowed in request");
                }

            });

            var userProfile = await context.UserProfile.SingleOrDefaultAsync(s => s.AdObjectId.ToString().Equals(userId));
            var profileId = userProfile == null ? 0 : userProfile?.UserProfileId;

            var planDateExist = false;
            var planDates = await context.PlanDates.Where(d => 
                d.UserProfileId == profileId).ToListAsync();
            planDates.ForEach(f =>
            {
                if (f.PlanDate.Date == model.PlanDate.Date)
                {
                    planDateExist = true;
                }
            });

            if (!isUpdateValidation)
            {
                if (planDateExist)
                    res.Add($"Plan date {model.PlanDate} present in the database, Trying to update? Use update to make changes");
            }
            return res;
        }
    }
}
