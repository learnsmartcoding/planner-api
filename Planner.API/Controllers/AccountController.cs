using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Core.Model;
using Planner.Core.ViewModels;
using Planner.Data;
using System.Security.Claims;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly PlannerContext context;

        public AccountController(PlannerContext context)
        {
            this.context = context;
        }


        private CreateUserProfile GetCreateUserProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
            }

            CreateUserProfile model = new CreateUserProfile();
            model.DisplayName = identity.Claims.FirstOrDefault(d => d.Type.Contains("givenname")).Value;
            model.Email = identity.Claims.FirstOrDefault(d => d.Type.Contains("emails")).Value;
            model.AdObjectId = identity.Claims.FirstOrDefault(d => d.Type.Contains("objectidentifier")).Value;
            return model;

        }

        [HttpPost("SaveProfile")]
        public async Task<IActionResult> SaveProfile()
        {
            CreateUserProfile model = GetCreateUserProfile();
            var userProfileEntity = await context.UserProfile.FirstOrDefaultAsync(f => f.AdObjectId == Guid.Parse(model.AdObjectId));


            if (userProfileEntity == null)
            {

                //new user flow
                var entityToAdd = new UserProfile()
                {
                    AdObjectId = Guid.Parse(model.AdObjectId),
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false
                };

                await context.UserProfile.AddAsync(entityToAdd);
                var savedCounts = await context.SaveChangesAsync();

            }
            else
            {
                userProfileEntity.DisplayName = model.DisplayName;
                userProfileEntity.FirstName = model.FirstName;
                userProfileEntity.LastName = model.LastName;
                userProfileEntity.PhoneNumber = model.PhoneNumber;

                context.UserProfile.Update(userProfileEntity);
                var savedCounts = await context.SaveChangesAsync();
            }


            model.IsSuccess = true;
            return Ok(model);
        }

        [HttpGet("GetUserProfile")]
        public IActionResult GetUserProfile()
        {
            var model = GetCreateUserProfile();
            return Ok(model);
        }

    }
}
