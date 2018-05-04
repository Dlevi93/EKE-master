using EKE.Data.Entities.Identity.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EKE_Admin.Web.ViewModels
{
    public class UserManagementVM
    {
        public List<IdentityUser> AppUser { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
