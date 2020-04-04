using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Areas.Admin.Models.Users
{
    public class DeleteUserViewModel
    {
        [Required]
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}
