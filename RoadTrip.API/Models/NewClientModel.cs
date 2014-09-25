using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Models
{
    public class NewClientModel
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public int ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}