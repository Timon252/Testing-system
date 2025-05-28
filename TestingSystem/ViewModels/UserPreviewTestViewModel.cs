using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class UserPreviewTestViewModel : Screen , IHandle<NavigateForUser>
    {
        public const int SecondforQuestion = 40;

        private readonly IEventAggregator _eventAggregator;
        private readonly Random _random;

        public UserPreviewTestViewModel(IEventAggregator eventAggregator, Part part , Account account)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            Questions = new List<Question>();
            _random = new Random();
            Part = part;
            Account = account;
            Time = GetTimeTest(Part.Questions.Count * SecondforQuestion);

           TestPart = GetTestPart(Account, Part);
        }

        TestPart GetTestPart(Account account, Part part)
        {
            using (var context = new TestingSystemContext())
            {
                var testPart = context.TestParts.FirstOrDefault(tp => tp.AccountId.Equals(account.AccountId) && tp.PartId.Equals(part.PartId));
                return testPart;
            }
 
        }

        public Part Part { get; set; }

        public Account Account { get; set; }

        private TestPart _testPart;

        public TestPart TestPart
        {
            get { return _testPart; }
            set { _testPart = value;
                NotifyOfPropertyChange(() => TestPart);
                    
                    }
        }


        public List<Question> Questions { get; set; }

        private TimeSpan _time;

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value;
                NotifyOfPropertyChange(() => Time);
            
            }
        }


        public void StartTest ()
        {
            Questions = GetShuffleQuestions(Part.Questions).ToList();
            Questions = GetShuffleAnswers(Questions).ToList();

            //_eventAggregator.PublishOnUIThread(new NavigateToTest(Questions,Account, Part));
            _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.Test, Questions, Account, Part));

        }

        public void Handle(NavigateForUser message)
        {
            //if(message.Message is Part)
            //{
            //    Part = message.Message as Part;
            //}
        }

        TimeSpan GetTimeTest(int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        private ICollection<T> GetShuffleCollection<T> (ICollection<T> sourceCollection)
        {
       
            var shuffleCollection = new List<T>();

            foreach (var itemCollection in sourceCollection)
            {
                int j = _random.Next(shuffleCollection.Count + 1);
                if (j == shuffleCollection.Count)
                {
                    shuffleCollection.Add(itemCollection);
                }
                else
                {
                    shuffleCollection.Add(shuffleCollection[j]);
                    shuffleCollection[j] = itemCollection;
                }
            }
            return shuffleCollection;
        }

        private ICollection<Question> GetShuffleQuestions (ICollection<Question> sourceQuestions)
        {
           return GetShuffleCollection(sourceQuestions);
        }

        private ICollection<Question> GetShuffleAnswers(ICollection<Question> sourceQuestions)
        {
           foreach(var question in sourceQuestions)
            {
                question.Answers = GetShuffleCollection(question.Answers);
               
            }

            return sourceQuestions;
        }

    }
}
