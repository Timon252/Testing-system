using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
  public  class Question
    {
        public int QuestionId { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }

        public int PartId { get; set; }
        public virtual Part Part { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new ObservableCollection<Answer>();
        }
    }
}
