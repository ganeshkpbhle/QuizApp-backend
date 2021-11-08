using System;
using System.Collections.Generic;

#nullable disable

namespace WEBAPI.Models​
{
    public partial class UserDatum
    {
        public UserDatum()
        {
            Attempts = new HashSet<Attempt>();
        }

        public int UId { get; set; }
        public string UEmail { get; set; }
        public string Passwd { get; set; }
        public string Mobnumber { get; set; }

        public virtual ICollection<Attempt> Attempts { get; set; }
    }
}
