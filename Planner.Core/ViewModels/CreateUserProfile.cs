using System.ComponentModel.DataAnnotations;

namespace Planner.Core.ViewModels
{
    public class CreateUserProfile : UserProfileViewModel
    {
        public bool? IsSuccess { get; set; }
    }

    public class UserProfileViewModel
    {
        public int? Id { get; set; }
        [Required]
        [MinLength(4), MaxLength(200)]
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HasLoggedIn { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public string AdObjectId { get; set; } //user id   
    }
}
