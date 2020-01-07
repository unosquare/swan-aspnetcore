using System;
using System.ComponentModel.DataAnnotations;
using Swan.AspNetCore.Models;

namespace Swan.AspNetCore.Test.Mocks
{
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