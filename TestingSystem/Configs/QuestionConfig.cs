using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem.Configs
{
    class QuestionConfig : EntityTypeConfiguration<Question>
    {
        public QuestionConfig()
        {
            HasKey(question => question.QuestionId);
            Property(question => question.Image).IsOptional();
            Property(question => question.Text).IsOptional().HasMaxLength(500);
            HasMany(question => question.Answers).WithRequired(question => question.Question).HasForeignKey(qustion => qustion.QuestionId).WillCascadeOnDelete(true);
        }
    }
}
