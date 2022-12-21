using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patito123.Models
{
    public class MRespuesta
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public MError error { get; set; } = new MError();
        public MImage[] imagenes { set; get; }
    }
}