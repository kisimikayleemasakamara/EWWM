using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WasteMangement.Models;

namespace WasteMangement.Controllers
{
    public class HomeController : Controller
    {
        private wwmDbEntities db = new wwmDbEntities();
        public ActionResult Index()
        {
            ViewBag.heading = "Web Based Solid Waste Management System for Sierra Leone";
            var p = Session["page"];
            TempData["dis"] = Session["district"];
            if (p != null)
            {
                if (p.Equals("group"))
                {
                    return RedirectToAction("ViewYouthGroups", "Home");
                }
                if (p.Equals("sites"))
                {
                    return RedirectToAction("CollectionSites");
                }
                if (p.Equals("bins"))
                {
                    return RedirectToAction("PublicWasteBins");
                }
                if (p.Equals("reg"))
                {
                    return RedirectToAction("Register","Home");
                }
            }
           
            return View();
        }

        public ActionResult loadDistricts()
        {
            var district = (from d in db.districts select new
            {
                d.name,
                d.districtsId
            });
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(district);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "DistrictAdmin")]
        public ActionResult DistrictAdmindash()
        {
            return View();
        }
        public ActionResult About()
        {
           //ViewBag.heading = "About The Waste Management";
           ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public ActionResult Check(string state)
        {
            Session["district"] = state;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult page(string state,string v)
        {
            Session["page"] = state;
            Session["district"] = v;
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrashIsMoney()
        {
            //ViewBag.heading = "Trash is Money";
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.fascilities = (from d in db.Fascilities
                                   where d.isDeleted == 0
                                   select d).ToList();
            var query = (from d in db.YouthGroups
                         where d.isDeleted == 0
                         select d).ToList();
            return View();
        }
        public ActionResult ViewYouthGroups()
        {
            
            return View();
        }

        public ActionResult CollectionSites()
        {
            ViewBag.heading = "Web Based Solid Waste Management System for Sierra Leone";
            return View();
        }

        public ActionResult CollectionSiteWard(int wardNumber)
        {
            var query = from w in db.wards
                        join s in db.sections on w.wardId equals s.wardId
                        join c in db.communities on s.sectionId equals c.sectionId
                        join col in db.collectionSites on c.communitiesId equals col.communitiesId
                        where w.wardId == wardNumber && w.isDeleted == 0
                        && s.isDeleted == 0 && c.isDeleted == 0 
                        && col.isDeleted == 0
                        select col;
            List<collectionSite> collectionSites = new List<collectionSite>();
            foreach (var item in query)
            {
                collectionSites.Add(new collectionSite()
                {
                    collectionSiteName = item.collectionSiteName,
                    collectionSiteManager = item.collectionSiteManager,
                    collectionSiteDescription = item.collectionSiteDescription,
                    collectionSiteNumber = item.collectionSiteNumber,
                    startDay = item.startDay,
                    startDayTimefrom = item.startDayTimefrom,
                    startDayTimetill = item.startDayTimetill,
                    tilDay = item.tilDay,
                    tillDayTimefrom = item.tillDayTimefrom,
                    tillDayTimetill = item.tillDayTimetill,
                    sstime = item.sstime,
                    sltime = item.sltime,
                    tltime = item.tltime,
                    tstime = item.tstime
                });
            }
            ViewBag.wardNumber = wardNumber;
            return View(collectionSites);
        }

        public ActionResult AllCollectionSites()
        {
            string district = Session["district"].ToString();
            var query = from d in db.districts
                        join con in db.constituencies on d.districtsId equals con.districtsId
                        join w in db.wards on con.constituenciesId equals w.constituenciesId
                        join s in db.sections on w.wardId equals s.wardId
                        join c in db.communities on s.sectionId equals c.sectionId
                        join col in db.collectionSites on c.communitiesId equals col.communitiesId
                        where w.isDeleted == 0 && d.isDeleted == 0 && con.isDeleted == 0
                        && s.isDeleted == 0 && c.isDeleted == 0
                        && col.isDeleted == 0 && d.name == district
                        select new
                        {
                            col.collectionSiteName,
                            col.collectionSiteNumber,
                            col.collectionSiteManager,
                            col.collectionSiteDescription,
                            col.sltime,
                            col.sstime,
                            col.startDay,
                            col.startDayTimefrom,
                            col.startDayTimetill,
                            col.tilDay,
                            col.tillDayTimefrom,
                            col.tillDayTimetill,
                            col.tstime,
                            col.tltime,
                            name = w.name
                        };
            var query1 = (from d in db.districts
                          join con in db.constituencies on d.districtsId equals con.districtsId
                          join w in db.wards on con.constituenciesId equals w.constituenciesId
                          join s in db.sections on w.wardId equals s.wardId
                          join c in db.communities on s.sectionId equals c.sectionId
                          join col in db.collectionSites on c.communitiesId equals col.communitiesId
                          where w.isDeleted == 0 && d.isDeleted == 0 && con.isDeleted == 0
                          && s.isDeleted == 0 && c.isDeleted == 0
                          && col.isDeleted == 0 && d.name == district
                          select new
                          {
                              name = w.name
                          });
            List<collectionSite> collectionSites = new List<collectionSite>();
            foreach (var item in query)
            {
                collectionSites.Add(new collectionSite()
                {
                    collectionSiteName = item.collectionSiteName,
                    collectionSiteManager = item.collectionSiteManager,
                    collectionSiteDescription = item.collectionSiteDescription,
                    collectionSiteNumber = item.collectionSiteNumber,
                    startDay = item.startDay,
                    startDayTimefrom = item.startDayTimefrom,
                    startDayTimetill = item.startDayTimetill,
                    tilDay = item.tilDay,
                    tillDayTimefrom = item.tillDayTimefrom,
                    tillDayTimetill = item.tillDayTimetill,
                    sstime = item.sstime,
                    sltime = item.sltime,
                    tltime = item.tltime,
                    tstime = item.tstime,
                    wardName = item.name
                });
            }
            List<ward> wards = new List<ward>();
            foreach (var item in query1)
            {
                wards.Add(new ward()
                {
                   name = item.name
                });
            }
            ViewBag.wards = wards;
            return View(collectionSites);
        }
        public ActionResult PublicWasteBins()
        {
            return View();
        }
    }
}