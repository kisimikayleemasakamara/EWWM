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
    [Authorize(Roles = "SystemAdmin")]
    public class districtsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: districts
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Index()
        {
            var query = (from a in db.districts
                         join b in db.regions on a.regionId equals b.regionId
                         join c in db.districtAdmins on a.districtAdminId equals c.districtAdminId
                         where b.isDeleted == 0
                        && a.isDeleted == 0 && c.isDeleted == 0
                         select new
                         {
                             name = a.name,
                             id = a.districtsId,
                             description = a.description,
                             region_name = b.region_name,
                             username = c.name
                         }).ToList();
            List<district> districts = new List<district>();
            foreach (var item in query)
            {
                districts.Add(new district()
                {
                    regionName = item.region_name,
                    AdminName = item.username,
                    description = item.description,
                    name = item.name,
                    districtsId = item.id
                });
            }
            ViewBag.alert = TempData["alert"];
            ViewBag.name = TempData["name"];
            return View(districts);
        }

        [Authorize(Roles = "SystemAdmin")]
        public ActionResult FillRegion(int state)
        {
            var query = (from g in db.regions
                         join a in db.countries on g.countryId equals a.countryId
                         where g.countryId == state
                         && a.countryId == state
                         && g.isDeleted == 0
                         select new
                         {
                             g.region_name,
                             g.regionId

                         }).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var c in query)
            {
                SelectListItem s = new SelectListItem();
                s.Text = c.region_name;
                s.Value = c.regionId.ToString();
                list.Add(s);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SystemAdmin")]
        public async System.Threading.Tasks.Task<ActionResult> countries()
        {
            var countries = (from d in db.countries
                             where d.isDeleted == 0
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

        [Authorize(Roles = "SystemAdmin")]
        public async System.Threading.Tasks.Task<ActionResult> admins()
        {
            var admins = (from d in db.districtAdmins
                             where d.isDeleted == 0
                             select d).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var c in admins)
            {
                SelectListItem s = new SelectListItem();
                s.Text = c.name;
                s.Value = c.districtAdminId.ToString();
                list.Add(s);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        

        // POST: districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Create([Bind(Include = "districtsId,regionId,districtAdminId,name,description,isDeleted")] district district)
        {
            var exist = (from d in db.districts
                         where d.districtAdminId == district.districtAdminId
                         && d.isDeleted == 0
                         select d).ToList();
            var dname = (from d in db.districtAdmins
                         where d.districtAdminId == district.districtAdminId
                         && d.isDeleted == 0
                         select new
                         {
                             name = d.name
                        }).SingleOrDefault();
            if (exist.Count == 0)
            {
                if (ModelState.IsValid)
                {
                    district.isDeleted = 0;
                    db.districts.Add(district);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["alert"] = false;
                TempData["name"] = dname.name;
            }
          
            return RedirectToAction("Index");
        }

        // GET: districts/Edit/5
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            district district = db.districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            string value = JsonConvert.SerializeObject(district, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult Edit([Bind(Include = "districtsId,regionId,districtAdminId,name,description,isDeleted")] district district)
        {
            var exist = (from d in db.districts
                         where d.districtAdminId == district.districtAdminId
                         && d.isDeleted == 0 && d.districtsId != district.districtsId
                         select d).ToList();
            var dname = (from d in db.districtAdmins
                         where d.districtAdminId == district.districtAdminId
                         && d.isDeleted == 0
                         select new
                         {
                             name = d.name
                         }).SingleOrDefault();
            if(exist.Count == 0)
            {
                if (ModelState.IsValid)
                {
                    district.isDeleted = 0;
                    db.Entry(district).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["alert"] = false;
                TempData["name"] = dname.name;
            }

            return RedirectToAction("Index");
        }

        // POST: districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            district district = db.districts.Find(id);
            district.isDeleted = 1;
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
