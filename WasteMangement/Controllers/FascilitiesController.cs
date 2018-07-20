using Microsoft.AspNet.Identity;
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
    public class FascilitiesController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: Fascilities
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from a in db.Fascilities
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             Fascility_Id = a.Fascility_Id,
                             Fascility_Name = a.Fascility_Name,
                             Fascility_Description = a.Fascility_Description,
                             WasteCollectionPricePerMonth = a.WasteCollectionPricePerMonth,
                             districtName = b.name,
                             districtId = b.districtsId

                         }).ToList();
            List<Fascility> facilities = new List<Fascility>();
            foreach (var item in query)
            {
                facilities.Add(new Fascility()
                {
                    Fascility_Id = item.Fascility_Id,
                    Fascility_Name = item.Fascility_Name,
                    Fascility_Description = item.Fascility_Description,
                    WasteCollectionPricePerMonth = item.WasteCollectionPricePerMonth,
                    districtName = item.districtName,
                    districtsId = item.districtId
                });
            }
            return View(facilities);
        }

        // POST: Fascilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fascility_Id,Fascility_Name,Fascility_Description,WasteCollectionPricePerMonth,districtsId")] Fascility fascility)
        {
            if (ModelState.IsValid)
            {
                fascility.isDeleted = 0;
                db.Fascilities.Add(fascility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: Fascilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fascility fascility = db.Fascilities.Find(id);
            if (fascility == null)
            {
                return HttpNotFound();
            }
            string value = JsonConvert.SerializeObject(fascility, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: Fascilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Fascility_Id,Fascility_Name,Fascility_Description,WasteCollectionPricePerMonth,districtsId")] Fascility fascility)
        {
            if (ModelState.IsValid)
            {
                fascility.isDeleted = 0;
                db.Entry(fascility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        // POST: Fascilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fascility fascility = db.Fascilities.Find(id);
            fascility.isDeleted = 1;
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
