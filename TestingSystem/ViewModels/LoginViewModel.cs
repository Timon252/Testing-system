using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class LoginViewModel : Screen 
    {

        private Group _selectedGroup;
        private readonly IEventAggregator _eventAggregator;

        TestingSystemContext Context { get; set; } 
        public LoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Context = new TestingSystemContext();
            GroupCollection =  Context.Groups.ToList();
        }

        private List<Group> _groupCollection;

        public List<Group> GroupCollection
        {
            get { return _groupCollection; }
            set { _groupCollection = value; }
        }

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set {
                _selectedGroup = value;
                NotifyOfPropertyChange(() => SelectedGroup);
                NotifyOfPropertyChange(() => CanSignInButtonClick);
            }
        }

        private string _signInPassword;

        public string SignInPassword
        {
            get { return _signInPassword; }
            set { 
                _signInPassword = value;
                NotifyOfPropertyChange(() => SignInPassword);
                NotifyOfPropertyChange(() => CanSignInButtonClick);
            }
        }

        private string _signInLogin;

        public string SignInLogin
        {
            get { return _signInLogin; }
            set
            {
                _signInLogin = value;
                NotifyOfPropertyChange(() => SignInLogin);
                NotifyOfPropertyChange(() => CanSignInButtonClick);
            }
        }

        private string _signInName;

        public string SignInName
        {
            get { return _signInName; }
            set {
                _signInName = value;
                NotifyOfPropertyChange(() => SignInName);
                NotifyOfPropertyChange(() => CanSignInButtonClick);
            }
        }

        private string _logInLogin;

        public string LogInLogin
        {
            get { return _logInLogin; }
            set { 
                _logInLogin = value;
                NotifyOfPropertyChange(() => LogInLogin);
                NotifyOfPropertyChange(() => CanLogInButtonClick);
            }
        }

        private string _logInPassword;

        public string LogInPassword
        {
            get { return _logInPassword; }
            set {
                _logInPassword = value;
                NotifyOfPropertyChange(() => LogInPassword);
                NotifyOfPropertyChange(() => CanLogInButtonClick);
            }
        }



        public void SignInPasswordChanged (RoutedEventArgs obj)
        {
            SignInPassword = (obj.Source as PasswordBox).Password;
        }

        public void LogInPasswordChanged(RoutedEventArgs obj)
        {
            LogInPassword = (obj.Source as PasswordBox).Password;
        }


        public void SignInButtonClick ()
        {
            Account newaccount = new Account { Login = SignInLogin, Name = SignInName , Password = CalculateHash(SignInPassword), Group = SelectedGroup };
            Context.Accounts.Add(newaccount);
            Context.SaveChanges();
        }

        private string CalculateHash(string clearTextPassword)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }

        public void LogInButtonClick()
        {
            if (LogInLogin == "admin" && LogInPassword == "1")
                _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.Admin));
           else
            {
                var password= CalculateHash(LogInPassword);
                Account account = Context.Accounts.FirstOrDefault(acc => acc.Login.Equals(LogInLogin) && acc.Password.Equals(password));

                if (account != null)
                {
                    _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.User,account));

                }
                else
                    MessageBox.Show("Неправильный логин или пароль");
            }
          
        }

        public bool CanSignInButtonClick
        {
            get {
                if (string.IsNullOrEmpty(SignInLogin) || string.IsNullOrEmpty(SignInName) || string.IsNullOrEmpty(SignInPassword) || SelectedGroup == null)
                    return false;
                return true;
            }
          
        }

        public bool CanLogInButtonClick
        {
            get
            {
                if (string.IsNullOrEmpty(LogInLogin) || string.IsNullOrEmpty(LogInPassword))
                    return false;
                return true;
            }

        }



    }
}
