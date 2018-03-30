using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PriceSetAPI.DataBL
{
    public class CAdapter
    {
        public static void GeneratePrcDtlCommand(ref NpgsqlDataAdapter da)
        {
            NpgsqlParameter p;
            NpgsqlCommand com;
            // update
            com = new NpgsqlCommand();

            com.Parameters.Add("@uid", NpgsqlDbType.Integer, 4, "uid");
            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcdate", NpgsqlDbType.Date, 8, "prcdate");
            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@unitprice", NpgsqlDbType.Double, 4, "unitprice");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");
            p = com.Parameters.Add("@olduid", NpgsqlDbType.Integer, 4, "uid");
            p.SourceVersion = DataRowVersion.Original;

            com.CommandText = "UPDATE prcdtl SET unitprice = @unitprice,modifieddate= @modifieddate, lastuser = @lastuser WHERE uid = @olduid";
            da.UpdateCommand = com;

            // insert
            com = new NpgsqlCommand();

            com.Parameters.Add("@uid", NpgsqlDbType.Integer, 4, "uid");
            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcdate", NpgsqlDbType.Date, 8, "prcdate");
            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@unitprice", NpgsqlDbType.Double, 4, "unitprice");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");
            com.CommandText =
                "INSERT INTO prcdtl (prcid, prcdate, prodid, unitprice, modifieddate, lastuser )" +
                            "VALUES (@prcid, @prcdate, @prodid, @unitprice, @modifieddate, @lastuser )";

            da.InsertCommand = com;

            // delete
            com = new NpgsqlCommand();
            com.Parameters.Add("@uid", NpgsqlDbType.Integer, 4, "uid");
            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcdate", NpgsqlDbType.Date, 8, "prcdate");
            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@unitprice", NpgsqlDbType.Double, 4, "unitprice");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");
            p = com.Parameters.Add("@olduid", NpgsqlDbType.Integer, 4, "uid");
            p.SourceVersion = DataRowVersion.Original;
       
            com.CommandText = "DELETE FROM prcdtl WHERE uid= @olduid ";
            da.DeleteCommand = com;

        }

        public static void GenerateAdUserCommand(ref NpgsqlDataAdapter da)
        {
            NpgsqlParameter p;
            NpgsqlCommand com;
            // update
            com = new NpgsqlCommand();
                      
            com.Parameters.Add("@id", NpgsqlDbType.Varchar, 10, "id");
            com.Parameters.Add("@name", NpgsqlDbType.Varchar, 30, "name");
            com.Parameters.Add("@pword", NpgsqlDbType.Varchar, 50, "pword");
            com.Parameters.Add("@email", NpgsqlDbType.Varchar, 30, "email");
           
            p = com.Parameters.Add("@oldid", NpgsqlDbType.Varchar, 10, "id");
            p.SourceVersion = DataRowVersion.Original;

            com.CommandText = "UPDATE aduser SET pword = @pword WHERE id = @oldid";
            da.UpdateCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@id", NpgsqlDbType.Varchar, 10, "id");
            com.Parameters.Add("@name", NpgsqlDbType.Varchar, 30, "name");
            com.Parameters.Add("@pword", NpgsqlDbType.Varchar, 50, "pword");
            com.Parameters.Add("@email", NpgsqlDbType.Varchar, 30, "email");

            

            com.CommandText = "Insert into aduser (id, name, pword, email) VALUES (@id, @name,@pword,@email)";
            da.InsertCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@id", NpgsqlDbType.Varchar, 10, "id");
            com.Parameters.Add("@name", NpgsqlDbType.Varchar, 30, "name");
            com.Parameters.Add("@pword", NpgsqlDbType.Varchar, 50, "pword");
            com.Parameters.Add("@email", NpgsqlDbType.Varchar, 30, "email");

            p = com.Parameters.Add("@oldid", NpgsqlDbType.Varchar, 10, "id");
            p.SourceVersion = DataRowVersion.Original;
            com.CommandText = "delete aduser where id=@oldid";
            da.DeleteCommand = com;
        }

        public static void GenerateProdCommand(ref NpgsqlDataAdapter da)
        {
            NpgsqlParameter p;
            NpgsqlCommand com;
            // update
            com = new NpgsqlCommand();

            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@code", NpgsqlDbType.Varchar, 10, "code");
            com.Parameters.Add("@codename", NpgsqlDbType.Varchar, 50, "codename");
            com.Parameters.Add("@prodgroup", NpgsqlDbType.Varchar, 10, "prodgroup");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");

            p = com.Parameters.Add("@oldid", NpgsqlDbType.Integer, 10, "prodid");
            p.SourceVersion = DataRowVersion.Original;

            com.CommandText = @"UPDATE prod
                                        SET code =@code, codename =@codename, prodgroup =@prodgroup, modifieddate =@modifieddate, lastuser =@lastuser
                              WHERE id = @oldid";
            da.UpdateCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@code", NpgsqlDbType.Varchar, 10, "code");
            com.Parameters.Add("@codename", NpgsqlDbType.Varchar, 50, "codename");
            com.Parameters.Add("@prodgroup", NpgsqlDbType.Varchar, 10, "prodgroup");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");


            com.CommandText =@"insert into prod (prodid, code, codename, prodgroup, modifieddate, lastuser)
                                values (@prodid, @code, @codename, @prodgroup, @modifieddate, @lastuser) ";
            da.InsertCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@prodid", NpgsqlDbType.Integer, 4, "prodid");
            com.Parameters.Add("@code", NpgsqlDbType.Varchar, 10, "code");
            com.Parameters.Add("@codename", NpgsqlDbType.Varchar, 50, "codename");
            com.Parameters.Add("@prodgroup", NpgsqlDbType.Varchar, 10, "prodgroup");
            com.Parameters.Add("@modifieddate", NpgsqlDbType.Timestamp, 20, "modifieddate");
            com.Parameters.Add("@lastuser", NpgsqlDbType.Varchar, 10, "lastuser");

            p = com.Parameters.Add("@oldid", NpgsqlDbType.Integer, 10, "prodid");
            p.SourceVersion = DataRowVersion.Original;
           
            com.CommandText = "delete prod where prodid=@oldid";
            da.DeleteCommand = com;
        }

        public static void GeneratePrcHdrCommand(ref NpgsqlDataAdapter da)
        {
            NpgsqlParameter p;
            NpgsqlCommand com;
            // update
            com = new NpgsqlCommand();

            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcname", NpgsqlDbType.Varchar, 50, "prcname");
           
            p = com.Parameters.Add("@oldid", NpgsqlDbType.Varchar, 10, "prcid");
            p.SourceVersion = DataRowVersion.Original;

            com.CommandText = @"UPDATE prchdr
                                        SET prcname = @prcname
                              WHERE prcid = @oldid";
            da.UpdateCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcname", NpgsqlDbType.Varchar, 50, "prcname");


            com.CommandText = @"insert into prchdr (prcid, prcname)
                                values (@prcid, @prcname) ";
            da.InsertCommand = com;

            com = new NpgsqlCommand();

            com.Parameters.Add("@prcid", NpgsqlDbType.Varchar, 10, "prcid");
            com.Parameters.Add("@prcname", NpgsqlDbType.Varchar, 50, "prcname");

            p = com.Parameters.Add("@oldid", NpgsqlDbType.Varchar, 10, "prcid");
            p.SourceVersion = DataRowVersion.Original;

            com.CommandText = "delete from prchdr where prcid=@oldid";
            da.DeleteCommand = com;
        }
    }
}
