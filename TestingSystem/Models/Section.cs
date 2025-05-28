using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
    public class Section
    {
        public int SectionId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
        public Section()
        {
            Parts = new ObservableCollection<Part>();
        }
    }
}
