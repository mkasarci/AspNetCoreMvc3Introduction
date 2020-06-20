using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreMvc3.Introduction.Identity
{
    public class AppIdentityUser :IdentityUser
    {
        public int Age { get; set; }
    }
}
