using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    public sealed class NavigateForUser
    {
        public NavigateForUser(object message)
        {
            Message = message;
        }

        public object Message { get; }

    }
}
