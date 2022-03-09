using DataLayer.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataLayer.Entities.Account
{
    public class Role : IdentityRole, IAccountEntity
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
