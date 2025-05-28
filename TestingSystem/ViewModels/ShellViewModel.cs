using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class ShellViewModel : Conductor<Screen>.Collection.OneActive, IHandle<NavigateToViewModel>
    {
        private  readonly IEventAggregator _eventAggregator;
        private readonly AdminTreeViewModel _adminTreeViewModel;
        private readonly UserTreeViewModel _userTreeViewModel;
        private readonly UserTestViewModel _userTestViewModel;
        private readonly LoginViewModel _loginViewModel;

        public ShellViewModel(
            IEventAggregator eventAggregator,
            AdminTreeViewModel adminTreeViewModel,
            UserTreeViewModel userTreeViewModel,
            UserTestViewModel userTestViewModel,
            LoginViewModel loginViewModel)
        {
            _eventAggregator = eventAggregator;
            _adminTreeViewModel = adminTreeViewModel;
            _userTreeViewModel = userTreeViewModel;
            _userTestViewModel = userTestViewModel;
            _loginViewModel = loginViewModel;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
            ActivateItem(_loginViewModel);
    
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle (NavigateToViewModel message)
        {
            switch (message.NavigateTo)
            {
                case NavigationToEnum.Login: ActivateItem(_loginViewModel); break;
                case NavigationToEnum.Admin: ActivateItem(_adminTreeViewModel); break;
                case NavigationToEnum.User:  ActivateItem(new UserTreeViewModel(_eventAggregator,message.Account)); break;
                case NavigationToEnum.Test: ActivateItem(new UserTestViewModel(_eventAggregator,message.Questions,message.Account,message.Part)); break;
                default:
                    break;
            }
        }

    }
}
