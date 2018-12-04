namespace Unosquare.Swan.AspNetCore
{
    using Microsoft.AspNetCore.Identity;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a basic UserStore for AspNetCore Identity.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IUserStore{ApplicationUser}" />
    /// <seealso cref="Microsoft.AspNetCore.Identity.IUserPasswordStore{ApplicationUser}" />
    /// <seealso cref="Microsoft.AspNetCore.Identity.IUserLoginStore{ApplicationUser}" />
    /// <seealso cref="Microsoft.AspNetCore.Identity.IUserPhoneNumberStore{ApplicationUser}" />
    /// <seealso cref="Microsoft.AspNetCore.Identity.IUserTwoFactorStore{ApplicationUser}" />
    public class BasicUserStore : IUserPasswordStore<ApplicationUser>,
                                   IUserLoginStore<ApplicationUser>,
                                   IUserPhoneNumberStore<ApplicationUser>,
                                   IUserTwoFactorStore<ApplicationUser>
    {
        private readonly List<ApplicationUser> _users = new List<ApplicationUser>();

        /// <inheritdoc />
        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.UserId = Guid.NewGuid().ToString();

            _users.Add(user);

            return Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var match = _users.FirstOrDefault(u => u.UserId == user.UserId);

            if (match != null)
            {
                match.UserName = user.UserName;
                match.Email = user.Email;
                match.PhoneNumber = user.PhoneNumber;
                match.TwoFactorEnabled = user.TwoFactorEnabled;
                match.PasswordHash = user.PasswordHash;

                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(IdentityResult.Failed());
        }

        /// <inheritdoc />
        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var match = _users.FirstOrDefault(u => u.UserId == user.UserId);
            if (match != null)
            {
                _users.Remove(match);

                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(IdentityResult.Failed());
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);

            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.FirstOrDefault(u => u.UserName == normalizedUserName);

            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserId);

        /// <inheritdoc />
        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);

        /// <inheritdoc />
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);

        /// <inheritdoc />
        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);

        /// <inheritdoc />
        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash != null);

        /// <inheritdoc />
        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.PhoneNumber);

        /// <inheritdoc />
        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.PhoneNumberConfirmed);

        /// <inheritdoc />
        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.TwoFactorEnabled);

        /// <inheritdoc />
        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // Just returning an empty list because I don't feel like implementing this. You should get the idea though...
            IList<UserLoginInfo> logins = new List<UserLoginInfo>();
            return Task.FromResult(logins);
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
