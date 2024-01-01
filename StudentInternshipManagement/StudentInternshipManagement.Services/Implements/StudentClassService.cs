using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Repositories.Implements;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentInternshipManagement.Services.Implements
{
    #region Interface

    public interface IStudentClassService : IGenericService<StudentClass>
    {
        StudentClass GetByClassName(string className);
        StudentClass GetByRelatedClassName(string className);
    }

    #endregion

    #region Class

    public class StudentClassService : GenericService<StudentClass>, IStudentClassService
    {
        public StudentClassService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public StudentClass GetByClassName(string className)
        {
            return UnitOfWork.Repository<StudentClass>().TableNoTracking.Where(x => x.ClassName == className).FirstOrDefault();
        }

        public StudentClass GetByRelatedClassName(string className)
        {
            ReplaceWhitespace(className, "");
            return UnitOfWork.Repository<StudentClass>().TableNoTracking.Where(x => x.ClassName == className).ToList().FirstOrDefault();
        }
        
        public static string ReplaceWhitespace(string input, string replacement)
        {
            Regex sWhitespace = new Regex(@"\s+");
            return sWhitespace.Replace(input, replacement);
        }
    }

    #endregion

}