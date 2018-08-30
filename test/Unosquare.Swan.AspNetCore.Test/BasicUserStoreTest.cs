namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Identity;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Mocks;

    [TestFixture]
    public class BasicUserStoreTest
    {
        private static readonly ApplicationUserMock Mock = new ApplicationUserMock();

        private readonly CancellationToken _ct = new CancellationToken();
        private readonly List<ApplicationUser> _users = Mock.GetUsers();
        private readonly BasicUserStore _userStore = new BasicUserStore();

        [Test]
        public async Task CreateAsyncTest()
        {
            var user = Mock.GetUser();
            var userId = user.UserId;

            var result = await _userStore.CreateAsync(user, _ct);
            var data = await _userStore.FindByIdAsync(user.UserId, _ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user, data);
            Assert.AreNotEqual(userId, data.UserId);
        }

        [Test]
        public async Task UpdateAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            user.UserName = "Changed";
            var result = await _userStore.UpdateAsync(user, _ct);
            var userName = await _userStore.GetUserNameAsync(user, _ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task DeleteAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var result = await _userStore.DeleteAsync(user, _ct);
            var data = await _userStore.FindByIdAsync(user.UserId, _ct);

            Assert.IsTrue(result.Succeeded);
            Assert.IsNull(data);
        }

        [Test]
        public async Task FindByIdAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);
            foreach (var u in _users)
                await _userStore.CreateAsync(u, _ct);

            var userData = await _userStore.FindByIdAsync(user.UserId, _ct);

            Assert.AreEqual(user, userData);
        }

        [Test]
        public async Task FindByNameAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            foreach (var u in _users)
                await _userStore.CreateAsync(u, _ct);

            var userData = await _userStore.FindByNameAsync(user.UserName, _ct);

            Assert.AreEqual(user.Name, userData.Name);
        }

        [Test]
        public async Task GetUserIdAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userId = await _userStore.GetUserIdAsync(user, _ct);

            Assert.AreEqual(user.UserId, userId);
        }

        [Test]
        public async Task GetUserNameAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userName = await _userStore.GetUserNameAsync(user, _ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task GetNormalizedUserNameAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userName = await _userStore.GetNormalizedUserNameAsync(user, _ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task GetPasswordHashAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var passwordHash = await _userStore.GetPasswordHashAsync(user, _ct);

            Assert.AreEqual(user.PasswordHash, passwordHash);
        }

        [Test]
        public async Task HasPasswordAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var hasPassword = await _userStore.HasPasswordAsync(user, _ct);

            Assert.IsTrue(hasPassword);
        }

        [Test]
        public async Task SetUserNameAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetUserNameAsync(user, "Changed", _ct);
            var userName = await _userStore.GetUserNameAsync(user, _ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task SetNormalizedUserNameAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetNormalizedUserNameAsync(user, "Changed", _ct);
            var userName = await _userStore.GetUserNameAsync(user, _ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task SetPasswordHashAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetNormalizedUserNameAsync(user, "Changed", _ct);
            var userName = await _userStore.GetUserNameAsync(user, _ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [Test]
        public async Task GetPhoneNumberAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userPhone = await _userStore.GetPhoneNumberAsync(user, _ct);

            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [Test]
        public async Task SetPhoneNumberAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetPhoneNumberAsync(user, "9876543210", _ct);
            var userPhone = await _userStore.GetPhoneNumberAsync(user, _ct);

            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [Test]
        public async Task GetPhoneNumberConfirmedAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userPhoneConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(user, _ct);

            Assert.IsTrue(userPhoneConfirmed);
        }

        [Test]
        public async Task SetPhoneNumberConfirmedAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetPhoneNumberConfirmedAsync(user, false, _ct);
            var userPhoneConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(user, _ct);

            Assert.IsFalse(userPhoneConfirmed);
        }

        [Test]
        public async Task SetTwoFactorEnabledAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            await _userStore.SetTwoFactorEnabledAsync(user, true, _ct);
            var twoFactor = await _userStore.GetTwoFactorEnabledAsync(user, _ct);

            Assert.IsTrue(twoFactor);
        }

        [Test]
        public async Task GetTwoFactorEnabledAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var twoFactor = await _userStore.GetTwoFactorEnabledAsync(user, _ct);

            Assert.IsFalse(twoFactor);
        }

        [Test]
        public async Task GetLoginsAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            var userLogin = await _userStore.GetLoginsAsync(user, _ct);

            // Assertion to 0 because the method return an empty list
            Assert.AreEqual(0, userLogin.Count);
        }

        [Test]
        public void FindByLoginAsyncTest()
        {
            Assert.Throws<NotImplementedException>(() =>
                _userStore.FindByLoginAsync("loginProvider", "providerkey", _ct));
        }

        [Test]
        public async Task AddLoginAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);
            var userLoginInfo = new UserLoginInfo("loginProv", "providerkey", "Name");

            Assert.Throws<NotImplementedException>(() => _userStore.AddLoginAsync(user, userLoginInfo, _ct));
        }

        [Test]
        public async Task RemoveLoginAsyncTest()
        {
            var user = Mock.GetUser();
            await _userStore.CreateAsync(user, _ct);

            Assert.Throws<NotImplementedException>(() =>
                _userStore.RemoveLoginAsync(user, "loginProvider", "providerKey", _ct));
        }
    }
}