using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class GradeViewModel
    {
        [Required]
        [DisplayName("Mã học kỳ")]
        public int SemesterId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Mã lớp")]
        public int LearningClassId { get; set; }

        [MaxLength(50)]
        [DisplayName("Tên lớp")]
        public string ClassName { get; set; }

        [DisplayName("Mã môn học")]
        [UIHint("SubjectTemplate")]
        public int SubjectId { get; set; }

        [DisplayName("Môn học")]
        [UIHint("SubjectTemplate")]
        public string SubjectName { get; set; }

        public GradeViewModel() { }
    }
}
