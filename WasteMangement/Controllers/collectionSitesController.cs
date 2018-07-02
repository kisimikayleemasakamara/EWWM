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
    public class collectionSitesController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: collectionSites
        public ActionResult Index()
        {
            var collectionSites = db.collectionSites.Include(c => c.community);
            return View(collectionSites.ToList());
        }

        // GET: collectionSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collectionSite collectionSite = db.collectionSites.Find(id);
            if (collectionSite == null)
            {
                return HttpNotFound();
            }
            return View(collectionSite);
        }

        // GET: collectionSites/Create
        public ActionResult Create()
        {
            ViewBag.communitiesId = new SelectList(db.communities, "communitiesId", "name");
            return View();
        }

        // POST: collectionSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "collectionSiteId,communitiesId,collectionSiteManager,collectionSiteName,collectionSiteNumber,collectionSiteDescription,isDeleted")] collectionSite collectionSite)
        {
            if (ModelState.IsValid)
            {
                db.collectionSites.Add(collectionSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.communitiesId = new SelectList(db.communities, "communitiesId", "name", collectionSite.communitiesId);
            return View(collectionSite);
        }

        // GET: collectionSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collectionSite collectionSite = db.collectionSites.Find(id);
            if (collectionSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.communitiesId = new SelectList(db.communities, "communitiesId", "name", collectionSite.communitiesId);
            return View(collectionSite);
        }

        // POST: collectionSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "collectionSiteId,communitiesId,collectionSiteManager,collectionSiteName,collectionSiteNumber,collectionSiteDescription,isDeleted")] collectionSite collectionSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collectionSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.communitiesId = new SelectList(db.communities, "communitiesId", "name", collectionSite.communitiesId);
            return View(collectionSite);
        }

        // GET: collectionSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collectionSite collectionSite = db.collectionSites.Find(id);
            if (collectionSite == null)
            {
                return HttpNotFound();
            }
            return View(collectionSite);
        }

        // POST: collectionSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            collectionSite collectionSite = db.collectionSites.Find(id);
            db.collectionSites.Remove(collectionSite);
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
