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
    public class sectionsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: sections
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from y in db.sections 
                         join x in db.wards on y.wardId equals x.wardId
                         join a in db.constituencies on x.constituenciesId equals a.constituenciesId
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where y.isDeleted == 0 && x.isDeleted == 0 && b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             wardId = x.wardId,
                             sectionName = y.name,
                             sectionDescription = y.description,
                             wardName = x.name,
                             sectionId = y.sectionId
                         }).ToList();
            List<section> sections = new List<section>();
            foreach (var item in query)
            {
                sections.Add(new section()
                {
                    wardId = item.wardId,
                    wardName = item.wardName,
                    description = item.sectionDescription,
                    name = item.sectionName,
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
            return View(sections);
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

        // POST: sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "sectionId,wardId,name,description,isDeleted")] section section)
        {
            if (ModelState.IsValid)
            {
                section.isDeleted = 0;
                db.sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        // POST: sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "sectionId,wardId,name,description,isDeleted")] section section)
        {
            if (ModelState.IsValid)
            {
                section.isDeleted = 0;
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        // POST: sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            section section = db.sections.Find(id);
            section.isDeleted = 1;
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
