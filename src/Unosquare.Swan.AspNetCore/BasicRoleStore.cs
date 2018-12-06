namespace Unosquare.Swan.AspNetCore
{
    using Microsoft.AspNetCore.Identity;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a basic RoleStore for AspNetCore Identity.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IRoleStore{ApplicationRole}" />
    public class BasicRoleStore : IRoleStore<ApplicationRole>
    {
        private readonly List<ApplicationRole> _roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRoleStore"/> class.
        /// </summary>
        public BasicRoleStore()
        {
            _roles = new List<ApplicationRole>();
        }

        /// <inheritdoc />
        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            _roles.Add(role);

            return Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            var match = _roles.FirstOrDefault(r => r.RoleId == role.RoleId);

            if (match == null) return Task.FromResult(IdentityResult.Failed());

            match.RoleName = role.RoleName;

            return Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            var match = _roles.FirstOrDefault(r => r.RoleId == role.RoleId);
            if (match == null) return Task.FromResult(IdentityResult.Failed());
            _roles.Remove(match);

            return Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = _roles.FirstOrDefault(r => r.RoleId == roleId);

            return Task.FromResult(role);
        }

        /// <inheritdoc />
        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = _roles.FirstOrDefault(r => r.RoleName == normalizedRoleName);

            return Task.FromResult(role);
        }

        /// <inheritdoc />
        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken) => Task.FromResult(role.RoleId);

        /// <inheritdoc />
        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken) => Task.FromResult(role.RoleName);

        /// <inheritdoc />
        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken) => Task.FromResult(role.RoleName);

        /// <inheritdoc />
        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.RoleName = roleName;

            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.RoleName = normalizedName;

            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
