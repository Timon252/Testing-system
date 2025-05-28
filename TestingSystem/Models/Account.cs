using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
   public class Account
    {
        public int AccountId { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public virtual ICollection<TestPart> TestParts { get; }

    }
}
