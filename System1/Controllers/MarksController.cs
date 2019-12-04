using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System1.Models;

namespace System1.Controllers
{
    [Authorize]
    public class MarksController : Controller
    {
        private DbEntities db = new DbEntities();
        // GET: Marks
        public ActionResult Index()
        {
            var marks = db.Marks.ToList();
            var students = db.Students.ToList();

            var multiplet = from m in marks
                            join st in students on m.StudentId equals st.StudentId
                            select new MultipleTable
                            {
                                marksdetails=m,
                                studentdetails=st
                            };
            //db.Marks.ToList()

            return View(multiplet);
        }

        public ActionResult Create()
        {
            var list = db.Students.ToList();
            SelectList selist = new SelectList(list,"StudentId","AdmNo");
            ViewBag.listname = selist;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,Maths,English")] Marks mark)
        {
            if (ModelState.IsValid)
            {
                db.Marks.Add(mark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mark);
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,Maths,English")] Marks marks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marks);
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}