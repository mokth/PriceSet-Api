using ERPWebAPI.Model;
using MRP;
using MRP.BL;
using PriceSetAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ERPWebAPI.Shared
{
    public class AuthHelper
    {
        public static bool CheckValidUser(UserInfo user)
        {
            string hashmethod = "SHA1";
            string hashedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(user.password, hashmethod);
            return CheckLogin(user, hashedPassword);
        }

        public static bool CheckValidUserNormal(UserInfo user)
        {
            string hashmethod = "SHA1";
            string hashedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(user.password, hashmethod);
            return CheckLoginNormal(user, hashedPassword);
        }

        private static bool CheckLogin(UserInfo user, string hashedPassword)
        {
            DataRow[] dr;
          
            DataTable dtUser =BaseADOPG.GetData("Select * from Aduser where ID = '" +user.name + "' AND PWord = '" + hashedPassword + "'  ");
          
            dr = dtUser.Select("ID = '" +user.name + "'");
            
            if (dr.Length > 0)
            {
                user.fullname =dr[0]["name"].ToString().ToUpper();
            }

            return (dr.Length > 0) ;


        }

        private static bool CheckLoginNormal(UserInfo user, string hashedPassword)
        {
            DataRow[] dr;
      
            DataTable dtUser = BaseADOPG.GetData("Select * from Aduser where id = '" + user.name + "' AND pword = '" + hashedPassword + "' ");
            //DataTable dtCust = BaseADOPG.GetData("Select CustCode,CustName from sySaCustAcc where CustCode = '" + user.name + "' AND Active = 1 ");

            dr = dtUser.Select("id = '" + user.name + "'");
            if (dr.LongLength > 0)
            {
                user.fullname = dr[0]["name"].ToString().ToUpper();
                user.access =  "";
            }

            return (dr.Length > 0) ;


        }

        public static bool IsValidAccessRight(string userID,string screenID)
        {
            bool isValid = false;
            //DataTable dtUserRight = AdUserBL.GetUserRight(userID);
            //if (CUser.CheckUserRight2((int)CCommon.enGroupRight.Access, userID, dtUserRight, screenID))
            //{
            //    isValid = true;
            //}
            return isValid;
        }
    }
}
