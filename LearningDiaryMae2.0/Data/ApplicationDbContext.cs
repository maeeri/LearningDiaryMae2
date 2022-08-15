using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LearningDiaryMae2.Models;

namespace LearningDiaryMae2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DiaryTopic> DiaryTopic { get; set; }
        public DbSet<DiaryTask> DiaryTask { get; set; }
    }
}
