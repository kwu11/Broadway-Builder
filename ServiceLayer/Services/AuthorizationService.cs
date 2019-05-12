using DataAccessLayer;
//using ServiceLayer.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        
        private readonly BroadwayBuilderContext _dbContext;

        
        public AuthorizationService(BroadwayBuilderContext dbcontext)
        {
            //Initialize the DbContext
            this._dbContext = dbcontext;
        }
        

        /// <summary>
        /// Checks if a user is authorized to perform a action by checking if they have the permission needed
        /// </summary>
        /// <param name="user">User that must be checked if they are authorized</param>
        /// <param name="checkIfAuthorized">check if user has this permission</param>
        /// <returns>true if user has permission, false otherwise</returns>
        public bool HasPermission(User user, DataAccessLayer.Enums.PermissionsEnum permission)
        {
            return HasPermission(user.UserId, permission);
        }

        public bool HasPermission(int userId, DataAccessLayer.Enums.PermissionsEnum permission)
        {
            UserRole userRoles = _dbContext.UserRoles
                .Where(o => o.UserId == userId)
                .Include(o => o.Role.RolePermissions)
                .FirstOrDefault();
            if (userRoles != null)
            {
                if (userRoles.Role.RolePermissions
                    .Where(rolePermission => rolePermission.PermissionID == permission)
                    .Any())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
