using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.CrossCutting;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Commands.Get
{
    public interface IGetUserPlanScedulesCommand : IAsyncCommand<DateTime>
    {
    }

    public class GetUserPlanScedulesCommand : IGetUserPlanScedulesCommand
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPlanScheduleRepository planScheduleRepository;

        public GetUserPlanScedulesCommand(ILogger<GetUserPlanScedulesCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<GetUserPlanScedulesCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(DateTime day, CancellationToken cancellationToken = default)
        {
            var userId = GetClaimInfo("objectidentifier");
            Logger.LogInformation($"Executing {nameof(GetUserPlanScedulesCommand)}");
            var model = await planScheduleRepository.GetUserPlanSchedulesAsync(userId, day);

            if (model == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(model);
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
