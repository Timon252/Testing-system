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
using System.Windows.Input;
using System.Windows.Media;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class AdminTreeViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly AdminTopicViewModel _adminTopicViewModel;
        private readonly AdminQuestionViewModel _adminQuestionViewModel;
        private dynamic _selectedItem;
        private Visibility _countQuestionsVisibility = Visibility.Collapsed;
        private int _numberQuestion;

        public AdminTreeViewModel(
            IEventAggregator eventAggregator,
            AdminTopicViewModel adminTopicViewModel,
            AdminQuestionViewModel adminQuestionViewModel
            )
        {
            _eventAggregator = eventAggregator;
            _adminTopicViewModel = adminTopicViewModel;
            _adminQuestionViewModel = adminQuestionViewModel;
            Context = new TestingSystemContext();
            Context.Sections.Load();
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
                NotifyOfPropertyChange(() => CanSelectedItemDelete);
                NotifyOfPropertyChange(() => CanSelectedItemAdd);
            }
        }


        public Visibility CountQuestionsVisibility
        {
            get { return _countQuestionsVisibility; }
            set
            {
                _countQuestionsVisibility = value;
                NotifyOfPropertyChange(() => CountQuestionsVisibility);
            }
        }

        public int NumberQuestion
        {
            get { return _numberQuestion; }
            set
            {
                _numberQuestion = value;
                NavigateToQuestion(_numberQuestion - 1);
                NotifyOfPropertyChange(() => NumberQuestion);
                NotifyOfPropertyChange(() => CanNavigateToPreviousQuestion);
                NotifyOfPropertyChange(() => CanNavigateToNextQuestion);
                NotifyOfPropertyChange(() => CanQuestionDelete);
            }
        }

        public void SelectedItemChanged(object obj)
        {
            IsInEditMode = false;
            SelectedItem = obj;

            if(SelectedItem is Section)
            {
                CountQuestionsVisibility = Visibility.Collapsed;
                ActivateItem(null);
            }
            if(SelectedItem is Part)
            {
                CountQuestionsVisibility = Visibility.Visible;
                NumberQuestion = 1;
            }
            else
            if (SelectedItem is Topic)
            {
                CountQuestionsVisibility = Visibility.Collapsed;
                _eventAggregator.PublishOnUIThread(new NavigateToMessageForAdmin(SelectedItem, Context));
                ActivateItem(_adminTopicViewModel);
            }
            
        }

        public void SelectedItemDelete()
        {
            if (SelectedItem is Section)
            {
                Context.Sections.Remove(SelectedItem);
            }
            else
            if (SelectedItem is Part)
            {
                Context.Parts.Remove(SelectedItem);
            }
            else
            if (SelectedItem is Topic)
            {
                Context.Topics.Remove(SelectedItem);
            }
            Context.SaveChanges();
        }

        public bool CanSelectedItemDelete
        {
            get
            {
                if (SelectedItem is Section)
                {
                    return Context.Sections.Local.Count() > 1 ? true : false;
                }
                else
                if (SelectedItem is Part)
                {
                    var sectionId = SelectedItem.SectionId;
                    return Context.Parts.Local.Count(part => part.SectionId == sectionId) > 1 ? true : false;
                }
                else
                if (SelectedItem is Topic)
                {
                    var partId = SelectedItem.PartId;
                    return Context.Topics.Local.Count(topic => topic.PartId == partId) > 1 ? true : false;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SelectedItemAdd()
        {
            if (SelectedItem is Section)
            {
                var countSection = Context.Sections.Count();
                var newTitleSection = $"Section ({++countSection})";

                var newSection = new Section() { Title = newTitleSection };
                var newPart = new Part() { Title = "Part 1", Section = newSection };
                var newTopic = new Topic { Title = "Topic 1", Part = newPart };
                var newQuestion = new Question() { Part = newPart };
                var newAnswers = new List<Answer>() { new Answer() { Question = newQuestion } , new Answer() { Question = newQuestion } };


                Context.Sections.Add(newSection);
                Context.Parts.Add(newPart);
                Context.Topics.Add(newTopic);
                Context.Questions.Add(newQuestion);
                Context.Answers.AddRange(newAnswers);
            }
            else
            if (SelectedItem is Part)
            {
                Section section = SelectedItem.Section;
                var countParts = Context.Parts.Count(part => part.SectionId == section.SectionId);
                var newTitlePart = $"Part ({++countParts})";

                var newPart = new Part() { Title = newTitlePart, Section = section };
                var newTopic = new Topic { Title = "Topic 1", Part = newPart };
                var newQuestion = new Question() { Part = newPart };
                var newAnswers = new List<Answer>() { new Answer() { Question = newQuestion }, new Answer() { Question = newQuestion } };


                Context.Parts.Add(newPart);
                Context.Topics.Add(newTopic);
                Context.Questions.Add(newQuestion);
                Context.Answers.AddRange(newAnswers);
            }
            else
            if (SelectedItem is Topic)
            {
                Part part = SelectedItem.Part;
                var countTopics = Context.Topics.Count(topic => topic.PartId == part.PartId);
                var newTitleTopic = $"Topic ({++countTopics})";

                var newTopic = new Topic { Title = newTitleTopic, Part = part };

                Context.Topics.Add(newTopic);
            }
            Context.SaveChanges();
        }

        public bool CanSelectedItemAdd
        {
            get {
                return SelectedItem != null ? true : false;
            }
        }

        public void QuestionAdd ()
        {
            var newQuestion = new Question() { Part = SelectedItem};
            var newAnswers = new List<Answer>() { new Answer() { Question = newQuestion }, new Answer() { Question = newQuestion } };

            Context.Questions.Add(newQuestion);
            Context.Answers.AddRange(newAnswers);
            Context.SaveChanges();
            NumberQuestion = SelectedItem.Questions.Count;
        }

        public void QuestionDelete ()
        {

            Context.Questions.Remove(SelectedItem.Questions[NumberQuestion - 1]);
            if (NumberQuestion == 1)
                NumberQuestion = 1;
            else
                NumberQuestion--;
            Context.SaveChanges();
        }

        public bool CanQuestionDelete
        {
            get
            {
                return SelectedItem?.Questions.Count == 1 ? false : true;
            }
        }

        bool isInEditMode = false;
        public bool IsInEditMode
        {
            get { return isInEditMode; }
            set
            {
                isInEditMode = value;
                NotifyOfPropertyChange(() => isInEditMode);

            }
        }

        public void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (FindTreeItem(e.OriginalSource as DependencyObject).IsSelected)
            {
                IsInEditMode = true;
                e.Handled = true;
            }
        }

        static TreeViewItem FindTreeItem(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);
            return source as TreeViewItem;
        }

        public void IsVisibleChanged(object sender)
        {
            var tb = sender as TextBox;
            if (tb.IsVisible)
            {
                tb.Focus();
                tb.SelectAll();
            }
        }

        public void LostFocus(object obj)
        {
            IsInEditMode = false;
            SelectedItem.Title = obj.ToString();
            Context.SaveChanges();
        }

        public void Exit ()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewModel(NavigationToEnum.Login));
        }

        public void NavigateToPreviousQuestion()
        {
            NumberQuestion--;
        }

        public void NavigateToNextQuestion()
        {
            NumberQuestion++;
        }

        public bool CanNavigateToPreviousQuestion
        {
            get { return NumberQuestion == 1 ? false : true; }
        }

        public bool CanNavigateToNextQuestion
        {
            get { return NumberQuestion == SelectedItem?.Questions.Count ? false : true; }
        }

        private void NavigateToQuestion (int index)
        {
            var question = SelectedItem.Questions[index];
            _eventAggregator.PublishOnUIThread(new NavigateToMessageForAdmin(question, Context));
            ActivateItem(_adminQuestionViewModel);
        }
    }
}
