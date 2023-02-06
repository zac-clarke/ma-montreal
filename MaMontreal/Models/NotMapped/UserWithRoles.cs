using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaMontreal.Models.NotMapped
{
    public class UserWithRoles
    {
        public string _userId { get; set; }
        public List<ManagedRole> _selectedRoles { get; set; }
    }

}