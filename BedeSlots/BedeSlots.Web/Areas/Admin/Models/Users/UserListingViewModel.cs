using BedeSlots.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class UserListingViewModel
    {
        public ICollection<UserViewModel> Users { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
    }
}
