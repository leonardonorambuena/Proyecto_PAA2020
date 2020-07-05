using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Proyecto_PAA.Helpers
{
    public static class UserClaimHelper
    {
        public static string GetFullName(this IIdentity identity)
        {
            if (identity == null)
                return string.Empty;
            var claim = ((ClaimsIdentity)identity);
            return claim.FindFirst("FullName")?.Value;
        }
    }
}