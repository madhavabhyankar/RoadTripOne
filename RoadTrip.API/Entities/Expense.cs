using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoadTrip.API.Entities
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public double Amount { get; set; }
        [Column(TypeName = "Date")]
        public DateTime ExpenseDate { get; set; }
        [MaxLength(20)]
        public string Notes { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        [ForeignKey("Trip")]
        public int TripId { get; set; }

        public Person Person { get; set; }
        public Trip Trip { get; set; }
    }
}