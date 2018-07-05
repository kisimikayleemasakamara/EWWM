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
    public class YouthGroupsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();

        // GET: YouthGroups
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var query = (from a in db.YouthGroups
                         join b in db.districts on a.districtsId equals b.districtsId
                         join c in db.districtAdmins on b.districtAdminId equals c.districtAdminId
                         where b.isDeleted == 0 && a.isDeleted == 0
                         && c.isDeleted == 0 && c.UserId == userID
                         select new
                         {
                             groupName = a.youthGroupName,
                             groupId = a.youthGroupTypeId,
                             description = a.youthGroupDescription,
                             districtName = b.name,
                             districtId = b.districtsId

                         }).ToList();
            List<YouthGroup> groups = new List<YouthGroup>();
            foreach (var item in query)
            {
                groups.Add(new YouthGroup()
                {
                    youthGroupName = item.groupName,
                    youthGroupTypeId = item.groupId,
                    youthGroupDescription = item.description,
                    districtName = item.districtName,
                    districtsId = item.districtId
                });
            }
            return View(groups);
        }

        // POST: YouthGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Create([Bind(Include = "youthGroupTypeId,youthGroupName,youthGroupDescription,districtsId")] YouthGroup youthGroup)
        {
            if (ModelState.IsValid)
            {
                youthGroup.isDeleted = 0;
                db.YouthGroups.Add(youthGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // POST: YouthGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult Edit([Bind(Include = "youthGroupTypeId,youthGroupName,youthGroupDescription,districtsId")] YouthGroup youthGroup)
        {
            if (ModelState.IsValid)
            {
                youthGroup.isDeleted = 0;
                db.Entry(youthGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        // POST: YouthGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            YouthGroup youthGroup = db.YouthGroups.Find(id);
            youthGroup.isDeleted = 1;
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
