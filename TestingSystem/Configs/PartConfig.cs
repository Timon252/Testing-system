using System.Data.Entity.ModelConfiguration;
using TestingSystem.Models;

namespace TestingSystem.Configs 
{
    class PartConfig : EntityTypeConfiguration<Part>
    {
        public PartConfig()
        {
            HasKey(part => part.PartId);
            HasMany(part => part.Topics).WithRequired(part => part.Part).HasForeignKey(part => part.PartId).WillCascadeOnDelete(true);
            HasMany(part => part.Questions).WithRequired(part => part.Part).HasForeignKey(part => part.PartId).WillCascadeOnDelete(true);
        }
    }
}
