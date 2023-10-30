using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentInternshipManagement.Models.Entities
{
    public class Department : BaseEntity
    {
        [DisplayName("Mã Khoa/Viện")]
        public override int Id { get; set; }

        
        [MaxLength(50)]
        [DisplayName("Tên Khoa/Viện")]
        [Required]
        public string DepartmentName { get; set; }
    }
}