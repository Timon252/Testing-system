using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem
{
    public enum NavigationToEnum
    {
        Login,
        Admin,
        User,
        Test
    }

    public sealed class NavigateToViewModel
    {
        public NavigateToViewModel(NavigationToEnum navigationTo)
        {
            NavigateTo = navigationTo;
        }

        public NavigateToViewModel(NavigationToEnum navigationTo,Account account)
        {
            NavigateTo = navigationTo;
            Account = account;
        }

        public NavigateToViewModel(NavigationToEnum navigationTo, ICollection<Question> questions, Account account, Part part) : this(navigationTo,account)
        {
            Questions = questions;
            Part = part;
        }

        public ICollection<Question> Questions { get; set; }

        public Part Part { get; set; }

        public Account Account { get; set; }

        public NavigationToEnum NavigateTo { get; }
    }
       
}

