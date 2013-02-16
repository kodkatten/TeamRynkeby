using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventBooking.Controllers
{
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/

        public ActionResult Create()
        {
            return View();
        }


        //
        // POST: /Questions/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
    }
}
