using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
    public class districtAdminsController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: districtAdmins
        public ActionResult Index()
        {
            var query = (from a in db.districtAdmins
                         join b in db.AspNetUsers on a.UserId equals b.Id
                         where b.isDeleted == 0
                        && a.isDeleted == 0
                         select new
                         {
                             districtAdminId = a.districtAdminId,
                             name = a.name,
                             description = a.description,
                             Email = b.Email,
                             PhoneNo = b.PhoneNo
                         }).ToList();
            List<districtAdmin> districtAdmins = new List<districtAdmin>();
            foreach (var item in query)
            {
                districtAdmins.Add(new districtAdmin()
                {
                    districtAdminId = item.districtAdminId,
                    name = item.name,
                    description = item.description,
                    Email = item.Email,
                    PhoneNo = item.PhoneNo
                });
            }
            return View(districtAdmins);
        }

        // GET: districtAdmins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var districtAdmin = (from a in db.districtAdmins
                         join b in db.AspNetUsers on a.UserId equals b.Id
                         where b.isDeleted == 0
                        && a.isDeleted == 0 && a.districtAdminId == id
                         select new
                         {
                             districtAdminId = a.districtAdminId,
                             userId = b.Id,
                             FirstName = b.FirstName,
                             LastName = b.LastName,
                             Address = b.Address,
                             description = a.description,
                             Email = b.Email,
                             PhoneNo = b.PhoneNo,
                             userName = b.UserName
                         }).SingleOrDefault();
            if (districtAdmin == null)
            {
                return HttpNotFound();
            }

            string value = JsonConvert.SerializeObject(districtAdmin, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: districtAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "districtAdminId,name,description,UserId,isDeleted")] districtAdmin districtAdmin, RegisterModel register, string Id)
        {
            if (ModelState.IsValid)
            {
                register.isDeleted = 0;
                var userStore = new UserStore<ApplicationUser>(new
                                  ApplicationDbContext());
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await UserManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.FirstName = register.FirstName;
                user.LastName = register.LastName;
                user.Email = register.Email;
                user.PhoneNo = register.PhoneNo;
                user.isDeleted = 0;
                user.Address = register.Address;
                var removePassword = UserManager.RemovePassword(Id);
                if (removePassword.Succeeded)
                {
                    //Removed Password Success
                    var AddPassword = UserManager.AddPassword(Id, register.Password);
                    if (AddPassword.Succeeded)
                    {
                        await UserManager.UpdateAsync(user);
                        var ctx = userStore.Context;
                        ctx.SaveChanges();
                        districtAdmin.name = register.FirstName + " " + register.LastName;
                        districtAdmin.UserId = Id;
                        db.Entry(districtAdmin).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
               
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: districtAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> DeleteConfirmed(int id)
        {
            var userStore = new UserStore<ApplicationUser>(new
                                 ApplicationDbContext());
            districtAdmin districtAdmin = db.districtAdmins.Find(id);
            districtAdmin.isDeleted = 1;
            var user = await UserManager.FindByIdAsync(districtAdmin.UserId);
            user.isDeleted = 1;
            await UserManager.UpdateAsync(user);
            var ctx = userStore.Context;
            ctx.SaveChanges();
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
