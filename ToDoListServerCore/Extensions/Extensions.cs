using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToDoListServerCore.Extensions
{
    public static class Extensions
    {
        public static bool IsUnitTest {set;get;}

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (IsUnitTest)
                return 1;

            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
