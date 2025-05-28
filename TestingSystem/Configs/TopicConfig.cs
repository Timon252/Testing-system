using System.Data.Entity.ModelConfiguration;
using TestingSystem.Models;

namespace TestingSystem.Configs
{
    class TopicConfig : EntityTypeConfiguration<Topic>
    {
        public TopicConfig()
        {
            HasKey(topic => topic.TopicId);
            Property(topic => topic.Title).IsOptional().HasMaxLength(100);
            Property(topic => topic.Text).IsOptional();
            Property(topic => topic.Image).IsOptional();
        }
    }
}
