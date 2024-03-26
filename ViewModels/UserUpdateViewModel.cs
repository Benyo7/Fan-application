using System.ComponentModel.DataAnnotations;

namespace Fan_platform.ViewModels
{
    public class UserUpdateViewModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
    }
}
