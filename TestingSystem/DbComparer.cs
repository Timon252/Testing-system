using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.Models;

namespace TestingSystem
{
    public class DBComparer : IEqualityComparer<Answer>
    {

        public bool Equals(Answer x, Answer y)
        {
            
            return x.Text != null && y.Text != null ? x.Text.Equals(y.Text) && x.IsCorrect.Equals(y.IsCorrect) : false;
        }

        public int GetHashCode(Answer obj)
        {
            if (obj == null || obj.Text == null) return 0;
            return obj.Text.GetHashCode() ^ obj.IsCorrect.GetHashCode();
        }
    }
}

