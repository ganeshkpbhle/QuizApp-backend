using System;
using System.Collections.Generic;

#nullable disable

namespace WEBAPI.Models​
{
    public partial class Attempt
    {
        public int EntryId { get; set; }
        public DateTime Date { get; set; }
        public int UId { get; set; }
        public int? TimeSpent { get; set; }
        public int? Score { get; set; }

        public virtual UserDatum UIdNavigation { get; set; }
    }
}
