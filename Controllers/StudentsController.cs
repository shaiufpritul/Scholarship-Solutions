using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class StudentsController : Controller
    {
        private final_32_dbbEntities db = new final_32_dbbEntities();

        public ActionResult Index(string searching)
        {
            return View(db.Students.Where(x => x.FirstName.Contains(searching) || x.LastName.Contains(searching) || x.Department.Contains(searching) || searching == null).ToList());
            //return View(db.Students.ToList());
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Student student, Student imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //    imageModel.StudentPicture = "~/Content/img/StudentPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/img/StudentPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                //db.Students.Add(student);
                db.Students.Add(imageModel);
                db.SaveChanges();

                return Content("SignUp Successfull");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TemoStudent tempStudent)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Where(u => u.Email.Equals(tempStudent.Email)
                && u.Password.Equals(tempStudent.Password)).FirstOrDefault();

                if (student != null)
                {
                    //return Content("Login Successfull");
                    //return RedirectToAction("Dashboard", new { email = student.Email });
                    Session["user_email"] = student.Email;
                    Session["user_id"] = student.StudentID;
                    return RedirectToAction("DashBoard");
                }
                else
                {
                    ViewBag.LoginFailed = "User not found";
                    return View();
                }

            }
            return View();

        }

        //public ActionResult Dashboard(string email)
        //{
        //    string user_email = Convert.ToString(Session["user_email"]);
        //    var user = db.Students.Where(u => u.Email.Equals(user_email)).FirstOrDefault();

        //    return View(user);
        //}
        

        public ActionResult IndexGrade()
        {
            return View(db.Students.ToList());
        }

        public ActionResult search(string searching)
        {

            return View(db.Universities.Where(x => x.UniName.Contains(searching) || x.City.Contains(searching) || x.Country.Contains(searching) || searching == null).ToList());


        }
        public ActionResult Professor()
        {
            return View(db.Professors.ToList());
        }

        public ActionResult StudentScholarship()
        {

            var studentscholar = db.Scholarships.Include(p => p.Professor).Include(p => p.University);
            return View(studentscholar.ToList());
        }

        // GET: Students/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = db.Students.Find(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dynamic dy = new ExpandoObject();
            dy.StudentCourseList = getStudentCourseResults(id);
            dy.StudentExpertiseList = getStudentExpertise(id);
            dy.StudentInterestList = getStudentInterest(id);
            dy.Student = getStudent(id); 
            return View(dy);
        }
        public ActionResult Dashboard(string email)
        {
            //string user_email = Convert.ToString(Session["user_email"]);
            int id = Convert.ToInt32(Session["user_id"]);
            //var user = db.Students.Where(u => u.Email.Equals(user_email)).FirstOrDefault();
            dynamic dy1 = new ExpandoObject();
            dy1.StudentCourseList = getStudentCourseResults(id);
            dy1.StudentExpertiseList = getStudentExpertise(id);
            dy1.StudentInterestList = getStudentInterest(id);
            dy1.Student = getStudent(id);
            return View(dy1);
            // return View(user);
        }
        public List<StudentCourseInfo> getStudentCourseResults(int? id)
        {
            final_32_dbbEntities db = new final_32_dbbEntities();
            string query = $"select StudentCourseResult.* ,  Course.* from StudentCourseResult FULL JOIN Course ON Course.CourseID = StudentCourseResult.CourseID WHERE StudentCourseResult.StudentId =  { id } ";
            List<StudentCourseResult> LStudentCourseResult = db.Database.SqlQuery<StudentCourseResult>(query).ToList();
            List<StudentCourseInfo> result = (from a in LStudentCourseResult
                          join c in db.Courses on a.CourseID equals c.CourseID
                          select new StudentCourseInfo
                          {
                              Grade = a.Grade,
                              CourseName = c.CourseName
                          }).ToList();
            return result;
        }
        public List<StudentInterestInfo> getStudentInterest(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            string query = $"select StudentInterest.* ,  Interest.* from StudentInterest FULL JOIN Interest ON StudentInterest.InterestID = Interest.InterestID WHERE StudentInterest.StudentId = { id } ";
            List<StudentInterest> LStudentInterest = db.Database.SqlQuery<StudentInterest>(query).ToList();
            List<StudentInterestInfo> result = (from a in LStudentInterest
                                                join c in db.Interests on a.InterestID equals c.InterestID
                                              select new StudentInterestInfo
                                              {
                                                  TopicName = c.TopicName,
                                                  Reason = a.Reason
                                              }).ToList();
            return result;
        }
        public List<StudentExpertiseInfo> getStudentExpertise(int? id)
        {
            final_32_dbbEntities db = new final_32_dbbEntities();
            string query = $"select StudentExpertise.* ,  Expertise.* from StudentExpertise FULL JOIN Expertise ON StudentExpertise.ExpertiseID = Expertise.ExpertiseID WHERE StudentExpertise.StudentId =  { id } ";
            List<StudentExpertise> LStudentExpertise = db.Database.SqlQuery<StudentExpertise>(query).ToList();
            List<StudentExpertiseInfo> result = (from a in LStudentExpertise
                        join c in db.Expertises on a.ExpertiseID equals c.ExpertiseID
                        select new StudentExpertiseInfo
                        {
                            TopicName = c.TopicName,
                            Institution = a.Institution
                        }).ToList();
            return result;
        }

        public Student getStudent(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            Student student = db.Students.Find(id);
            return student;
        }







        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,StudentPicture,Department,Session,Year,Semester,Email,AverageGrade,CompletedCredit,ExtracurricularActivities,Blog,Reference,Password")] Student student, Student imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
         //   imageModel.StudentPicture = "~/Content/img/StudentPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/img/StudentPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }




        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
