using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver.Models
{
   public class BaseObject
    {
        /// <summary>
        /// Id stored in DB, ONLY auto increment so please NOT fill it
        /// </summary>
        public string Id { get; set; }
    }
}
