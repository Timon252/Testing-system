using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
   
    class UserTestViewModel : Screen , IHandle<NavigateToTest>
    {
        public const int SecondforQuestion = 40;

        private readonly IEventAggregator _eventAggregator;
        private DispatcherTimer _dispatcherTimer;
        private Question _question;
        private Answer _selectedAnswer;
        private int _numberQuestion;

        public UserTestViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
         
        }

        public UserTestViewModel(IEventAggregator eventAggregator, ICollection<Question> questions, Account account, Part part) : this(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            Questions = questions.ToList();
            Account = account;
            Part = part;
        }

        public List<Question> Questions { get; set; }

        public Account Account { get; set; }

        public Part Part { get; set; }


        private int _countCorrect=0;

        public int CountCorrect
        {
            get { return _countCorrect; }
            set { _countCorrect = value; }
        }


        public int NumberQuestion
        {
            get { return _numberQuestion; }
            set
            {
                _numberQuestion = value;
                NotifyOfPropertyChange(() => NumberQuestion);
            }
        }

        private bool _isSelectedItem = false;

        public bool IsSelectedItem
        {
            get { return _isSelectedItem; }
            set { 
                
                _isSelectedItem = value;
                NotifyOfPropertyChange(() => IsSelectedItem);
            
            }
        }


        private TimeSpan _timeTest;

        public TimeSpan TimeTest
        {
            get { return _timeTest; }
            set
            {
                _timeTest = value;
                NotifyOfPropertyChange(() => TimeTest);
            }
        }


        public Question Question
        {
            get { return _question; }
            set
            {
                _question = value;
                NotifyOfPropertyChange(() => Question);
            }
        }

        public Answer SelectedAnswer
        {
            get { return _selectedAnswer; }
            set
            {
                _selectedAnswer = value;

                if(_selectedAnswer != null)
                {
                    if (_selectedAnswer.IsCorrect == true)
                        CountCorrect++;
                    IsSelectedItem = true;
                }
                else
                    IsSelectedItem = false;
      

                NotifyOfPropertyChange(() => SelectedAnswer);
            }
        }
    
        protected override void OnActivate()
        {
            base.OnActivate();
            Question = Questions[++NumberQuestion - 1];
            TimeTest = GetTimeTest(Questions.Count * SecondforQuestion);
            StartTimer();
        }

        private void StartTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += Timer_Tick;
            _dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           if(TimeTest.Minutes == 0 && TimeTest.Seconds == 0)
            {
                _dispatcherTimer.Stop();
                MessageBox.Show("Время вышло");
                SaveResault();
            }
           else
          TimeTest = TimeTest.Subtract(TimeSpan.FromSeconds(1));
        }

        TimeSpan GetTimeTest(int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public void NextQuestion()
        {
            if(NumberQuestion == Questions.Count)
            {
                _dispatcherTimer.Stop();
                MessageBox.Show("Тест пройден");
                SaveResault();
            }
            else
            Question = Questions[++NumberQuestion - 1];
        } 

        void SaveResault()
        {
            using(var context = new TestingSystemContext())
            {
                var testParts = new TestPart() { AccountId = Account.AccountId, PartId = Part.PartId , CountCorrect =CountCorrect, Date = DateTime.Now};
                context.TestParts.Add(testParts);
                context.SaveChanges();
            }
            _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.User,Account));
        }

        public void Handle(NavigateToTest message)
        {
            //if(message.Questions is List<Question>)
            //{
            //    Questions = message.Questions.ToList();
            //    Account = message.Account;
            //    Part = message.Part;
            //}
        }
    }
}
