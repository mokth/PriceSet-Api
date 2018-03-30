using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRP.BL;
using Npgsql;
using POS.DATA;
using PriceSetAPI.DataBL;
using PriceSetAPI.Model;

namespace PriceSetAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Prod")]
    public class ProdController : Controller
    {
        NpgsqlConnection con = new NpgsqlConnection();
        NpgsqlDataAdapter da = new NpgsqlDataAdapter();
        DataTable dt;
        string _err = "";

        // GET: api/Price
        [HttpGet]
        public JsonResult Get()
        {
            JsonResult categoryJson = new JsonResult(ProdBL.GetProd());
            return categoryJson;
        }

        [HttpPost, Route("update")]
        public JsonResult SaveProds([FromBody] Prod[] prods)
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
            BaseADOPG.ExceuteSql("Delete from prod ", con, sqlTrans);
            dt = BaseADOPG.GetData("Select * from prod");
            DataRow nrow;
            foreach (Prod itm in prods)
            {
                nrow =dt.NewRow();
                nrow["prodid"] = itm.prodid;
                nrow["code"] = itm.code;
                nrow["codename"] = itm.codename;
                nrow["prodgroup"] = itm.prodgroup;
                nrow["modifieddate"] = DateTime.Now;
                nrow["lastuser"] = "Admin";
                dt.Rows.Add(nrow);
            }
            bool success = false;         
            CAdapter.GenerateProdCommand(ref da);
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