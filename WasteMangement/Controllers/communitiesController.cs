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
    public class communitiesController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: communities
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from z in db.communities
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
                             sectionName = y.name,
                             communityName = z.name,
                             communityDescription = z.description,
                             sectionId = y.sectionId
                         }).ToList();
            List<community> communities = new List<community>();
            foreach (var item in query)
            {
                communities.Add(new community()
                {
                    communitiesId = item.communityID,
                    sectionName = item.sectionName,
                    description = item.communityDescription,
                    name = item.communityName,
                    sectionId = item.sectionId
                });
            }
            var q = (from a in db.constituencies
                     join b in db.districts on a.districtsId equals b.districtsId
                     join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                     where b.isDeleted == 0 && a.isDeleted == 0
                     && c.isDeleted == 0 && c.UserId == userID
                     select new
                     {
                         constituencyId = a.constituenciesId,
                         constitunecyName = a.name
                     }).ToList();
            List<ConstituencyDropdown> constituencies = new List<ConstituencyDropdown>();
            foreach (var item in q)
            {
                constituencies.Add(new ConstituencyDropdown()
                {
                    constituencyId = item.constituencyId,
                    constitunecyName = item.constitunecyName
                });
            }
            ViewBag.constituecy = constituencies;
            return View(communities);
        }

        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult LoadWardList(int state)
        {
            string userID = User.Identity.GetUserId();
            var query = (from x in db.wards
                         join a in db.constituencies on x.constituenciesId equals state
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && a.constituenciesId == state && c.UserId == userID
                         select new
                         {
                             wardName = x.name,
                             wardId = x.wardId

                         }).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var c in query)
            {
                SelectListItem s = new SelectListItem();
                s.Text = c.wardName;
                s.Value = c.wardId.ToString();
                list.Add(s);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult LoadSectionList(int state)
        {
            string userID = User.Identity.GetUserId();
            var query = (from y in db.sections
                         join x in db.wards on y.wardId equals state
                         join a in db.constituencies on x.constituenciesId equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where y.isDeleted == 0 && x.isDeleted == 0 && a.isDeleted == 0 && b.isDeleted == 0
                         && c.isDeleted == 0 && x.wardId == state && c.UserId == userID
                         
                         select new
                         {
                             sectionName = y.name,
                             sectionId = y.sectionId

                         }).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var c in query)
            {
                SelectListItem s = new SelectListItem();
                s.Text = c.sectionName;
                s.Value = c.sectionId.ToString();
                list.Add(s);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // POST: communities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "communitiesId,sectionId,name,description,isDeleted")] community community)
        {
            if (ModelState.IsValid)
            {
                community.isDeleted = 0;
                db.communities.Add(community);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: communities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "communitiesId,sectionId,name,description,isDeleted")] community community)
        {
            if (ModelState.IsValid)
            {
                community.isDeleted = 0;
                db.Entry(community).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            community community = db.communities.Find(id);
            community.isDeleted = 1;
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
