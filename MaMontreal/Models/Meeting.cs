using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaMontreal.Models
{
    public class Meeting
    {
        //Data Annotations missing to optimize the database
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";//Does Google calendar Api need an address object for scheduling?

        //..rest of properties

        //relational properties

    }
}