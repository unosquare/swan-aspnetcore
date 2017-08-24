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
        private readonly ApplicationUserMock _mock = new ApplicationUserMock();
        private readonly List<ApplicationUser> _users = new ApplicationUserMock().GetUsers();

        [TestMethod]
        public async Task CreateAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            var userId = user.UserId;

            var result = await userStore.CreateAsync(user, ct);
            var data = await userStore.FindByIdAsync(user.UserId, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user, data);
            Assert.AreNotEqual(userId, data.UserId);
        }

        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            user.UserName = "Changed";
            var result = await userStore.UpdateAsync(user, ct);
            var userName = await userStore.GetUserNameAsync(user, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var result = await userStore.DeleteAsync(user, ct);
            var data = await userStore.FindByIdAsync(user.UserId, ct);

            Assert.IsTrue(result.Succeeded);
            Assert.IsNull(data);
        }

        [TestMethod]
        public async Task FindByIdAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);
            foreach (var u in _users)
                await userStore.CreateAsync(u, ct);

            var userData = await userStore.FindByIdAsync(user.UserId, ct);

            Assert.AreEqual(user, userData);
        }

        [TestMethod]
        public async Task FindByNameAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);
            foreach (var u in _users)
                await userStore.CreateAsync(u, ct);

            var userData = await userStore.FindByNameAsync(user.UserName, ct);

            Assert.AreEqual(user, userData);
        }

        [TestMethod]
        public async Task GetUserIdAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userId = await userStore.GetUserIdAsync(user, ct);

            Assert.AreEqual(user.UserId, userId);
        }

        [TestMethod]
        public async Task GetUserNameAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userName = await userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetNormalizedUserNameAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userName = await userStore.GetNormalizedUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetPasswordHashAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var passwordHash = await userStore.GetPasswordHashAsync(user, ct);

            Assert.AreEqual(user.PasswordHash, passwordHash);
        }

        [TestMethod]
        public async Task HasPasswordAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var hasPassword = await userStore.HasPasswordAsync(user, ct);

            Assert.IsTrue(hasPassword);
        }

        [TestMethod]
        public async Task SetUserNameAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetUserNameAsync(user, "Changed", ct);
            var userName = await userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task SetNormalizedUserNameAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetNormalizedUserNameAsync(user, "Changed", ct);
            var userName = await userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task SetPasswordHashAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetNormalizedUserNameAsync(user, "Changed", ct);
            var userName = await userStore.GetUserNameAsync(user, ct);

            Assert.AreEqual(user.UserName, userName);
        }

        [TestMethod]
        public async Task GetPhoneNumberAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userPhone = await userStore.GetPhoneNumberAsync(user, ct);
            
            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [TestMethod]
        public async Task SetPhoneNumberAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetPhoneNumberAsync(user, "9876543210", ct);
            var userPhone = await userStore.GetPhoneNumberAsync(user, ct);

            Assert.AreEqual(user.PhoneNumber, userPhone);
        }

        [TestMethod]
        public async Task GetPhoneNumberConfirmedAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userPhoneConfirmed = await userStore.GetPhoneNumberConfirmedAsync(user, ct);

            Assert.IsTrue(userPhoneConfirmed);
            Assert.AreEqual(user.PhoneNumberConfirmed, userPhoneConfirmed);
        }

        [TestMethod]
        public async Task SetPhoneNumberConfirmedAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetPhoneNumberConfirmedAsync(user, false, ct);
            var userPhoneConfirmed = await userStore.GetPhoneNumberConfirmedAsync(user, ct);

            Assert.IsFalse(userPhoneConfirmed);
            Assert.AreEqual(user.PhoneNumberConfirmed, userPhoneConfirmed);
        }

        [TestMethod]
        public async Task SetTwoFactorEnabledAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            await userStore.SetTwoFactorEnabledAsync(user, true, ct);
            var twoFactor = await userStore.GetTwoFactorEnabledAsync(user, ct);

            Assert.IsTrue(twoFactor);
            Assert.AreEqual(user.TwoFactorEnabled, twoFactor);
        }

        [TestMethod]
        public async Task GetTwoFactorEnabledAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var twoFactor = await userStore.GetTwoFactorEnabledAsync(user, ct);

            Assert.IsFalse(twoFactor);
            Assert.AreEqual(user.TwoFactorEnabled, twoFactor);
        }

        [TestMethod]
        public async Task GetLoginsAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            var userLogin = await userStore.GetLoginsAsync(user, ct);

            // Assertion to 0 because the method return an empty list
            Assert.AreEqual(0, userLogin.Count);
        }

        [TestMethod]
        public void FindByLoginAsyncTest()
        {
            var userStore = new BasicUserStore();

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return userStore.FindByLoginAsync("loginProvider", "providerkey", ct);
            });
        }

        [TestMethod]
        public async Task AddLoginAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);
            var userLoginInfo = new UserLoginInfo("loginProv", "providerkey", "Name");

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return userStore.AddLoginAsync(user, userLoginInfo, ct);
            });
        }

        [TestMethod]
        public async Task RemoveLoginAsyncTest()
        {
            var userStore = new BasicUserStore();
            var user = _mock.GetUser();
            await userStore.CreateAsync(user, ct);

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                return userStore.RemoveLoginAsync(user,"loginProvider","providerKey",ct);
            });
        }
    }
}
