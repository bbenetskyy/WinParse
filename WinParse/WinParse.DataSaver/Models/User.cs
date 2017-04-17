using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver.Models
{
    public class User : BaseObject
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool PlaceBets { get; set; }
    }
}
