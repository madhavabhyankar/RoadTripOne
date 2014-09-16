using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class ExpenseDisplayModel
    {
        public int RoadTripId { get; set; }
        public double DollarAmount { get; set; }
        public string Notes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}