using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TestingSystem.Models;
using TestingSystem.TestingSystemDbContext;

namespace TestingSystem.ViewModels
{
    class AdminTopicViewModel : Caliburn.Micro.Screen , IHandle<NavigateToMessageForAdmin>
    {
        private readonly IEventAggregator _eventAggregator;
        private byte[] _topicImage;
        private string _topicText;
        private Visibility _topicImageEditorVisibility = Visibility.Collapsed;
        private Visibility _topicImageAddButtonVisibility;

        public AdminTopicViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public TestingSystemContext Context { get; set; }

        public Topic Topic { get; set; }

        public byte[] TopicImage
        {
            get { return _topicImage; }
            set
            {
                _topicImage = value;
                if (_topicImage != null)
                {
                    TopicImageAddButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    TopicImageAddButtonVisibility = Visibility.Visible;
                }
                NotifyOfPropertyChange(() => TopicImage);
                NotifyOfPropertyChange(() => CanTopicSave);
            }
        }

        public string TopicText
        {
            get { return _topicText; }
            set
            {
                _topicText = value;
                NotifyOfPropertyChange(() => TopicText);
                NotifyOfPropertyChange(() => CanTopicSave);
            }
        }

        public Visibility TopicImageEditorVisibility
        {
            get
            {
                return _topicImageEditorVisibility;
            }
            set
            {
                _topicImageEditorVisibility = value;
                NotifyOfPropertyChange(() => TopicImageEditorVisibility);
            }
        }

        public Visibility TopicImageAddButtonVisibility
        {
            get
            {
                return _topicImageAddButtonVisibility;
            }
            set
            {
                _topicImageAddButtonVisibility = value;
                NotifyOfPropertyChange(() => TopicImageAddButtonVisibility);
            }
        }

        public bool CanTopicSave
        {
           get
            {
                if (TopicImage != Topic.Image || TopicText != Topic.Text) 
                {
                    if (!string.IsNullOrEmpty(TopicText))
                    {
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            }
        }

        public void TopicSave()
        {
            Topic.Image = TopicImage;
            Topic.Text = TopicText;
            Context.SaveChanges();
            NotifyOfPropertyChange(() => CanTopicSave);
        }

        public void TopicImageDelete()
        {
            TopicImage = null;
        }

        public void TopiceImageChoice()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TopicImage = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        public void TopicImageMouseEnter()
        {
            TopicImageEditorVisibility = Visibility.Visible;
        }

        public void TopicImageMouseLeave()
        {
            TopicImageEditorVisibility = Visibility.Collapsed;
        }

        public void Handle(NavigateToMessageForAdmin message)
        {
            if (message.Message is Topic)
            {
                Context = message.Context;
                Topic = message.Message as Topic;
                TopicImage = Topic.Image;
                TopicText = Topic.Text;
            }

        }
    }
}
