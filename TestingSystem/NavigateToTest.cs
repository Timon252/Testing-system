using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem
{
    class NavigateToTest
    {
        public NavigateToTest(ICollection<Question> questions,Account account,Part part)
        {
            Questions = questions;
            Account = account;
            Part = part;
        }

        public ICollection<Question> Questions { get; }

        public Account Account { get; set; }

        public Part Part { get; set; }


    }
}
