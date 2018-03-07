using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EKE.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [NotMapped]
        public string RoleAssigned { get; set; }
    }

    public class ApplicationRole : IdentityRole<Guid>
    {

    }
}
