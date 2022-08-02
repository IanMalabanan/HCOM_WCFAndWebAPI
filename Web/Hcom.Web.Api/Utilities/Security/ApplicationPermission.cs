using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities.Security
{

    public class ApplicationPermission
    {
        public ApplicationPermission()
        { }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }


        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }



    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        //Default standard permissions 

        public const string UsersPermissionGroupName = "User Permissions";
        public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view", UsersPermissionGroupName, "Permission to view other users account details");
        public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");

        public const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");


        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
                ViewUsers,
                ManageUsers,
                ViewRoles,
                ManageRoles,
                AssignRoles              
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).SingleOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).SingleOrDefault();
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string GetAllPermissionValuesToCsvData()
        {
            var tmp = AllPermissions.Select(p => p.Value).ToArray();
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < tmp.Length; index++)
            {

                sb.Append(tmp[index].ToString());

                if (index < tmp.Length - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        public static List<string> ConvertPermissionToString(string values)
        {
            var _tmp = values.Split(',');
            var _result = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < _tmp.Length; index++)
            {

                _result.Add(_tmp[index]);

            }

            return _result;
        }

        public static string[] GetAdministrativePermissionValues()
        {
            return new string[] { ManageUsers, ManageRoles, AssignRoles };
        }
    }




    /// <summary>
    /// Operation Policy to allow adding, viewing, updating and deleting general or specific user records.
    /// </summary>
    public static class AccountManagementOperations
    {
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";

        public static UserAccountAuthorizationRequirement Create = new UserAccountAuthorizationRequirement(CreateOperationName);
        public static UserAccountAuthorizationRequirement Read = new UserAccountAuthorizationRequirement(ReadOperationName);
        public static UserAccountAuthorizationRequirement Update = new UserAccountAuthorizationRequirement(UpdateOperationName);
        public static UserAccountAuthorizationRequirement Delete = new UserAccountAuthorizationRequirement(DeleteOperationName);
        public static UserAccountAuthorizationRequirement Operator = new UserAccountAuthorizationRequirement("Operator");
        public static UserAccountAuthorizationRequirement Admin = new UserAccountAuthorizationRequirement("Admin");
    }

    #region "Authorizations"

    public class UserAccountAuthorizationRequirement : IAuthorizationRequirement
    {
        public UserAccountAuthorizationRequirement(string operationName)
        {
            this.OperationName = operationName;
        }


        public string OperationName { get; private set; }
    }



    public class ViewUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthorizationRequirement requirement, string targetUserId)
        {
            if (context.User == null || requirement.OperationName != AccountManagementOperations.ReadOperationName)
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }


      
    }



    public class ManageUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthorizationRequirement requirement, string targetUserId)
        {
            if (context.User == null ||
                (requirement.OperationName != AccountManagementOperations.CreateOperationName &&
                 requirement.OperationName != AccountManagementOperations.UpdateOperationName &&
                 requirement.OperationName != AccountManagementOperations.DeleteOperationName))
                return Task.CompletedTask;

         if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers) )
                context.Succeed(requirement);

            return Task.CompletedTask;
        }


    }
    #endregion



    #region "Policies"

    public class Policies
    {
        ///<summary>Policy to allow viewing all user records.</summary>
        public const string ViewAllUsersPolicy = "View All Users";

        ///<summary>Policy to allow adding, removing and updating all user records.</summary>
        public const string ManageAllUsersPolicy = "Manage All Users";

        /// <summary>Policy to allow viewing details of all roles.</summary>
        public const string ViewAllRolesPolicy = "View All Roles";

        /// <summary>Policy to allow viewing details of all or specific roles (Requires roleName as parameter).</summary>
        public const string ViewRoleByRoleNamePolicy = "View Role by RoleName";

        /// <summary>Policy to allow adding, removing and updating all roles.</summary>
        public const string ManageAllRolesPolicy = "Manage All Roles";

        /// <summary>Policy to allow assigning roles the user has access to (Requires new and current roles as parameter).</summary>
        public const string AssignAllowedRolesPolicy = "Assign Allowed Roles";


        /// <summary>Policy for operator.</summary>
        public const string OperatorPolicy = "Operator Access Rights";

        /// <summary>Policy for operator.</summary>
        public const string AdministratorPolicy = "Administrator Access Rights";
    }


   




    #endregion
}
