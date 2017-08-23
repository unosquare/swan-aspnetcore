using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unosquare.Swan.AspNetCore.Models;

namespace Unosquare.Swan.AspNetCore.Test
{
    [TestClass]
    public class BasicUserStoreTest
    {
        private readonly CancellationToken cancellatinToken = new CancellationToken();
        private readonly ApplicationUser _user = new ApplicationUser
        {
            UserId = "1",
            UserName = "Israel",
            Email = "myemail@domain.com",
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true,
            PasswordHash = "PasswordHash",
            TwoFactorEnabled = false
        };

        [TestMethod]
        public void CreateAsyncTest()
        {
            var userStore = new BasicUserStore();

            var result = userStore.CreateAsync(_user, cancellatinToken);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCompleted);
        }

    }
}
