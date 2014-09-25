using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoadTrip.API.Models;

namespace RoadTrip.API.Controllers
{
    public class AddNewClientController : Controller
    {
        

        // GET: AddNewClient
        public ActionResult Index()
        {
            var model = new NewClientModel();
            model.Secret = Helper.GetHash("managemyroadtripmobil@def.com");
            return View(model);
        }

        [HttpPost]

        public JsonResult SaveNewClient(NewClientModel model)
        {

            return null;
        }
    }
}