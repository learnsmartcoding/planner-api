using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Commands.Create;
using Planner.Core.ViewModels;
using Planner.CrossCutting;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Commands.Get
{
    public interface IUpdateUserPlanScedulesCommand : IAsyncCommand<PlanScheduleModel>
    {
    }

    public class UpdateUserPlanScedulesCommand : IUpdateUserPlanScedulesCommand
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPlanScheduleRepository planScheduleRepository;

        public UpdateUserPlanScedulesCommand(ILogger<UpdateUserPlanScedulesCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<UpdateUserPlanScedulesCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(PlanScheduleModel model, CancellationToken cancellationToken = default)
        {
            var userId = GetClaimInfo("objectidentifier");
            Logger.LogInformation($"Executing {nameof(UpdateUserPlanScedulesCommand)}");

            var validationResponse = await planScheduleRepository.ValidatePlanScheduleAsync(model, userId, true);
            if (validationResponse.Any())
            {
                return new BadRequestObjectResult(validationResponse);
            }

            //All good to process

            var updateResponse = await planScheduleRepository.UpdatePlanSchedule(model, userId);


            return new CreatedResult("getUserPlanSchedules", model);
        }


        private string GetClaimInfo(string property)
        {
            var propertyData = "";
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                propertyData = identity.Claims.FirstOrDefault(d => d.Type.Contains(property)).Value;

            }
            return propertyData;
        }
    }
}
