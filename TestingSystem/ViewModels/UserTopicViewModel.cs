using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem.ViewModels
{
    class UserTopicViewModel : Screen , IHandle<NavigateToMessageForUser>
    {
        private readonly IEventAggregator _eventAggregator;

        public UserTopicViewModel(IEventAggregator eventAggregator,Topic topic)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            Topic = topic;
        }


        private Topic _topic;

        public Topic Topic
        {
            get { return _topic; }
            set { _topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }

      

        public void Handle(NavigateToMessageForUser message)
        {
            //if (message.Message is Topic)
            //{
            //    Topic = message.Message as Topic;
            //}

        }

    }
}
