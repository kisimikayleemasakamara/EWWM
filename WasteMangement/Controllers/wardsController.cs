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
    public class wardsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: wards
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from x in db.wards
                         join a in db.constituencies on x.constituenciesId  equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             constituencyId = a.constituenciesId,
                             constitunecyName = a.name,
                             wardsDescription = x.description,
                             wardName = x.name,
                             wardId = x.wardId

                         }).ToList();
            List<ward> wards = new List<ward>();
            foreach (var item in query)
            {
                wards.Add(new ward()
                {
                    constituenciesId = item.constituencyId,
                    constitunecyName = item.constitunecyName,
                    description = item.wardsDescription,
                    name = item.wardName,
                    wardId = item.wardId
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
            return View(wards);
            
        }

        // POST: wards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "wardId,constituenciesId,name,description,isDeleted")] ward ward)
        {
            if (ModelState.IsValid)
            {
                ward.isDeleted = 0;
                db.wards.Add(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: wards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "wardId,constituenciesId,name,description,isDeleted")] ward ward)
        {
            if (ModelState.IsValid)
            {
                ward.isDeleted = 0;
                db.Entry(ward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: wards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ward ward = db.wards.Find(id);
            ward.isDeleted = 1;
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
