using System;
using System.Collections.Generic;

#nullable disable

namespace WEBAPI.Models​
{
    public partial class QuesDb
    {
        public int QnId { get; set; }
        public string Qn { get; set; }
        public string ImageName { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int? Ans { get; set; }
    }
}
