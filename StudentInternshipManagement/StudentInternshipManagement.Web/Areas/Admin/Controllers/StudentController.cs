using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using StudentInternshipManagement.Models.Entities;
using StudentInternshipManagement.Services;
using StudentInternshipManagement.Services.Implements;
using StudentInternshipManagement.Services.ViewModel;
using StudentInternshipManagement.Web.Controllers;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

namespace StudentInternshipManagement.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : AdminBaseController
    {
        private readonly ILearningClassService _learningClassService;
        private readonly IStudentClassService _studentClassService;
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, IStudentClassService studentClassService,
            ILearningClassService learningClassService)
        {
            _studentService = studentService;
            _studentClassService = studentClassService;
            _learningClassService = learningClassService;
        }

        public ActionResult Index()
        {
            ViewBag.StudentClasses = _studentClassService.GetAll();
            ViewBag.LearningClasses = _learningClassService.GetAll();

            return View();
        }

        public ActionResult Students_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<StudentViewModel> studentViewModels = StudentViewModel.convertEntitiesToListViewModel(_studentService.GetAll().ToList());

            DataSourceResult result = studentViewModels.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Create([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //var student = StudentViewModel.convertViewModelToEntity(studentViewModel, _studentService);
                //_studentService.Add(student);//Hàm cũ hoạt động bị lỗi

                var result = AddUserService.AddStudent(new StudentInternshipManagement.Models.Contexts.WebContext(), viewModel, User.Identity.GetUserId());
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Update([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var student = StudentViewModel.convertViewModelToEntity(viewModel, _studentService);

                _studentService.Update(student);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Students_Destroy([DataSourceRequest] DataSourceRequest request,
            global::StudentInternshipManagement.Services.ViewModel.StudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = StudentViewModel.convertViewModelToEntity(viewModel, _studentService);

                _studentService.Delete(student);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetLearningClassList([DataSourceRequest] DataSourceRequest request, int studentId)
        {

            DataSourceResult result = _studentService.GetById(studentId).LearningClassStudents.ToDataSourceResult(
                request, student => new
                {
                    student.StudentId,
                    student.ClassId,
                    student.MidTermPoint,
                    student.EndTermPoint,
                    student.TotalPoint,
                    student.CreatedAt,
                    student.CreatedBy,
                    student.UpdatedAt,
                    student.UpdatedBy,
                    student.IsDeleted
                });

            return Json(result);
        }



        public ActionResult AddStudentsByExcelSheet()
        {
            return View();
        }

        static List<StudentViewModel> studentSheet;

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadStudentSheet(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                List<string> notExistStudentClassNames = new List<string>();

                string extension = Path.GetExtension(file.FileName);

                if (extension == ".xls" || extension == ".xlsx" || extension == ".xlt" || extension == ".xlsm" || extension == ".csv")
                {
                    string rootFolder = Server.MapPath("/Data/Excel/");
                    string pathFile =  rootFolder + new Random().Next(10000, 99999).ToString() + file.FileName;
                    file.SaveAs(pathFile);

                    studentSheet = new List<StudentViewModel>();

                    //create the Application object we can use in the member functions.
                    Microsoft.Office.Interop.Excel.Application _excelApp = new Microsoft.Office.Interop.Excel.Application();
                    _excelApp.Visible = true;

                    //string fileName = file.FileName;

                    //open the workbook
                    Workbook workbook = _excelApp.Workbooks.Open(pathFile,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing); 

                    //select the first sheet        
                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];

                    //find the used range in worksheet
                    Range excelRange = worksheet.UsedRange;

                    //get an object array of all of the cells in the worksheet (their values)
                    object[,] valueArray = (object[,])excelRange.get_Value(
                                XlRangeValueDataType.xlRangeValueDefault);

                    int studentUserName = int.Parse(AddUserService.GenerateStudentUserName(_studentService));

                    //access the cells
                    for (int row = 1; row <= worksheet.UsedRange.Rows.Count; ++row)
                    {
                        StudentViewModel newStudent = new StudentViewModel();

                        newStudent.UserName = studentUserName.ToString(); 

                        for (int col = 1; col <= worksheet.UsedRange.Columns.Count; ++col)
                        {

                            if (col == 1)
                            {
                                newStudent.FullName = valueArray[row, col].ToString();
                            } else if(col == 2)
                            {
                                newStudent.ClassName = valueArray[row, col].ToString();

                                StudentClass studentClass = _studentClassService.GetByRelatedClassName(valueArray[row, col].ToString());
                                if (studentClass != null)
                                {
                                    newStudent.ClassId = studentClass.Id;
                                    newStudent.ClassName = studentClass.ClassName;
                                }
                                else
                                {
                                    notExistStudentClassNames.Add(valueArray[row, col].ToString());
                                }
                            } else if (col == 3)
                            {
                                newStudent.Cpa = float.Parse(valueArray[row, col].ToString());
                            }
                            else if (col == 4)
                            {
                                newStudent.Phone = valueArray[row, col].ToString();
                            }
                            else if (col == 5)
                            {
                                newStudent.BirthDate = DateTime.Parse(valueArray[row, col].ToString());
                            }
                            else if (col == 6)
                            {
                                newStudent.Address = valueArray[row, col].ToString();
                            }
                        }

                        studentSheet.Add(newStudent);
                        studentUserName++;
                    }

                    //clean up stuffs
                    workbook.Close(false, Type.Missing, Type.Missing);
                    Marshal.ReleaseComObject(workbook);

                    _excelApp.Quit();
                    Marshal.FinalReleaseComObject(_excelApp);

                    if (System.IO.File.Exists(pathFile))
                    {
                        System.IO.File.Delete(pathFile);
                    }
                }
                else
                {
                    ViewBag.Message = "Please choose a excel file to upload";
                    return View("AddStudentsByExcelSheet");
                }

                if (notExistStudentClassNames.Count > 0)
                {
                    ViewBag.Message = "Lỗi không tồn tại lớp";
                    ViewBag.NotExistStudentClassNames = notExistStudentClassNames;
                    return View("AddStudentsByExcelSheet");
                }

                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                ViewBag.Message = "Please choose a file to upload";
            }

            return View("AddStudentsByExcelSheet");
        }

        public ActionResult StudentSheet_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = studentSheet.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveStudentExcelSheet(List<StudentViewModel> rows)
        {
            var dictionaryFailureResult = AddUserService.AddStudentsByList(new StudentInternshipManagement.Models.Contexts.WebContext(), rows, User.Identity.GetUserId()).Where(x => x.Value == "failure");

            ViewBag.Message = "Lưu thành công";

            ViewBag.DictionaryResult = dictionaryFailureResult;

            string result = "success";

            return Json(new
            {
                result = result,
                dictionaryFailureResult = dictionaryFailureResult
            }
            , JsonRequestBehavior.AllowGet);

        }
    }
}