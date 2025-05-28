using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem
{
    public sealed class NavigateToMessageForUser
    {
        public NavigateToMessageForUser(object message,TestingSystemContext context)
        {
            Message = message;
            Context = context;
        }

        public object Message { get; }

        public TestingSystemContext Context { get; }

    }
}
