namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System.ComponentModel.DataAnnotations;

    class SampleUser
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public SampleUser GetUser()
        {
            return new SampleUser
            {
                UserId = 1,
                UserName = "User Test",
                Email = "user@domain.com",
            }; ;
        }
    }

    class SampleManager
    {
        [Key]
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }

        public SampleManager GetManager()
        {
            return new SampleManager
            {
                ManagerId = 1,
                ManagerName = "Manager Test",
                Email = "manager@domain.com",
            }; ;
        }
    }
}
