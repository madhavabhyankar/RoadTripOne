using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class AddUserToRoadTripModel
    {
        public int RoadTripId { get; set; }
        public string UserToAddEmail { get; set; }
        

    }
}