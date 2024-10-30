using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dynamic__Time_Table.Models
{
    public class TimetableModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalHours => WorkingDays * SubjectsPerDay;

        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }

    public class Subject
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }
}
