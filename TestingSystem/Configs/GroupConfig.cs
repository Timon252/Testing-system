using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem.Configs 
{
    class GroupConfig : EntityTypeConfiguration<Group>
    {
        public GroupConfig()
        {
            HasKey(group => group.GroupId);
            Property(group => group.Title).IsRequired().HasMaxLength(100);
            HasMany(group => group.Accounts).WithRequired(group => group.Group).HasForeignKey(group => group.GroupId).WillCascadeOnDelete(true);
        }
       
    }
}
