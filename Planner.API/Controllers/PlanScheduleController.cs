using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Commands.Create;
using Planner.Commands.Get;
using Planner.Common;
using Planner.Core.ViewModels;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlanScheduleController : ControllerBase
    {
        [HttpGet("getTimeSlots", Name = ControllerRoute.GetTimeSlots)]
        [ProducesResponseType(typeof(List<TimeSlotDetails>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetCompanyAddress(
           [FromServices] IGetTimeSlotDetailsCommand command,
           CancellationToken cancellationToken)
      => command.ExecuteAsync(cancellationToken);


        [HttpGet("getUserPlanByDates", Name = ControllerRoute.GetUserPlanByDates)]
        [ProducesResponseType(typeof(List<UserPlanDate>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetUserPlanByDates(
          [FromServices] IGetUserPlanByDatesCommand command,
          [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate,
          CancellationToken cancellationToken)
     => command.ExecuteAsync(fromDate, toDate, cancellationToken);

        [HttpGet("getUserPlanSchedules", Name = ControllerRoute.GetUserPlanSchedules)]
        [ProducesResponseType(typeof(UserPlanSchedules), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetUserPlanSchedules(
          [FromServices] IGetUserPlanScedulesCommand command,
          [FromQuery] DateTime day,
          CancellationToken cancellationToken)
     => command.ExecuteAsync(day, cancellationToken);


        [HttpGet("{id}", Name = ControllerRoute.GetUserPlanSchedulesByPlanId)]
        [ProducesResponseType(typeof(UserPlanSchedules), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetUserPlanSchedulesByPlanId(
          [FromServices] IGetUserPlanScedulesByPlanIdCommand command,
          [FromRoute] int id,
          CancellationToken cancellationToken)
     => command.ExecuteAsync(id, cancellationToken);


        [HttpPost("", Name = ControllerRoute.CreateUserPlanSchedules)]
        [ProducesResponseType(typeof(List<TimeSlotDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public Task<IActionResult> CreateUserPlanSchedules(
          [FromServices] ICreateUserPlanScedulesCommand command,
          [FromBody] PlanScheduleModel model,
          CancellationToken cancellationToken)
     => command.ExecuteAsync(model, cancellationToken);

        [HttpPut("", Name = ControllerRoute.UpdateUserPlanSchedules)]
        [ProducesResponseType(typeof(List<TimeSlotDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public Task<IActionResult> UpdateUserPlanSchedules(
         [FromServices] IUpdateUserPlanScedulesCommand command,
         [FromBody] PlanScheduleModel model,
         CancellationToken cancellationToken)
    => command.ExecuteAsync(model, cancellationToken);

        [HttpDelete("{id:int}", Name = ControllerRoute.DeleteUserPlanSchedules)]
        [ProducesResponseType(typeof(List<TimeSlotDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<TimeSlotDetails>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public Task<IActionResult> DeleteUserPlanSchedules(
         [FromServices] IDeleteUserPlanSceduleCommand command,
         [FromRoute] int id,
         CancellationToken cancellationToken)
    => command.ExecuteAsync(id, cancellationToken);


    }
}
