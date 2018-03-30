using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceSetAPI.Model
{
    public class Prod
    {
        public Int32 prodid { set; get; }
        public string code { set; get; }
        public string codename { set; get; }
        public string prodgroup { set; get; }
        public string lastuser { set; get; }
        public DateTime modifieddate { set; get; }
        public Int32 status { set; get; }
    }
}
