using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Commands.Create;
using Planner.Core.ViewModels;
using Planner.CrossCutting;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Commands.Get
{
    public interface IDeleteUserPlanSceduleCommand : IAsyncCommand<int>
    {
    }

    public class DeleteUserPlanSceduleCommand : IDeleteUserPlanSceduleCommand
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPlanScheduleRepository planScheduleRepository;

        public DeleteUserPlanSceduleCommand(ILogger<DeleteUserPlanSceduleCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<DeleteUserPlanSceduleCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(int dateIdtoRemove, CancellationToken cancellationToken = default)
        {
            var userId = GetClaimInfo("objectidentifier");
            Logger.LogInformation($"Executing {nameof(DeleteUserPlanSceduleCommand)}");

            var exists = await planScheduleRepository.PlanDateExistsAsync(userId, dateIdtoRemove);
            if (!exists)
            {
                return new NotFoundResult();
            }       

            var updateResponse = await planScheduleRepository.DeletePlanDateRecordsAsync(dateIdtoRemove);

            return new AcceptedResult();
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
