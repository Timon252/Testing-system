using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem
{
  public sealed class  NavigateToMessageForAdmin
    {
        public NavigateToMessageForAdmin(object message, TestingSystemContext context)
        {
            Message = message;
            Context = context;
        }

        public object Message { get; }

        public TestingSystemContext Context { get; }
    }
}
