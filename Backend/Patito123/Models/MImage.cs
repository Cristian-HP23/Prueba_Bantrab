using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patito123.Models
{
    public class MImage
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public string base64 { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

    }
}