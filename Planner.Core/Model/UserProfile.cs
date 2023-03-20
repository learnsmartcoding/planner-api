using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planner.Core.Model
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            PlanDates = new HashSet<PlanDates>();
        }

        [Key]
        public int UserProfileId { get; set; }
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool HasLoggedIn { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public Guid AdObjectId { get; set; }

        public virtual ICollection<PlanDates> PlanDates { get; set; }
    }
}
