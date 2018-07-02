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
    public class publicWasteBinsController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: publicWasteBins
        public ActionResult Index()
        {
            var publicWasteBins = db.publicWasteBins.Include(p => p.collectionSite);
            return View(publicWasteBins.ToList());
        }

        // GET: publicWasteBins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicWasteBin publicWasteBin = db.publicWasteBins.Find(id);
            if (publicWasteBin == null)
            {
                return HttpNotFound();
            }
            return View(publicWasteBin);
        }

        // GET: publicWasteBins/Create
        public ActionResult Create()
        {
            ViewBag.collectionSiteId = new SelectList(db.collectionSites, "collectionSiteId", "collectionSiteManager");
            return View();
        }

        // POST: publicWasteBins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "publicWasteBinId,publicWasteBinName,publicWasteBinDescription,collectionSiteId,isDeleted")] publicWasteBin publicWasteBin)
        {
            if (ModelState.IsValid)
            {
                db.publicWasteBins.Add(publicWasteBin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.collectionSiteId = new SelectList(db.collectionSites, "collectionSiteId", "collectionSiteManager", publicWasteBin.collectionSiteId);
            return View(publicWasteBin);
        }

        // GET: publicWasteBins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicWasteBin publicWasteBin = db.publicWasteBins.Find(id);
            if (publicWasteBin == null)
            {
                return HttpNotFound();
            }
            ViewBag.collectionSiteId = new SelectList(db.collectionSites, "collectionSiteId", "collectionSiteManager", publicWasteBin.collectionSiteId);
            return View(publicWasteBin);
        }

        // POST: publicWasteBins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "publicWasteBinId,publicWasteBinName,publicWasteBinDescription,collectionSiteId,isDeleted")] publicWasteBin publicWasteBin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicWasteBin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.collectionSiteId = new SelectList(db.collectionSites, "collectionSiteId", "collectionSiteManager", publicWasteBin.collectionSiteId);
            return View(publicWasteBin);
        }

        // GET: publicWasteBins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicWasteBin publicWasteBin = db.publicWasteBins.Find(id);
            if (publicWasteBin == null)
            {
                return HttpNotFound();
            }
            return View(publicWasteBin);
        }

        // POST: publicWasteBins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            publicWasteBin publicWasteBin = db.publicWasteBins.Find(id);
            db.publicWasteBins.Remove(publicWasteBin);
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
