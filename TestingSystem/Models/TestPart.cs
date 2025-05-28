using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
   public class TestPart
    {
        [Key,Column(Order =0)]

        public int PartId { get; set; }

        [Key, Column(Order = 1)]
        public int AccountId { get; set; }



        public Part Part { get; set; }
        public Account Account { get; set; }

        public int CountCorrect { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
    }
}
