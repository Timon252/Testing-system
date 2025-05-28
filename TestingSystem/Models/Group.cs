using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
   public class Group
    {
        public int GroupId { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }



        public Group()
        {
            Accounts = new List<Account>();
        }
    }
}
