using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Entities
{
    public class TripUserMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Trips")]
        public int TripId { get; set; }
        [ForeignKey("Users")]
        public int PersonId { get; set; }

        public List<Trip> Trips { get; set; }
        public List<Person> Users { get; set; }
    }
}