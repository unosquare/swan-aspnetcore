using System;
using Microsoft.EntityFrameworkCore;
using Swan.AspNetCore.Models;
using Swan.AspNetCore.Test.Mocks;
using Swan.Formatters;

namespace Swan.AspNetCore.Test
{
    internal class CustomAuditTrailController : AuditTrailController<BusinessDbContextMock, AuditTrailMock>
    {
        public CustomAuditTrailController(BusinessDbContextMock context, string currentUserId) : base(context, currentUserId)
        {
        }

        protected override void AuditEntry(ActionFlags flag, object entity, string name)
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId)) return;

            var instance = (IAuditTrailEntry)Activator.CreateInstance<AuditTrailMock>();
            instance.TableName = name;
            instance.DateCreated = DateTime.UtcNow;
            instance.Action = (int)flag;
            instance.UserId = CurrentUserId;
            instance.JsonBody = Json.SerializeExcluding(entity, false, "Image");

            Context.Entry(instance).State = EntityState.Added;
        }
    }
}
