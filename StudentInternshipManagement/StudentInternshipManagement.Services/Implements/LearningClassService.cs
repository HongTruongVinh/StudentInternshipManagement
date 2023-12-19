using System.Linq;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Repositories.Implements;

namespace StudentInternshipManagement.Services.Implements
{
    #region Interface

    public interface ILearningClassService : IGenericService<LearningClass>
    {
        IQueryable<Student> GetStudentList(int classId);

        
        IQueryable<LearningClass> GetAllLearningClassToRigisterInternship(int lastestSemesterId, Student student);// vinh đã thêm hàm này
    }

    #endregion

    #region Class

    public class LearningClassService : GenericService<LearningClass>, ILearningClassService
    {
        private readonly ILearningClassStudentService _learningClassStudentService;

        public LearningClassService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public LearningClassService(IUnitOfWork unitOfWork, ILearningClassStudentService learningClassStudentService) :
            base(unitOfWork)
        {
            _learningClassStudentService = learningClassStudentService;
        }


        public IQueryable<Student> GetStudentList(int classId)
        {
            return _learningClassStudentService.GetByClass(classId).Select(m => m.Student);
        }

        public IQueryable<LearningClass> GetAllLearningClassToRigisterInternship(int lastestSemesterId, Student student)
        {
            var allClass = GetAll().Where(x => x.SemesterId == lastestSemesterId && x.Subject.DepartmentId == student.Class.DepartmentId);

            return allClass;
        }
    }

    #endregion

}