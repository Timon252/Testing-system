using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestingSystem.Models
{

    public class Part
    {
        public int PartId { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Question> Questions {get; set; }

        public virtual ICollection<TestPart> TestParts { get; }

        public Part()
        {
            Topics = new ObservableCollection<Topic>();
            Questions = new ObservableCollection<Question>();
        }
    }
}
