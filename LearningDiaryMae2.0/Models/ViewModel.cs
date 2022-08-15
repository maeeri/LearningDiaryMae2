using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearningDiaryMae2.Models
{
    public class ViewModel
    {
        [NotMapped] public List<DiaryTopic> Topics { get; set; }
        [NotMapped] public DiaryTopic Topic { get; set; }
        [NotMapped] public List<DiaryTask> Tasks { get; set; }
        [NotMapped] public DiaryTask Task { get; set; }
    }
}
