using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roommates.Models;

namespace Roommates.Models
{
    public class RoommateChore
    {
        public Chore Chore { get; set; }
        public Roommate Roommate { get; set; }
    }
}
