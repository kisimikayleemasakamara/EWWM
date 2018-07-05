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
    public class publicWasteBinsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: publicWasteBins
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from w in db.publicWasteBins
                         join i in db.collectionSites on w.collectionSiteId equals i.collectionSiteId
                         join z in db.communities on i.communitiesId equals z.communitiesId
                         join y in db.sections on z.sectionId equals y.sectionId
                         join x in db.wards on y.wardId equals x.wardId
                         join a in db.constituencies on x.constituenciesId equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where w.isDeleted == 0 && i.isDeleted == 0 && z.isDeleted == 0 &&
                         y.isDeleted == 0 && x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             siteName = i.collectionSiteName,
                             siteId = i.collectionSiteId,
                             binId = w.publicWasteBinId,
                             binName = w.publicWasteBinName,
                             binDescription = w.publicWasteBinDescription
                         }).ToList();
            List<publicWasteBin> publicWasteBins = new List<publicWasteBin>();
            foreach (var item in query)
            {
                publicWasteBins.Add(new publicWasteBin()
                {
                    collectionSiteId = item.siteId,
                    siteName = item.siteName,
                    publicWasteBinId = item.binId,
                    publicWasteBinName = item.binName,
                    publicWasteBinDescription = item.binDescription
                });
            }
            var query1 = (from i in db.collectionSites
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
                              siteName = i.collectionSiteName,
                              siteId = i.collectionSiteId,
                          }).ToList();
            List<collectionSite> sites = new List<collectionSite>();
            foreach (var item in query1)
            {
                sites.Add(new collectionSite()
                {
                    collectionSiteId = item.siteId,
                    collectionSiteName = item.siteName
                });
            }
            ViewBag.sites = sites;
            return View(publicWasteBins);
        }

        // POST: publicWasteBins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "publicWasteBinId,publicWasteBinName,publicWasteBinDescription,collectionSiteId,isDeleted")] publicWasteBin publicWasteBin)
        {
            if (ModelState.IsValid)
            {
                publicWasteBin.isDeleted = 0;
                db.publicWasteBins.Add(publicWasteBin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: publicWasteBins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "publicWasteBinId,publicWasteBinName,publicWasteBinDescription,collectionSiteId,isDeleted")] publicWasteBin publicWasteBin)
        {
            if (ModelState.IsValid)
            {
                publicWasteBin.isDeleted = 0;
                db.Entry(publicWasteBin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: publicWasteBins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            publicWasteBin publicWasteBin = db.publicWasteBins.Find(id);
            publicWasteBin.isDeleted = 1;
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
