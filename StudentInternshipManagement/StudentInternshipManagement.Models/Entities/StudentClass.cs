using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace StudentInternshipManagement.Models.Entities
{
    public class StudentClass : BaseEntity
    {
        [Key]
        [DisplayName("Mã lớp")]
        public override int Id { get; set; }

        /// <summary>
        /// Tên lớp mà sinh viên thộc về, VD: Lớp KTPM2020
        /// </summary>
        [Required]
        [MaxLength(50)]
        [DisplayName("Tên lớp")]
        public string ClassName { get; set; }

        [DisplayName("Khoa")]
        [UIHint("DepartmentTemplate")]
        public int DepartmentId { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual Department Department { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        [DisplayName("Danh sách sinh viên")]
        public virtual ICollection<Student> Students { get; set; }
    }
}