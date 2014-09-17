using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class PersonDisplayModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonId { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
    }
}