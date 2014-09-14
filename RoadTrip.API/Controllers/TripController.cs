using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web.Http;
using RoadTrip.API.Entities;
using RoadTrip.API.Models;

namespace RoadTrip.API.Controllers
{
    [RoutePrefix("api/Trips")]
    public class TripController : ApiController
    {
        private AuthContext _ctx { get; set; }

        public TripController()
        {
            _ctx = new AuthContext();
            
        }
        [Authorize]
        [HttpPost]
        [Route("NewRoadTrip")]
        public Trip CreateNewRoadTrip(RoadTripModel model)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);
            var newRoadTrip = new Trip {Name = model.Name, OwnerId = ownerPerson.Id};
            _ctx.Trips.Add(newRoadTrip);
            return _ctx.SaveChanges() > 0 ? newRoadTrip : null;

        }

        [Authorize]
        [HttpGet]
        [Route("TripsIOwn")]
        public List<Trip> GetTripsIOwn()
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);

            var d = _ctx.Trips.Where(x => x.OwnerId == ownerPerson.Id).ToList();
            return d;
        } 
    }
}
