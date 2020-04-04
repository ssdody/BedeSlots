using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Areas.Admin.Models.Users
{
    public class EditRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [Required]
        public string RoleId { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
    }
}
