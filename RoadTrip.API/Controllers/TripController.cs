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

        public TripController(AuthContext context)
        {
            _ctx = context;

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
            
            _ctx.SaveChanges();
            newRoadTrip.Hash = String.Format("{0}{1}", username.Substring(0, 2).ToUpper(), newRoadTrip.Id);
            var tripMap = new TripUserMap {IsOwner = true, PersonId = ownerPerson.Id, TripId = newRoadTrip.Id};
            _ctx.TripUserMaps.Add(tripMap);
            return _ctx.SaveChanges() > 0 ? newRoadTrip : null;

        }

        [Authorize]
        [HttpGet]
        [Route("TripsIOwn")]
        public List<Trip> GetTripsIOwn()
        {

            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);
            return
                _ctx.TripUserMaps.Include("Trip").Where(x => x.PersonId == ownerPerson.Id).Select(x => x.Trip).ToList();
            //var d = _ctx.Trips.Where(x => x.OwnerId == ownerPerson.Id).ToList();
            //return d;
        }

        [Authorize]
        [HttpGet]
        [Route("TripDetails/{roadTripHash}")]
        public TripDetailsModel GetTripDetails(string roadTripHash)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);

            var referencedRoadTrip = _ctx.Trips.Include("Owner").FirstOrDefault(x => x.Hash == roadTripHash.ToUpper()
                                                                    && x.UserMap.Any(g => g.PersonId == ownerPerson.Id));
            if (referencedRoadTrip != null)
            {
                var ret = new TripDetailsModel
                {
                    RoadTripName = referencedRoadTrip.Name,
                    RoadTripHash = referencedRoadTrip.Hash,
                    RoadTripId = referencedRoadTrip.Id,
                    OwnerFirstName = referencedRoadTrip.Owner.FirstName,
                    OwnerLastName = referencedRoadTrip.Owner.LastName
                };

                return ret;
            }
            return null;

        }
        [Authorize]
        [HttpGet]
        [Route("TripDetailsById/{roadTripId}")]
        public TripDetailsModel GetTripDetailsById(int roadTripId)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);

            var referencedRoadTrip = _ctx.Trips.Include("Owner").FirstOrDefault(x => x.Id == roadTripId
                                                                    && x.UserMap.Any(g => g.PersonId == ownerPerson.Id));
            if (referencedRoadTrip != null)
            {
                var ret = new TripDetailsModel
                {
                    RoadTripName = referencedRoadTrip.Name,
                    RoadTripHash = referencedRoadTrip.Hash,
                    RoadTripId = referencedRoadTrip.Id,
                    OwnerFirstName = referencedRoadTrip.Owner.FirstName,
                    OwnerLastName = referencedRoadTrip.Owner.LastName
                };

                return ret;
            }
            return null;

        }
        [Authorize]
        [HttpGet]
        [Route("UsersForTrip/{roadTripId}")]
        public List<PersonDisplayModel> GetUsersForRoadTrip(int roadTripId)
        {
            
            var roadTrip = _ctx.TripUserMaps.Include("User").Where(x => x.TripId == roadTripId).ToList();
            return
                roadTrip.Select(
                    x =>
                        new PersonDisplayModel
                        {
                            FirstName = x.User.FirstName,
                            LastName = x.User.LastName,
                            Email = x.User.Email,
                            PersonId = x.PersonId,
                            IsOwner = x.IsOwner
                        }).ToList();
            throw  new Exception("Road Trip Id was not recognized");
        }

        [Authorize]
        [HttpPost]
        [Route("AddUserToRoadTrip")]
        public void AddUserToRoadTrip(AddUserToRoadTripModel userToAdd)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);

            var roadTripByRoadTripId = _ctx.Trips.First(x => x.Id == userToAdd.RoadTripId);
            if (roadTripByRoadTripId.OwnerId == ownerPerson.Id)
            {
                var userByUserName = _ctx.Persons.FirstOrDefault(x => x.Email.ToLower() == userToAdd.UserToAddEmail.ToLower());
                if (userByUserName == null)
                {
                    throw new Exception("This email is not registered in the system.");
                }
                //verify that the users is not already added
                if (!_ctx.TripUserMaps.Any(x => x.PersonId == userByUserName.Id && x.TripId == roadTripByRoadTripId.Id))
                {
                    var tripMap = new TripUserMap
                    {
                        IsOwner = false,
                        PersonId = userByUserName.Id,
                        TripId = roadTripByRoadTripId.Id
                    };
                    _ctx.TripUserMaps.Add(tripMap);
                    _ctx.SaveChanges();
                }
                
            }
            throw new Exception("You are not the owner of this road trip.  Only owners can add users!");
            

        }

        [Authorize]
        [HttpPost]
        [Route("JoinRoadTrip")]
        public TripDetailsModel JoinRoadTrip(JoinRoadTripModel joinRoadTrip)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _ctx.Persons.First(x => x.RegisteredUserName == username);

            var roadTripToJoin = _ctx.Trips.FirstOrDefault(x => x.Hash == joinRoadTrip.RoadTripHash.ToUpper());
            var roadTripUsers = _ctx.TripUserMaps.Where(x => x.TripId == roadTripToJoin.Id);
            if (roadTripUsers.Any(x=>x.PersonId == ownerPerson.Id))
            {
                throw new Exception("You are already a part of this road trip.");
            }
            if (roadTripToJoin != null)
            {
                var tripUserMap = new TripUserMap {PersonId = ownerPerson.Id, TripId = roadTripToJoin.Id, IsOwner = false};
                _ctx.TripUserMaps.Add(tripUserMap);
                _ctx.SaveChanges();
                return new TripDetailsModel
                {
                    RoadTripId = roadTripToJoin.Id,
                    //Expenses = 
                };
            }
            throw new Exception("No roadtrip found for the join code");
        }
    }
}
