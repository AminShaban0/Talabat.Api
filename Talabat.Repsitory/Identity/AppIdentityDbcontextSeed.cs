using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repsitory.Identity
{
    public static class AppIdentityDbcontextSeed
    {
        public static async Task StoreIdentityAppUser(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Amin Mohamed",
                    Email = "xamin166@gmail.com",
                    UserName = "Amin_Mohamed",
                    PhoneNumber = "01128452206"
                };
                await _userManager.CreateAsync(user , "P@ssw0rd");
            }
        }
    }
}
