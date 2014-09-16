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
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        [ForeignKey("User")]
        public int PersonId { get; set; }

        public bool IsOwner { get; set; }
        public Trip Trip { get; set; }
        public Person User { get; set; }
    }
}