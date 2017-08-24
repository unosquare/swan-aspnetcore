using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Swan.AspNetCore.Models;
using Unosquare.Swan.AspNetCore.Test.Mocks;

namespace Unosquare.Swan.AspNetCore.Test
{
    [TestClass]
    public class BasicUserStoreTest
    {
        private readonly CancellationToken ct = new CancellationToken();
        private static readonly ApplicationUserMock _mock = new ApplicationUserMock();
        private readonly List<ApplicationUser> _users = _mock.GetUsers();
        private readonly BasicUserStore _userStore = new BasicUserStore();

        [TestMethod]
        public async Task CreateAsyncTest()
        {
            var user = _mock.GetUser();
            var userId = user.UserId;

            var result = await _userStore.CreateAsync(user, ct);
            var data = await _userStore.FindByIdAsync(user.UserId, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user, data);
            Assert.AreNotEqual(userId, data.UserId);
        }

        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            user.UserName = "Changed";
            var result = await _userStore.UpdateAsync(user, ct);
            var userName = await _userStore.GetUserNameAsync(user, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var result = await _userStore.DeleteAsync(user, ct);
            var data = await _userStore.FindByIdAsync(user.UserId, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.IsNull(data);
        }

        [TestMethod]
        public async Task FindByIdAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);
            foreach (var u in _users)
                await _userStore.CreateAsync(u, ct);

            var userData = await _userStore.FindByIdAsync(user.UserId, ct);

            Assert.AreEqual(user, userData);
        }

        [TestMethod]
        public async Task FindByNameAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);
            foreach (var u in _users)
                await _userStore.CreateAsync(u, ct);

            var userData = await _userStore.FindByNameAsync(user.UserName, ct);

            Assert.AreEqual(user, userData);
        }

        [TestMethod]
        public async Task GetUserIdAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userId = await _userStore.GetUserIdAsync(user, ct);

            Assert.AreEqual(user.UserId, userId);
        }

        [TestMethod]
        public async Task GetUserNameAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userName = await _userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetNormalizedUserNameAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userName = await _userStore.GetNormalizedUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetPasswordHashAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var passwordHash = await _userStore.GetPasswordHashAsync(user, ct);

            Assert.AreEqual(user.PasswordHash, passwordHash);
        }

        [TestMethod]
        public async Task HasPasswordAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var hasPassword = await _userStore.HasPasswordAsync(user, ct);

            Assert.IsTrue(hasPassword);
        }

        [TestMethod]
        public async Task SetUserNameAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetUserNameAsync(user, "Changed", ct);
            var userName = await _userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task SetNormalizedUserNameAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetNormalizedUserNameAsync(user, "Changed", ct);
            var userName = await _userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task SetPasswordHashAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetNormalizedUserNameAsync(user, "Changed", ct);
            var userName = await _userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetPhoneNumberAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userPhone = await _userStore.GetPhoneNumberAsync(user, ct);

            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [TestMethod]
        public async Task SetPhoneNumberAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetPhoneNumberAsync(user, "9876543210", ct);
            var userPhone = await _userStore.GetPhoneNumberAsync(user, ct);

            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [TestMethod]
        public async Task GetPhoneNumberConfirmedAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userPhoneConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(user, ct);

            Assert.IsTrue(userPhoneConfirmed);
            Assert.AreEqual(user.PhoneNumberConfirmed, userPhoneConfirmed);
        }

        [TestMethod]
        public async Task SetPhoneNumberConfirmedAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetPhoneNumberConfirmedAsync(user, false, ct);
            var userPhoneConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(user, ct);

            Assert.IsFalse(userPhoneConfirmed);
            Assert.AreEqual(user.PhoneNumberConfirmed, userPhoneConfirmed);
        }

        [TestMethod]
        public async Task SetTwoFactorEnabledAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            await _userStore.SetTwoFactorEnabledAsync(user, true, ct);
            var twoFactor = await _userStore.GetTwoFactorEnabledAsync(user, ct);

            Assert.IsTrue(twoFactor);
            Assert.AreEqual(user.TwoFactorEnabled, twoFactor);
        }

        [TestMethod]
        public async Task GetTwoFactorEnabledAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var twoFactor = await _userStore.GetTwoFactorEnabledAsync(user, ct);

            Assert.IsFalse(twoFactor);
            Assert.AreEqual(user.TwoFactorEnabled, twoFactor);
        }

        [TestMethod]
        public async Task GetLoginsAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            var userLogin = await _userStore.GetLoginsAsync(user, ct);

            // Assertion to 0 because the method return an empty list
            Assert.AreEqual(0, userLogin.Count);
        }

        [TestMethod]
        public void FindByLoginAsyncTest()
        {

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return _userStore.FindByLoginAsync("loginProvider", "providerkey", ct);
            });
        }

        [TestMethod]
        public async Task AddLoginAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);
            var userLoginInfo = new UserLoginInfo("loginProv", "providerkey", "Name");

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return _userStore.AddLoginAsync(user, userLoginInfo, ct);
            });
        }

        [TestMethod]
        public async Task RemoveLoginAsyncTest()
        {
            var user = _mock.GetUser();
            await _userStore.CreateAsync(user, ct);

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return _userStore.RemoveLoginAsync(user, "loginProvider", "providerKey", ct);
            });
        }
    }
}
