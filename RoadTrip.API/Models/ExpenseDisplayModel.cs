using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class ExpenseDisplayModel
    {
        public double DollarAmount { get; set; }
        public RoadTripUserModel UserModel { get; set; }
    }
}