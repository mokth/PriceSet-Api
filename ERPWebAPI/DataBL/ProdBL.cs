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
    public class ProdBL
    {
        public static List<Prod> GetProd()
        {
            DataTable dt = BaseADOPG.GetData("select * from prod");
            IEnumerable<DataRow> rows = dt.Select().AsEnumerable();
            var list = from row in rows
                       select new Prod()
                       {
                           prodid = Convert2NumTool<Int32>.ConvertVal(row["prodid"]),
                           code = row["code"].ToString(),
                           codename = row["codename"].ToString(),
                           prodgroup = row["prodgroup"].ToString(),
                           lastuser = row["lastuser"].ToString(),
                           modifieddate = Convert2NumTool<DateTime>.ConvertVal(row["modifieddate"]),
                           status = Convert2NumTool<Int32>.ConvertVal(row["status"]),
                       };
            return list.ToList();
        }
    }
}
