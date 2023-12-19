using StudentInternshipManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class LearningClassStudentViewModel
    {
        public int ClassId { get; set; }
        public string StudentId { get; set; }
        public float? MidTermPoint { get; set; }
        public float? EndTermPoint { get; set; }
        public float? TotalPoint { get; set; }

        public LearningClassStudentViewModel() { }

        public List<LearningClassStudentViewModel> convertStudentsToList(List<Student> students)
        {
            List<LearningClassStudentViewModel> listLearningClassStudentViewModels = new List<LearningClassStudentViewModel>();

            foreach (Student student in students)
            {
                ClassId = student.ClassId;
                StudentId = student.UserId;
            }

            return listLearningClassStudentViewModels;
        }

    }
}
