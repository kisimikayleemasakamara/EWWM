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
    public class youthGroupsController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: youthGroups
        public ActionResult Index()
        {
            var youthGroups = db.youthGroups.Include(y => y.AspNetUser);
            return View(youthGroups.ToList());
        }

        // GET: youthGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            youthGroup youthGroup = db.youthGroups.Find(id);
            if (youthGroup == null)
            {
                return HttpNotFound();
            }
            return View(youthGroup);
        }

        // GET: youthGroups/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName");
            return View();
        }

        // POST: youthGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "youthGroupId,districsId,youthGroupName,youthGroupDescription,isApproved,address,UserId,isDeleted")] youthGroup youthGroup)
        {
            if (ModelState.IsValid)
            {
                db.youthGroups.Add(youthGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", youthGroup.UserId);
            return View(youthGroup);
        }

        // GET: youthGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            youthGroup youthGroup = db.youthGroups.Find(id);
            if (youthGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", youthGroup.UserId);
            return View(youthGroup);
        }

        // POST: youthGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "youthGroupId,districsId,youthGroupName,youthGroupDescription,isApproved,address,UserId,isDeleted")] youthGroup youthGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(youthGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", youthGroup.UserId);
            return View(youthGroup);
        }

        // GET: youthGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            youthGroup youthGroup = db.youthGroups.Find(id);
            if (youthGroup == null)
            {
                return HttpNotFound();
            }
            return View(youthGroup);
        }

        // POST: youthGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            youthGroup youthGroup = db.youthGroups.Find(id);
            db.youthGroups.Remove(youthGroup);
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
