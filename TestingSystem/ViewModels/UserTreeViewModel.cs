using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class UserTreeViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;
        //private readonly UserTopicViewModel _userTopicViewModel;
        //private readonly UserPreviewTestViewModel _userPreviewTestViewModel;

        private dynamic _selectedItem;

        public UserTreeViewModel(
            IEventAggregator eventAggregator)


        {
            _eventAggregator = eventAggregator;
      
            //_userTopicViewModel = userTopicViewModel;
            //_userPreviewTestViewModel = userPreviewTestViewModel;
            Context = new TestingSystemContext();
      
        }

        public UserTreeViewModel(
    IEventAggregator eventAggregator, Account account ) :this(eventAggregator)
        { 

            Account = account;
        }

    

        public Account Account { get; set; }

        private ObservableCollection<Section> _sections;

        public ObservableCollection<Section> Sections
        {
            get { return _sections; }
            set { _sections = value;
                NotifyOfPropertyChange(() => Sections);
            }
        }


        protected override void OnActivate()
        {
            base.OnActivate();
            Sections = new ObservableCollection<Section>(Context.Sections.ToList());
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

        }

        public TestingSystemContext Context { get; set; }

        public dynamic SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public void Handle(NavigateToViewModel message)
        {
        
        }

        public void SelectedItemChanged(object obj)
        {
            SelectedItem = obj;
            if (obj is Topic)
            {
                //_eventAggregator.PublishOnUIThread(new NavigateToMessageForUser(SelectedItem, Context));
                ActivateItem(new UserTopicViewModel(_eventAggregator,SelectedItem) ) ;
            }
            else
                if(obj is Part)
            {
                //_eventAggregator.PublishOnUIThread(new NavigateForUser(SelectedItem));
                ActivateItem(new UserPreviewTestViewModel(_eventAggregator, SelectedItem,Account) );
            }
        }

        public void Exit()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.Login));
        }
    }


}
