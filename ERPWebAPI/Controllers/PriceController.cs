using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MRP.BL;
using Npgsql;
using POS.DATA;
using PriceSetAPI.DataBL;
using PriceSetAPI.Model;

namespace PriceSetAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Price")]
    public class PriceController : Controller
    {
        NpgsqlConnection con = new NpgsqlConnection();
        NpgsqlDataAdapter da = new NpgsqlDataAdapter();
        NpgsqlDataAdapter da2 = new NpgsqlDataAdapter();
        DataTable dt;
        string _err = "";

        // GET: api/Price
        public JsonResult Get()
        {
            JsonResult categoryJson = new JsonResult(PriceBL.GetPriceID());
            return categoryJson;
        }

        [HttpGet, Route("set")]
        public JsonResult GetPriceSet() //yyyy-mm-dd
        {
            JsonResult categoryJson = new JsonResult(PriceBL.GetPrcSet());
            return categoryJson;
        }

        [HttpGet, Route("details")]
        public JsonResult GetPriceSetDtls() //yyyy-mm-dd
        {
            var queryString = this.Request.Query;
            StringValues prcid;
            StringValues datestr;
            queryString.TryGetValue("id", out prcid);
            queryString.TryGetValue("date", out datestr);
            JsonResult categoryJson = new JsonResult(PriceBL.GetPriceDetails(prcid, datestr));
            return categoryJson;
        }

        [HttpPost, Route("priceid")]
        public JsonResult SavePriceSet([FromBody] PrcSet[] prcsets)
        {
            string msg = "";
           
            if (!CSys.OpenCon(ref con))
            {
                JsonResult errjson = Json(new
                {
                    ok = "no",
                    Error = "connection error."
                });
                return errjson;
            }
            NpgsqlTransaction sqlTrans;
            sqlTrans = con.BeginTransaction();
           // BaseADOPG.ExceuteSql("Delete from prchdr",con,sqlTrans);
            dt = BaseADOPG.GetData("Select * from prchdr");
            foreach (DataRow row in dt.Select())
            {
                row.Delete();
            }
            
            UpdatePrcSet(prcsets);
            bool success = false;
         
            CAdapter.GeneratePrcHdrCommand(ref da);
            if (UpdateTable(ref dt, sqlTrans))
            {
                sqlTrans.Commit();
                success = true;
                msg = "Saved successfully";
            }
            else
            {
                sqlTrans.Rollback();
                msg = _err;
            }

            JsonResult restultJson = Json(new
            {
                ok = (success) ? "yes" : "no",
                Error = msg
            });
            con.Close();
            return restultJson;
        }

        [HttpPost, Route("priceset")]
        public JsonResult SavePriceSet([FromBody] PriceSet prcset)
        {
            string msg = "";
            if (!CSys.OpenCon(ref con))
            {
                JsonResult errjson = Json(new
                {
                    ok = "no",
                    Error = "connection error."
                });
                return errjson;
            }
          

            // BaseADOPG.ExceuteSql("Delete from from prcdtl where where prcdate='" + prcset.prcdate + "' and prcid='" + prcset.prcid + "'", con, sqlTrans);
            dt = BaseADOPG.GetData("Select * from prcdtl where prcdate='" + prcset.prcdate + "' and prcid='" + prcset.prcid + "'");
           
            UpdatePrcdtl(prcset);
            bool success = false;
            NpgsqlTransaction sqlTrans;
            sqlTrans = con.BeginTransaction();
            CAdapter.GeneratePrcDtlCommand(ref da);
            if (UpdateTable(ref dt,sqlTrans))
            {
                sqlTrans.Commit();
                success = true;
                msg = "Saved successfully";
            }
            else
            {
                sqlTrans.Rollback();
                msg = _err;
            }

            JsonResult restultJson = Json(new
            {
                ok = (success) ? "yes" : "no",
                Error = msg
            });
            con.Close();
            return restultJson;
        }

        private void UpdatePrcdtl(PriceSet prcset)
        {
            DataRow[] found;
            DataRow nrow;
            DateTime date = Convert.ToDateTime(prcset.prcdate);
            DateTime current = DateTime.Now;
            foreach (PriceDtl itm in prcset.items)
            {
                found = dt.Select("prodid='" + itm.prodid.ToString() + "'");
                if (found.Length == 0)
                {
                    nrow = dt.NewRow();
                    nrow["prodid"] = itm.prodid;
                    nrow["prcdate"] = date;
                    nrow["prcid"] = prcset.prcid;
                }
                else
                {
                    nrow = found[0];
                }
                nrow["unitprice"] = itm.unitprice;
                nrow["lastuser"] = prcset.lastuser;
                nrow["modifieddate"] = current;

                if (found.Length == 0)
                {
                    dt.Rows.Add(nrow);
                }
            }
        }

        private void UpdatePrcSet(PrcSet[] prcsets)
        {
            DataRow nrow;

            foreach (PrcSet itm in prcsets)
            {
                DataRow[] found = dt.Select("prcid='" + itm.prcid + "'");
                if (found.Length == 0)
                {
                    nrow = dt.NewRow();
                    nrow["prcid"] = itm.prcid;
                }
                else
                {
                    nrow = found[0];
                }
                nrow["prcname"] = itm.prcname;
                if (found.Length == 0)
                {
                    dt.Rows.Add(nrow);
                }
            }
        }

        private bool UpdateRecDym(NpgsqlTransaction sqlTrans, DataTable dt)
        {

            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(da);
            da.DeleteCommand = builder.GetDeleteCommand();
            da.InsertCommand =builder.GetInsertCommand();
            da.UpdateCommand = builder.GetUpdateCommand(true);
            da.UpdateCommand.CommandText = @"UPDATE prcdtl SET unitprice = @p4,  lastuser = @p6 WHERE uid = @p7";
            // var aa =da.InsertCommand.Parameters["hdruid"];
            if (!UpdateTable(ref dt, sqlTrans))
                return false;
            return true;
        }

        private bool UpdateTable(ref DataTable tablename, NpgsqlTransaction sqlTrans)
        {

            da.UpdateCommand.Connection = con;
            da.InsertCommand.Connection = con;
            da.DeleteCommand.Connection = con;

            da.InsertCommand.Transaction = sqlTrans;
            da.UpdateCommand.Transaction = sqlTrans;
            da.DeleteCommand.Transaction = sqlTrans;

            try
            {

                da.Update(tablename);
                da.UpdateCommand.Parameters.Clear();

            }
            catch (Exception ex)
            {
                //CCommon.CreateMessageAlert(this, "Error saving data, please contact your system admistrator! " + CCommon.ConvertToHTMLString(ex.Message), "CONNECTION_EXCEPTION");
                // ItemGridEntry.GetGridView.JSProperties["cpErrMsg"] = "Error saving data, please contact your system admistrator! " + ex.Message;
                _err = ex.Message;
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
    }
}
