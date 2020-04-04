using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class AddUserToRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
