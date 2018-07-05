using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "DistrictAdmin")]
    public class collectionSitesController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: collectionSites
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from i in db.collectionSites
                         join z in db.communities on i.communitiesId equals z.communitiesId
                         join y in db.sections on z.sectionId equals y.sectionId
                         join x in db.wards on y.wardId equals x.wardId
                         join a in db.constituencies on x.constituenciesId equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where i.isDeleted == 0 && z.isDeleted == 0 &&
                         y.isDeleted == 0 && x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             communityID = z.communitiesId,
                             communityName = z.name,
                             siteName = i.collectionSiteName,
                             siteId = i.collectionSiteId,
                             siteDescription = i.collectionSiteDescription,
                             manager = i.collectionSiteManager,
                             Number = i.collectionSiteNumber
                         }).ToList();
            List<collectionSite> collectionSites = new List<collectionSite>();
            foreach (var item in query)
            {
                collectionSites.Add(new collectionSite()
                {
                    communitiesId = item.communityID,
                    communityName = item.communityName,
                    collectionSiteName = item.siteName,
                    collectionSiteManager = item.manager,
                    collectionSiteDescription = item.siteDescription,
                    collectionSiteNumber = item.Number,
                    collectionSiteId = item.siteId
                });
            }
            var query1 = (from z in db.communities
                         join y in db.sections on z.sectionId equals y.sectionId
                         join x in db.wards on y.wardId equals x.wardId
                         join a in db.constituencies on x.constituenciesId equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where z.isDeleted == 0 &&
                         y.isDeleted == 0 && x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             communityID = z.communitiesId,
                             communityName = z.name,
                         }).ToList();
            List<community> communities = new List<community>();
            foreach (var item in query1)
            {
                communities.Add(new community()
                {
                    communitiesId = item.communityID,
                    name = item.communityName
                });
            }
            ViewBag.communities = communities;
            return View(collectionSites);
        }

        // POST: collectionSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "collectionSiteId,communitiesId,collectionSiteManager,collectionSiteName,collectionSiteNumber,collectionSiteDescription,isDeleted")] collectionSite collectionSite)
        {
            if (ModelState.IsValid)
            {
                collectionSite.isDeleted = 0;
                db.collectionSites.Add(collectionSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // POST: collectionSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "collectionSiteId,communitiesId,collectionSiteManager,collectionSiteName,collectionSiteNumber,collectionSiteDescription,isDeleted")] collectionSite collectionSite)
        {
            if (ModelState.IsValid)
            {
                collectionSite.isDeleted = 0;
                db.Entry(collectionSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: collectionSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            collectionSite collectionSite = db.collectionSites.Find(id);
            collectionSite.isDeleted = 1;
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
