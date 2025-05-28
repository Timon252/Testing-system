using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace TestingSystem.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }

        public int PartId { get; set; }
        public virtual Part Part { get; set; }
    }
}
