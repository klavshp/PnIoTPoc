using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PnIotPoc.WebApi.Security
{
    /// <summary>
    /// Helper class for checking permissions in code.
    /// </summary>
    public static class PermsChecker
    {
        private static readonly RolePermissions RolePermissions;

        static PermsChecker()
        {
            RolePermissions = new RolePermissions();
        }

        /// <summary>
        /// Call this method in code to determine if a user has a given permission
        /// </summary>
        /// <param name="permission">Permission to check for</param>
        /// <returns>True if they have it</returns>
        public static bool HasPermission(Permission permission)
        {
            return RolePermissions.HasPermission(permission, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// Call this method in code to determine if a user has a given permissions
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool HasPermission(List<Permission> permissions)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            if (permissions == null || !permissions.Any())
            {
                return true;
            }

            // return true only if the user has ALL permissions
            return permissions
                    .Select(p => RolePermissions.HasPermission(p, httpContext))
                    .All(val => val == true);
        }
    }
}