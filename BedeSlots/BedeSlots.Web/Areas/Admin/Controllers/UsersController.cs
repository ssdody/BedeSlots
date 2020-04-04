using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models.Users;
using BedeSlots.Web.Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole + "," + WebConstants.MasterAdminRole)]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;
        private readonly IPaginationProvider<UserDto> paginationProvider;

        public UsersController(IUserService userService, UserManager<User> userManager,
            ITransactionService transactionService, IPaginationProvider<UserDto> paginationProvider)
        {
            this.userService = userService;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.paginationProvider = paginationProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string userid)
        {
            if (userid == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            User user;
            try
            {
                user = await this.userManager.FindByIdAsync(userid);
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var userRoles = await this.userManager.GetRolesAsync(user);
            var currentUserId = userManager.GetUserId(HttpContext.User);

            if (user.Id == currentUserId)
            {
                return PartialView("_SelfEditPartial");
            }

            if (userRoles.Contains(WebConstants.MasterAdminRole))
            {
                return PartialView("_MasterAdminEditPartial");
            }

            var roleId = await userService.GetUserRoleIdAsync(userid);
            var roles = await userService.GetAllRolesAsync();
            roles = roles.Where(x => x.Name != WebConstants.MasterAdminRole).ToList();

            var rolesSelectList = roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();
            var model = new EditRoleViewModel()
            {
                UserId = userid,
                RoleId = roleId,
                UserName = user.FirstName + " " + user.LastName,
                Roles = rolesSelectList
            };

            return PartialView("_EditRolePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditRole");
            }

            var role = model.RoleId;
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userRoles = await this.userManager.GetRolesAsync(user);

            if (userRoles.Contains(WebConstants.MasterAdminRole))
            {
                return RedirectToAction("Index");
            }

            await this.userService.EditUserRoleAsync(model.UserId, model.RoleId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userid)
        {
            if (userid == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            User user;
            try
            {
                user = await this.userManager.FindByIdAsync(userid);
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var userRoles = await this.userManager.GetRolesAsync(user);
            if (userRoles.Contains(WebConstants.MasterAdminRole))
            {
                return PartialView("_MasterAdminEditPartial");
            }

            var model = new DeleteUserViewModel()
            {
                Id = userid,
                UserName = user.FirstName + " " + user.LastName
            };

            return PartialView("_DeleteUserPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Delete");
            }

            await this.userService.DeleteUserAsync(model.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadData()
        {
            try
            {
                string draw, sortColumn, sortColumnDirection, searchValue;
                int pageSize, skip, recordsTotal;

                this.paginationProvider.GetParameters(out draw, out sortColumn, out sortColumnDirection, out searchValue, out pageSize, out skip, out recordsTotal, HttpContext, Request);

                var users = this.userService.GetAllUsers();

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    users = users
                        .Where(u => u.Firstname.ToLower().Contains(searchValue)
                        || u.Lastname.ToLower().Contains(searchValue)
                        || u.Username.ToLower().Contains(searchValue)
                        || u.Email.ToLower().Contains(searchValue));
                }

                //Sorting
                users = this.paginationProvider.SortData(sortColumn, sortColumnDirection, users);

                //Total number of rows count 
                recordsTotal = users.Count();

                //Paging 
                var data = users
                    .Skip(skip)
                    .Take(pageSize).AsEnumerable()
                    .Select(u => new UserDtoListing
                    {
                        Userid = u.Id,
                        Username = u.Username,
                        Firstname = u.Firstname,
                        Lastname = u.Lastname,
                        Email = u.Email,
                        Balance = "$" + u.Balance,
                        Currency = u.Currency.ToString(),
                        Role = u.Role
                    });

                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
        }
    }
}