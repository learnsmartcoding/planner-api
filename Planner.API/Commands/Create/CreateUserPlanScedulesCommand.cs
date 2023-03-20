using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Planner.Core.ViewModels;
using Planner.CrossCutting;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Commands.Create
{
    public interface ICreateUserPlanScedulesCommand : IAsyncCommand<PlanScheduleModel>
    {
    }

    public class CreateUserPlanScedulesCommand : ICreateUserPlanScedulesCommand
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPlanScheduleRepository planScheduleRepository;

        public CreateUserPlanScedulesCommand(ILogger<CreateUserPlanScedulesCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<CreateUserPlanScedulesCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(PlanScheduleModel model, CancellationToken cancellationToken = default)
        {
            var userId = GetClaimInfo("objectidentifier");
            Logger.LogInformation($"Executing {nameof(CreateUserPlanScedulesCommand)}");

            var validationResponse = await planScheduleRepository.ValidatePlanScheduleAsync(model, userId);
            if (validationResponse.Any())
            {                
                return new BadRequestObjectResult(validationResponse);
            }

            //All good to process

            var saveResponse = await planScheduleRepository.SavePlanSchedule(model, userId);
            model.PlanDateId = saveResponse;


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
