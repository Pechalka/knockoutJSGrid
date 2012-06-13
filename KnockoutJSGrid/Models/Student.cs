using System.Collections.Generic;

namespace KnockoutJSGrid.Models
{
    public class Student : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Grage { get; set; }
        public string Teacher { get; set; }
        public int Record { get; set; }
        public string Status { get; set; }
    }

    public class StudentViewModel
    {
        public string FindBy { get; set; }
        public Sorting Sort { get; set; }
        public StudentFilter Filter { get; set; }
    }

    public class StudentFilter
    {
        public string SelectedStatus { get; set; }
        public List<KeyValuePair<string, string>> Statuses { get; set; }

        public string SelectedStaff { get; set; }
        public List<KeyValuePair<string, string>> Staffs { get; set; }

        public string SelectedGrade { get; set; }
        public List<KeyValuePair<string, string>> GradeList { get; set; }
        

        public List<KeyValuePair<string, string>> Conditions { get; set; }
    }
}