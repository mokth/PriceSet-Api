using MRP.BL;
using PriceSetAPI.Model;
using PriceSetAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PriceSetAPI.DataBL
{
    public class PriceBL
    {
        public static List<PriceHdr> GetPriceID()
        {
            DataTable dt = BaseADOPG.GetData("select * from prchdr");
            IEnumerable<DataRow> rows = dt.Select().AsEnumerable();
            var list = from row in rows
                       select new PriceHdr()
                       {
                           prcid = row["prcid"].ToString(),
                           prcname = row["prcname"].ToString()
                       };
            return list.ToList();
        }

        public static List<PriceDtl> GetPriceDetails(string prcid,string prcdate)
        {
           
            string sql = string.Format(@"SELECT uid, prcid, prcdate,coalesce( m.prodid,v.prodid) prodid,coalesce( m.code,v.code) code,
                                                 coalesce( m.codename,v.codename) codename, coalesce(unitprice,0) unitprice, v.modifieddate, v.lastuser
                                             from prod m
                                             left join
                                          (
                                            SELECT uid, prcid, prcdate, p.prodid,i.code,i.codename, unitprice, p.modifieddate, p.lastuser
                                             FROM  prcdtl p left outer join prod i   on p.prodid= i.prodid where prcdate='{0}' and prcid='{1}'
	                                        ) v  on  v.prodid= m.prodid where m.status=0 "
                                         , prcdate, prcid);
            DataTable dt = BaseADOPG.GetData(sql);
            if (dt.Rows.Count == 0)
            {
                string sqlo = string.Format(@"SELECT max(prcdate) prcdate FROM prcdtl where prcdate <'{0}'"
                                        , prcdate);
                DataTable dto = BaseADOPG.GetData(sqlo);
                DateTime d = Convert.ToDateTime(prcdate);
                if (dto.Rows.Count > 0)
                {
                    if (dto.Rows[0]["prcdate"] != DBNull.Value)
                    {
                        d = Convert.ToDateTime(dto.Rows[0]["prcdate"]);
                    }
                }

               // DateTime d = Convert.ToDateTime(prcdate);
                sql = string.Format(@"SELECT uid, prcid, prcdate,coalesce( m.prodid,v.prodid) prodid,coalesce( m.code,v.code) code,
                                             coalesce( m.codename,v.codename) codename, coalesce(unitprice,0) unitprice, v.modifieddate, v.lastuser
                                        from prod m
                                         left join
                                         (
                                        SELECT uid, prcid, prcdate, p.prodid,i.code,i.codename, unitprice, p.modifieddate, p.lastuser
                                         FROM  prcdtl p left outer join prod i   on p.prodid= i.prodid where prcdate='{0}' and prcid='{1}'
	                                        ) v  on  v.prodid= m.prodid where m.status=0 "
                                         , d.ToString("yyyy-MM-dd"), prcid);
                dt = BaseADOPG.GetData(sql);
            }
            IEnumerable<DataRow> rows = dt.Select().AsEnumerable();
            var list = from row in rows
                       select new PriceDtl()
                       {
                           uid = Convert2NumTool<Int32>.ConvertVal(row["uid"]),
                           prcid = prcid, /* row["prcid"].ToString()*/
                           prcdate = Convert2NumTool<DateTime>.ConvertVal(row["prcdate"]).ToString("yyyy-MM-dd"),
                           prodid = Convert2NumTool<Int32>.ConvertVal(row["prodid"]),                          
                           unitprice = Convert2NumTool<double>.ConvertVal(row["unitprice"]),
                           code = Convert2NumTool<string>.ConvertVal(row["code"]),
                           codename = Convert2NumTool<string>.ConvertVal(row["codename"]),
                           lastuser = Convert2NumTool<string>.ConvertVal(row["lastuser"]),
                           modifieddate = Convert2NumTool<DateTime>.ConvertVal(row["modifieddate"])
                       };

            return list.ToList();
        }

        public static List<PrcSet> GetPrcSet()
        {
            string sql = string.Format(@"Select * from prchdr");
            DataTable dt = BaseADOPG.GetData(sql);
            IEnumerable<DataRow> rows = dt.Select().AsEnumerable();
            var list = from row in rows
                       select new PrcSet()
                       {
                           prcid = row["prcid"].ToString(),
                           prcname = row["prcname"].ToString()
                       };

            return list.ToList();
        }
    }
}
