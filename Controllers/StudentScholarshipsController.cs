using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class StudentScholarshipsController : Controller
    {
        private final_32_dbbEntities db = new final_32_dbbEntities();

        // GET: StudentScholarships
        public ActionResult Index(int id)
        {
            //var studentScholarships = db.StudentScholarships.Include(s => s.Scholarship).Include(s => s.Student);
            //return View(studentScholarships.ToList());
            string query = $"Select * from StudentScholarship WHERE StudentID =  { id } ";

            var StudentScholarships = db.Database.SqlQuery<StudentScholarship>(query).ToList();
            var result = (from a in StudentScholarships
                          select new StudentScholarship
                          {
                              StudentScholarshipID = a.StudentScholarshipID,
                              ScholarshipID = a.ScholarshipID,
                              StudentID = a.StudentID,
                              Status = a.Status
                          }).ToList();
            return View(result);
        }

        // GET: StudentScholarships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            return View(studentScholarship);
        }

        // GET: StudentScholarships/Create
        public ActionResult Create()
        {
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentScholarships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentScholarshipID,ScholarshipID,StudentID,Status")] StudentScholarship studentScholarship, int id)
        {
            int student_id = Convert.ToInt32(Session["user_id"]);
            studentScholarship.StudentID = student_id;
            studentScholarship.ScholarshipID = id;
            studentScholarship.Status = "Waiting";
            if (ModelState.IsValid)
            {
                db.StudentScholarships.Add(studentScholarship);
                db.SaveChanges();
                return View();
            }

            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }
        public ActionResult AcceptStudent(int? id)
        {
            string query = $"Update StudentScholarship set Status = 'Accept' Where StudentID = {id} ";
            var studentScholarship = db.Database.SqlQuery<StudentScholarship>(query);
            //db.StudentScholarships
            //db.Entry(studentScholarship).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }
        public ActionResult EnrolledStudent(int? id)
        {
            string query = $"Select Student.*, StudentScholarship.* From Student Inner join StudentScholarship on Student.StudentID = StudentScholarship.StudentID WHERE  StudentScholarship.StudentScholarshipID =  { id } ";

            var studentScholarships = db.Database.SqlQuery<StudentScholarship>(query).ToList();
            var result = (from a in studentScholarships
                          join b in db.Students on a.StudentID equals b.StudentID                         
                          select new EnrollStudent
                          {
                              ScholarshipId = (int)a.ScholarshipID,
                              Name = b.FirstName,
                              Email = b.Email,
                              status = a.Status,
                              StudentId = b.StudentID
                          }).ToList();
            return View(result);
        }

        // GET: StudentScholarships/Edit/5
        public ActionResult Edit(int? stuid, int?schid)
        {
            if (stuid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(stuid);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }

        // POST: StudentScholarships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentScholarshipID,ScholarshipID,StudentID,Status")] StudentScholarship studentScholarship, int stuid, int schid)
        {
            int student_id = Convert.ToInt32(Session["user_id"]);
            studentScholarship.StudentID = stuid;
            studentScholarship.ScholarshipID = schid;
            //studentScholarship.Status = "Waiting";
            if (ModelState.IsValid)
            {
                db.Entry(studentScholarship).State = EntityState.Modified;
                db.SaveChanges();
                return View();
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }

        // GET: StudentScholarships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            return View(studentScholarship);
        }

        // POST: StudentScholarships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            db.StudentScholarships.Remove(studentScholarship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
