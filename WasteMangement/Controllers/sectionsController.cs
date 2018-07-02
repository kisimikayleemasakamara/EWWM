using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WasteMangement.Models;

namespace WasteMangement.Controllers
{
    public class sectionsController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: sections
        public ActionResult Index()
        {
            var sections = db.sections.Include(s => s.ward);
            return View(sections.ToList());
        }

        // GET: sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: sections/Create
        public ActionResult Create()
        {
            ViewBag.wardId = new SelectList(db.wards, "wardId", "name");
            return View();
        }

        // POST: sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sectionId,wardId,name,description,isDeleted")] section section)
        {
            if (ModelState.IsValid)
            {
                db.sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.wardId = new SelectList(db.wards, "wardId", "name", section.wardId);
            return View(section);
        }

        // GET: sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.wardId = new SelectList(db.wards, "wardId", "name", section.wardId);
            return View(section);
        }

        // POST: sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sectionId,wardId,name,description,isDeleted")] section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.wardId = new SelectList(db.wards, "wardId", "name", section.wardId);
            return View(section);
        }

        // GET: sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section section = db.sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            section section = db.sections.Find(id);
            db.sections.Remove(section);
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
