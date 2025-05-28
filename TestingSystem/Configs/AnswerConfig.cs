using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem.Configs
{
    class AnswerConfig : EntityTypeConfiguration<Answer>
    {
        public AnswerConfig()
        {
            HasKey(answer => answer.AnswerId);
            Property(answer => answer.Text).IsOptional().HasMaxLength(200);
        }
    }
}
