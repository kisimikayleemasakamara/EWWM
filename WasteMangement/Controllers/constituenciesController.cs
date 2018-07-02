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
    public class constituenciesController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: constituencies
        public ActionResult Index()
        {
            var constituencies = db.constituencies.Include(c => c.district);
            return View(constituencies.ToList());
        }

        // GET: constituencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            constituency constituency = db.constituencies.Find(id);
            if (constituency == null)
            {
                return HttpNotFound();
            }
            return View(constituency);
        }

        // GET: constituencies/Create
        public ActionResult Create()
        {
            ViewBag.districtsId = new SelectList(db.districts, "districtsId", "name");
            return View();
        }

        // POST: constituencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "constituenciesId,districtsId,name,description,isDeleted")] constituency constituency)
        {
            if (ModelState.IsValid)
            {
                db.constituencies.Add(constituency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.districtsId = new SelectList(db.districts, "districtsId", "name", constituency.districtsId);
            return View(constituency);
        }

        // GET: constituencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            constituency constituency = db.constituencies.Find(id);
            if (constituency == null)
            {
                return HttpNotFound();
            }
            ViewBag.districtsId = new SelectList(db.districts, "districtsId", "name", constituency.districtsId);
            return View(constituency);
        }

        // POST: constituencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "constituenciesId,districtsId,name,description,isDeleted")] constituency constituency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(constituency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.districtsId = new SelectList(db.districts, "districtsId", "name", constituency.districtsId);
            return View(constituency);
        }

        // GET: constituencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            constituency constituency = db.constituencies.Find(id);
            if (constituency == null)
            {
                return HttpNotFound();
            }
            return View(constituency);
        }

        // POST: constituencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            constituency constituency = db.constituencies.Find(id);
            db.constituencies.Remove(constituency);
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
