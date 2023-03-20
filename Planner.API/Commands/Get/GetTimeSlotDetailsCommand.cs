using Microsoft.AspNetCore.Mvc;
using Planner.CrossCutting;
using Planner.Data;

namespace Planner.Commands.Get
{
    public interface IGetTimeSlotDetailsCommand : IAsyncCommand
    {
    }

    public class GetTimeSlotDetailsCommand : IGetTimeSlotDetailsCommand
    {
        private readonly IPlanScheduleRepository planScheduleRepository;

        public GetTimeSlotDetailsCommand(ILogger<GetTimeSlotDetailsCommand> logger, IPlanScheduleRepository planScheduleRepository)
        {
            Logger = logger;
            this.planScheduleRepository = planScheduleRepository;            
        }

        public ILogger<GetTimeSlotDetailsCommand> Logger { get; }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Executing {nameof(GetTimeSlotDetailsCommand)}");
            var model  = await planScheduleRepository.GetTimeSlotDetailsAsync();

            return new OkObjectResult(model);
        }
    }
}
