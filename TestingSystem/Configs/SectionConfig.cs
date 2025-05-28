using System.Data.Entity.ModelConfiguration;
using TestingSystem.Models;

namespace TestingSystem.Configs
{
    class SectionConfig : EntityTypeConfiguration<Section>
    {
        public SectionConfig()
        {
            HasKey(section => section.SectionId);
            HasMany(section => section.Parts).WithRequired(section => section.Section).HasForeignKey(section => section.SectionId).WillCascadeOnDelete(true);
        }
    }
}
