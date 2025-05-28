using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem.Configs
{
    class AccountConfig : EntityTypeConfiguration<Account>
    {
        public AccountConfig()
        {
            HasKey(account => account.AccountId);
            Property(account => account.Login).IsRequired().HasMaxLength(50);
            Property(account => account.Name).IsRequired().HasMaxLength(300);
            Property(account => account.Password).IsRequired();
        }

    }
}
