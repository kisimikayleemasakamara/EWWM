using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WasteMangement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.heading = "Web Based Solid Waste Management System for Sierra Leone";
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult About()
        {
           //ViewBag.heading = "About The Waste Management";
           ViewBag.Message = "Your application description page.";

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
            ViewBag.wardNumber = wardNumber;
            return View();
        }

        public ActionResult AllCollectionSites()
        {
            return View();
        }
        public ActionResult PublicWasteBins()
        {
            return View();
        }
    }
}