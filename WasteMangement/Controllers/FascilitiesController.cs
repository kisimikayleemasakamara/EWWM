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
            return View((from d in db.Fascilities
                         where d.isDeleted == 0
                         select d).ToList());
        }

        // POST: Fascilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fascility_Id,Fascility_Name,Fascility_Description,WasteCollectionPricePerMonth")] Fascility fascility)
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
        public ActionResult Edit([Bind(Include = "Fascility_Id,Fascility_Name,Fascility_Description,WasteCollectionPricePerMonth")] Fascility fascility)
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
