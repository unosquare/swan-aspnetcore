namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System;

    class BusinessRulesControllerTest : IBusinessRulesController
    {
        public BusinessDbContextMock Context { get; protected set; }

        public BusinessRulesControllerTest()
        {
        }

        public BusinessRulesControllerTest(BusinessDbContextMock context)
        {
            Context = context;
        }

        public void RunBusinessRules()
        {
            var manager = new SampleManager();
            var user = new SampleUser();
            Context.Add(manager);
            Context.Add(user);
        }
    }
}
