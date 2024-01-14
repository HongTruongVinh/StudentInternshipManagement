using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Hangfire;
using Hangfire.Storage;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Ajax.Utilities;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InternshipController : AdminBaseController
    {
        private static int _semester = -1;
        private static string _jobId = string.Empty;

        private readonly IGroupService _groupService;
        private readonly IInternshipService _internshipService;
        private readonly ISemesterService _semesterService;
        private readonly ILearningClassService _learningClassService;

        public InternshipController(IInternshipService internshipService, IGroupService groupService,
            ISemesterService semesterService, ILearningClassService learningClassService)
        {
            _internshipService = internshipService;
            _groupService = groupService;
            _semesterService = semesterService;
            _learningClassService = learningClassService;
        }

        public ActionResult Index()
        {
            CheckJob1();
            
            return View();
        }

        public void CheckJob1()
        {
            int semesterId = _semesterService.GetLatest().Id;
            ViewBag.Semester = _semester;
        }
        public ActionResult Process()
        {
            //_jobId = BackgroundJob.Enqueue(() => _internshipService.ProcessRegistration());

            _internshipService.ProcessRegistration();
            ViewBag.IsProcessing = false;

            _semester = _semesterService.GetLatest().Id;
            return RedirectToAction("Index");
        }

        public void CheckJob()
        {
            int semesterId = _semesterService.GetLatest().Id;
            ViewBag.Semester = _semester;
            if (_semester != semesterId)
            {
                ViewBag.IsProcessing = null;
            }
            else
            {
                IStorageConnection connection = JobStorage.Current.GetConnection();
                JobData jobData = connection.GetJobData(_jobId);
                string stateName = jobData.State;
                switch (stateName)
                {
                    case "Scheduled":
                    case "Awaiting":
                    case "Enqueued":
                        ViewBag.IsProcessing = true;
                        break;

                    case "Succeeded":
                        ViewBag.IsProcessing = false;
                        break;

                    // case "Failed":
                    default:
                        ViewBag.IsProcessing = null;
                        break;
                }
            }
        }

        public ActionResult Internships_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<Internship> internships = _internshipService.GetByLatestSemester();

            DataSourceResult result = internships.ToDataSourceResult(request, internship => new
            {
                internship.Id,
                Semester = internship.Class.SemesterId,
                internship.RegistrationDate,
                internship.Status,
                Student = internship.Student.User.FullName,
                Class = internship.Class.ClassName,
                Company = internship.Major.Company.CompanyName,
                TrainingMajor = internship.Major.TrainingMajor.TrainingMajorName,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.MidTermPoint,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.EndTermPoint,
                internship.Student.LearningClassStudents.FirstOrDefault(l => l.ClassId == internship.ClassId)
                    ?.TotalPoint,
                Group = _groupService.GetByInternship(internship)?.GroupName,
                Teacher = _groupService.GetByInternship(internship)?.Teacher.User.FullName
            });

            return Json(result);
        }

        public ActionResult Grade()
        {
            return View();
        }

        static GradeViewModel _gradeViewModel;

        [HttpPost]
        public ActionResult GradeFiltering(int semesterId, int learningClassId)
        {
            _gradeViewModel = new GradeViewModel() { SemesterId = semesterId, LearningClassId = learningClassId };

            var semester = _semesterService.GetById(semesterId);
            var learningClass = _learningClassService.GetById(learningClassId);
            var subject = learningClass.Subject;

            _gradeViewModel.StartDate = semester.StartDate;
            _gradeViewModel.EndDate = semester.EndDate;
            _gradeViewModel.SubjectName = subject.SubjectName;
            _gradeViewModel.ClassName = learningClass.ClassName;

            return Json(new 
            { 
                status = true,
                semesterId = semesterId,
                startDate = semester.StartDate.ToString("dd-MM-yyyy"),
                endDate = semester.EndDate.ToString("dd-MM-yyyy"),
                subjectName = subject.SubjectName,
                className = learningClass.ClassName,
            });
        }

        
        public ActionResult FilteredGrade_Read([DataSourceRequest] DataSourceRequest request)
        {
            if(_gradeViewModel == null || _gradeViewModel.LearningClassId == 0) { _gradeViewModel = new GradeViewModel() { LearningClassId = 1 }; }
            var learningClass = LearningClassStudentViewModel.convertEntitiesToListViewModel(
                    _learningClassService.GetById(_gradeViewModel.LearningClassId).LearningClassStudents.ToList());

            DataSourceResult result = learningClass.ToDataSourceResult(request, student => new
            {
                student.StudentId,
                student.ClassId,
                student.StudentCode,
                student.StudentClassName,
                student.FullName,
                student.ClassName,
                student.MidTermPoint,
                student.EndTermPoint,
                student.TotalPoint
            });

            return Json(result);
        }

        public ActionResult GetAllSemesters()
        {
            var result = _semesterService.GetAll().Select(x => new 
            { 
                SemesterId = x.Id,
                x.StartDate,
                x.EndDate,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            byte[] fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        /// <summary>
        /// Action này sẽ trả về View lọc kết quả tìm kiếm nhưng tạm thời không cần nữa 
        /// </summary>
        /// <param name="gradeViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FilteredGrade(GradeViewModel gradeViewModel)
        {
            if (ModelState.IsValid)
            {
                var semester = _semesterService.GetById(gradeViewModel.SemesterId);
                var learningClass = _learningClassService.GetById(gradeViewModel.LearningClassId);
                var subject = learningClass.Subject;

                gradeViewModel.StartDate = semester.StartDate;
                gradeViewModel.EndDate = semester.EndDate;
                gradeViewModel.SubjectName = subject.SubjectName;
                gradeViewModel.ClassName = learningClass.ClassName;

                _gradeViewModel = gradeViewModel;

                return View(gradeViewModel);
            }

            return null;
        }


    }
}