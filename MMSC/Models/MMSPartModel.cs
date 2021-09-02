using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{
    public class MMSPartModel
    {
       

        public string ContentType { get; set; }

        public string Name { get; set; } = null;
        public string Charset { get; set; }
        public string ContentLocation { get; set; }
        public string ContentId { get; set; }


        public byte[] Content { get; set; }
    }
}