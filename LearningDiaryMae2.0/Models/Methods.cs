using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearningDiaryMae2.Models
{
    [NotMapped]public class Methods
    {
        public static bool DateCheck(DateTime first, DateTime second)
        {
            return second > first;
        }
    }
}
