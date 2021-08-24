using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{
    public class IRResponseModel
    {
        public string Status { get; set;}
        public string Description { get; set; }
        public string AddStatus { get; set; }

        public Dictionary<string, string> Info = new Dictionary<string, string>();

    }
}