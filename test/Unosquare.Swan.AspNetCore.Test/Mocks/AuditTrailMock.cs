namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Unosquare.Swan.AspNetCore.Models;

    public class AuditTrailMock : IAuditTrailEntry
    {
        [Key]
        public int AuditId { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public int Action { get; set; }
        public string JsonBody { get; set; }
        public DateTime DateCreated { get; set; }
    }
}