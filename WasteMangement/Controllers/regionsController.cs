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
    public class regionsController : Controller
    {
        private wwmDbEntities1 db = new wwmDbEntities1();

        // GET: regions
        public ActionResult Index()
        {
            var query = (from a in db.regions
                          join b in db.countries on a.countryId equals b.countryId
                          where b.isDeleted == 0
                         && a.isDeleted == 0
                          select new
                          {
                              regionId = a.regionId,
                              region_name = a.region_name,
                              countryId = a.countryId,
                              description = a.description,
                              name = b.name
                          }).ToList();
            List<region> regions = new List<region>();
            foreach (var item in query)
            {
                regions.Add(new region()
                {
                    regionId = item.regionId,
                    region_name = item.region_name,
                    description = item.description,
                    countryId = item.countryId,
                    name = item.name
                });
            }
            return View(regions);
        }

        public async System.Threading.Tasks.Task<ActionResult> Countries()
        {
            var countries = (from d in db.countries where d.isDeleted == 0
            select d).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var c in countries)
            {
                SelectListItem s = new SelectListItem();
                s.Text = c.name;
                s.Value = c.countryId.ToString();
                list.Add(s);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: regions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            region region = db.regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "regionId,countryId,region_name,description,isDeleted")] region region)
        {
            if (ModelState.IsValid)
            {
                region.isDeleted = 0;
                db.regions.Add(region);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        // GET: regions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            region region = db.regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            string value = JsonConvert.SerializeObject(region, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "regionId,countryId,region_name,description,isDeleted")] region region)
        {
            if (ModelState.IsValid)
            {
                region.isDeleted = 0;
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        // POST: regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            region region = db.regions.Find(id);
            region.isDeleted = 1;
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
