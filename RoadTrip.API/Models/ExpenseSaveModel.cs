using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class ExpenseSaveModel
    {
        public int TripId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double DollarAmount { get; set; }
        public string Notes { get; set; }
        
    }
}