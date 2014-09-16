using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class TripDetailsModel
    {
        public int RoadTripId { get; set; }
        public string RoadTripName { get; set; }

        public string RoadTripHash { get; set; }
        public List<RoadTripUserModel> Users { get; set; }
        public List<ExpenseDisplayModel> Expenses { get; set; }
    }
}