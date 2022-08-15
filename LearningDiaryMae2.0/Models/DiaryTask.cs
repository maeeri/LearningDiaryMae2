using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearningDiaryMae2.Models
{
    public class DiaryTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        public DateTime? Deadline { get; set; }
        [Range(1,3,ErrorMessage = "Please enter a number between 1 and 3")]
        public int? Priority { get; set; }
        public bool Done { get; set; }

        [DisplayName("Topic")]
        public virtual int DiaryTopic { get; set; }
        [ForeignKey("DiaryTopic")]
        public virtual DiaryTopic DiaryTopics { get; set; }
    }
}
