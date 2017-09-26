using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Unosquare.Swan.AspNetCore.Models;

namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    class ApplicationUserMock
    {
        private List<string> Names { get; set; } = new List<string> { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian", "abby", "abigail", "adele", "adrian" };
        private readonly Random rnd = new Random();
        private List<ApplicationUser> _users = new List<ApplicationUser>();

        public List<ApplicationUser> GetUsers()
        {
            for (int i = 0; i < 50; i++)
            {
                var r = rnd.Next(Names.Count);

                _users.Add(new ApplicationUser
                {
                    UserId = i.ToString(),
                    UserName = Names[r],
                    Email = Names[r]+"@domain.com",
                    PhoneNumber = "12345678"+r+i,
                    PhoneNumberConfirmed = true,
                    PasswordHash = (r*i).ToString(),
                    TwoFactorEnabled = false
                });
            }

            return _users;
        }

        public ApplicationUser GetUser()
        {           
            return new ApplicationUser
            {
                UserId = "0",
                UserName = "Test",
                Email = "myemail@domain.com",
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "PasswordHash",
                TwoFactorEnabled = false
            };
        }
    }
}
