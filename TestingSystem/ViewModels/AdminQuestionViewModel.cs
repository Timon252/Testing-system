using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class AdminQuestionViewModel : Caliburn.Micro.Screen , IHandle<NavigateToMessageForAdmin>
    {
        private readonly IEventAggregator _eventAggregator;
        private byte[] _questionImage;
        private string _questionText;
        private ObservableCollection<Answer> _questionAnswers;
        private Visibility _questionImageEditorVisibility = Visibility.Collapsed;
        private Visibility _questionImageAddButtonVisibility;

        public AdminQuestionViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public TestingSystemContext Context { get; set; }

        public Question Question { get; set; }




        public byte[] QuestionImage
        {
            get { return _questionImage; }
            set
            {
                _questionImage = value;
                if (_questionImage != null)
                {
                    QuestionImageAddButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    QuestionImageAddButtonVisibility = Visibility.Visible;
                }
                NotifyOfPropertyChange(() => QuestionImage);
                NotifyOfPropertyChange(() => CanSaveQuestion);
            }
        }

        public string QuestionText
        {
            get { return _questionText; }
            set
            {
                _questionText = value;
                NotifyOfPropertyChange(() => QuestionText);
                NotifyOfPropertyChange(() => CanSaveQuestion);
            }
        }


        public ObservableCollection<Answer> QuestionAnswers
        {
            get { return _questionAnswers; }
            set
            {
                _questionAnswers = value;
                NotifyOfPropertyChange(() => QuestionAnswers);

            }
        }


        public Visibility QuestionImageEditorVisibility
        {
            get { return _questionImageEditorVisibility; }
            set
            {
                _questionImageEditorVisibility = value;
                NotifyOfPropertyChange(() => QuestionImageEditorVisibility);
            }
        }

        public Visibility QuestionImageAddButtonVisibility
        {
            get { return _questionImageAddButtonVisibility; }
            set
            {
                _questionImageAddButtonVisibility = value;
                NotifyOfPropertyChange(() => QuestionImageAddButtonVisibility);
            }
        }

        private bool _answerButtonDeleteIsEnabled = true;

        public bool AnswerButtonDeleteIsEnabled
        {
            get { return _answerButtonDeleteIsEnabled; }
            set
            {
                _answerButtonDeleteIsEnabled = value;
                NotifyOfPropertyChange(() => AnswerButtonDeleteIsEnabled);
            }
        }


        public bool CanSaveQuestion
        {
            get
            { 
                if (QuestionImage != Question.Image || QuestionText != Question.Text || IsEditAnswers() == true || QuestionAnswers.Count != Question.Answers.Count )
                {
                    if (!string.IsNullOrEmpty(QuestionText) && !IsEmptyTextAnswers() && IsExistIsCorrectTrueAnswers() == true)
                    {
                        return true;
                    }
                    return false;
                }
                else
                    return false;
                }
        }

        bool IsEditAnswers ()
        {
            return QuestionAnswers.Except(Question.Answers, new DBComparer()).Any();
        }

        bool IsEmptyTextAnswers()
        {
           return QuestionAnswers.Any(q => string.IsNullOrEmpty(q.Text));
        }

        bool IsExistIsCorrectTrueAnswers()
        {
            return QuestionAnswers.Any(q => q.IsCorrect == true);
        }

        public void SaveQuestion()
        {
            Question.Image = QuestionImage;
            Question.Text = QuestionText;
            UpdateAnswers();
            Context.SaveChanges();
            LoadAnswers();
            NotifyOfPropertyChange(() => CanSaveQuestion);
            
        }
      

        public void QuestionImageDelete()
        {
            QuestionImage = null;
        }

        public void QuestioneImageChoice()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                QuestionImage = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        public void QuestionImageMouseEnter()
        {
            QuestionImageEditorVisibility = Visibility.Visible;
        }

        public void QuestionImageMouseLeave()
        {
            QuestionImageEditorVisibility = Visibility.Collapsed;
        }

        public void ChangeAnswer()
        {
            NotifyOfPropertyChange(() => CanSaveQuestion);
        }

        public void Handle(NavigateToMessageForAdmin message)
        {
            if (message.Message is Question)
            {
                Context = message.Context;
                Question = message.Message as Question;  
                QuestionImage = Question.Image;
                QuestionText = Question.Text;
                LoadAnswers();
                NotifyOfPropertyChange(() => CanSaveQuestion);
            }
        }

        public void UpdateAnswers()
        {
            var existingParent = Context.Questions
                .Where(p => p.QuestionId == Question.QuestionId)
                .SingleOrDefault();

                foreach (var existingChild in existingParent.Answers.ToList())
                {
                    if (!QuestionAnswers.Any(c => c.AnswerId == existingChild.AnswerId))
                        Context.Answers.Remove(existingChild);
                }

                foreach (var childModel in QuestionAnswers)
                {
                    var existingChild = existingParent.Answers
                        .Where(c => c.AnswerId == childModel.AnswerId)
                        .SingleOrDefault();

                    if (existingChild != null && existingChild?.AnswerId != 0)
            
                            Context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {

                        var newChild = new Answer
                        {
                            Text = childModel.Text,
                            IsCorrect = childModel.IsCorrect,
                            Question = existingParent,
                            QuestionId = existingParent.QuestionId 
                        };
                        existingParent.Answers.Add(newChild);
                    }

            }
        }

        public void AnswerDelete(object obj)
        {
 
            QuestionAnswers.Remove(obj as Answer);
            if(QuestionAnswers.Count == 2)
            {
                AnswerButtonDeleteIsEnabled = false;
            }
            NotifyOfPropertyChange(() => CanSaveQuestion);
        }

        public void AnswerAdd()
        {
            QuestionAnswers.Add(new Answer());
          
                AnswerButtonDeleteIsEnabled = true;
            NotifyOfPropertyChange(() => CanSaveQuestion);
        }

        public void LoadAnswers()
        {
            Context.Configuration.ProxyCreationEnabled = false;
            QuestionAnswers = new ObservableCollection<Answer>(Context.Answers.Where(answers => answers.QuestionId == Question.QuestionId).AsNoTracking().ToList());
            Context.Configuration.ProxyCreationEnabled = true;
        }
    }
}
