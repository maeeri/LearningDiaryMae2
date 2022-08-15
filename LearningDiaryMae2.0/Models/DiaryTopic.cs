using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using LearningDiaryMae2.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LearningDiaryMae2.Models
{
    public class DiaryTopic
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("Time to master")]
        public double EstimatedTimeToMaster { get; set; }
        public string Source { get; set; }
        [DisplayName("Start date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        public DateTime StartLearningDate { get; set; }
        public bool InProgress { get; set; }
        [NotMapped]
        [DisplayName("Done")]
        public bool Done
        {
            get => !InProgress;
            private set => Done = !InProgress;
        }
        [DisplayName("Finished")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        [DateCompareValidation(DateCompareValidationAttribute.CompareType.GreaterThanOrEqualTo, "End date must be the same as or after start date", "StartLearningDate")]
        public DateTime? CompletionDate { get; set; }
        [DisplayName("Last edited")]
        public DateTime? LastEditDate { get; set; }
        [DisplayName("Time spent")]
        public double? TimeSpent { get; set; }
    }
}
