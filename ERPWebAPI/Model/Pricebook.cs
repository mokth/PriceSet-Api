using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceSetAPI.Model
{
    public class PriceHdr
    {
        public string prcid { set; get; }
        public string prcname { set; get; }
    }

    public class PriceDtl
    {
        public Int32 uid { set; get; }
        public string prcid { set; get; }
        public string prcdate { set; get; }
        public Int32 prodid { set; get; }
        public string code { set; get; }
        public string codename { set; get; }
        public double unitprice { set; get; }
        public string lastuser { set; get; }
        public DateTime modifieddate { set; get; }
    }


    public class PriceSet
    {
        public string prcid { set; get; }
        public string prcdate { set; get; }
        public string lastuser { set; get; }
        public PriceDtl[] items;
        public string status { set; get; }

    }

    public class PrcSet
    {
        public string prcid { set; get; }
        public string prcname { set; get; }
       

    }
}
