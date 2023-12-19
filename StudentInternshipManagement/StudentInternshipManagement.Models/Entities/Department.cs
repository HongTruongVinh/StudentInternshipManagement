using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentInternshipManagement.Models.Entities
{
    public class Department : BaseEntity
    {
        [DisplayName("Mã Khoa")]
        public override int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Tên Khoa")]
        public string DepartmentName { get; set; }
    }
}