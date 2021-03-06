﻿using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
    public class constituenciesController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: constituencies
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from a in db.constituencies
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where b.isDeleted == 0 && a.isDeleted == 0 
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                            constituencyId = a.constituenciesId,
                            constitunecyName = a.name,
                            constituencydescription = a.description,
                            districtName = b.name,
                            districtId = b.districtsId
                             
                         }).ToList();
            List<constituency> constituencies = new List<constituency>();
            foreach (var item in query)
            {
                constituencies.Add(new constituency()
                {
                    constituenciesId = item.constituencyId,
                    name = item.constitunecyName,
                    description = item.constituencydescription,
                    districtName = item.districtName,
                    districtsId = item.districtId
                });
            }
            return View(constituencies);
        }

        // POST: constituencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "constituenciesId,districtsId,name,description,isDeleted")] constituency constituency)
        {
            if (ModelState.IsValid)
            {
                constituency.isDeleted = 0;
                db.constituencies.Add(constituency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: constituencies/Edit/5
        [Authorize(Roles = "DistrictAdmin")]
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
            string value = JsonConvert.SerializeObject(constituency, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: constituencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "constituenciesId,districtsId,name,description,isDeleted")] constituency constituency)
        {
            if (ModelState.IsValid)
            {
                constituency.isDeleted = 0;
                db.Entry(constituency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        // POST: constituencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            constituency constituency = db.constituencies.Find(id);
            constituency.isDeleted = 1;
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
