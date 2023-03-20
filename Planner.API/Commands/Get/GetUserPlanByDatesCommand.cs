using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.CrossCutting;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Commands.Get
{
    public interface IGetUserPlanByDatesCommand : IAsyncCommand<DateTime, DateTime>
    {
    }

    public class GetUserPlanByDatesCommand : IGetUserPlanByDatesCommand
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPlanScheduleRepository planScheduleRepository;

        public GetUserPlanByDatesCommand(ILogger<GetUserPlanByDatesCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<GetUserPlanByDatesCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(DateTime fromDate,DateTime toDate, CancellationToken cancellationToken = default)
        {
            var userId = GetClaimInfo("objectidentifier");
            Logger.LogInformation($"Executing {nameof(GetUserPlanByDatesCommand)}");
            var model = await planScheduleRepository.GetUserPlanSchedulesAsync(userId, fromDate, toDate);

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
