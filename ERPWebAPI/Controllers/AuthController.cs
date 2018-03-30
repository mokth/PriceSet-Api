using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERPWebAPI.Model;
using ERPWebAPI.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using PriceSetAPI.Model;
using MRP.BL;
using System.Data;
using PriceSetAPI.DataBL;

namespace ERPWebAPI.Controllers
{
    // [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public AuthController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            JsonResult categoryJson = new JsonResult(ProdBL.GetProd());
            return categoryJson;
        }

        [HttpPost, Route("jwt")]
        [AllowAnonymous]
        public JsonResult GetJWT([FromBody]UserInfo user)
        {
            JsonResult categoryJson = null;
            if (AuthHelper.CheckValidUser(user))
            {
                categoryJson = new JsonResult(Model.JWTokenHelper.GenerateJWT(user));
            } else categoryJson = new JsonResult("Invalid user/password");

            return categoryJson;

        }

        [HttpPost, Route("jwt1")]
        public JsonResult GetJWT1([FromBody]UserInfo user)
        {
            JsonResult categoryJson = null;
            if (AuthHelper.CheckValidUserNormal(user))
            {
                categoryJson = new JsonResult(Model.JWTokenHelper.GenerateJWT(user));
            }
            else categoryJson = new JsonResult("Invalid user/password");

            return categoryJson;

        }

        [HttpPost, Route("access")]
        public JsonResult GetAccessRight([FromBody]UserInfo user)
        {
            JsonResult categoryJson = null;
            //make used of user.fullname to be the ERP screen id
            bool isvalid = AuthHelper.IsValidAccessRight(user.name, user.fullname);
            categoryJson = Json(new
            {
                ok = (isvalid) ? "YES" : "NO"                
            });

            return categoryJson;

        }

        [HttpPost, Route("reset")]
        [AllowAnonymous]
        public JsonResult ResetPassword([FromBody]UserInfo user)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            AuthenticationHelper help = new AuthenticationHelper();
            JsonResult categoryJson = null;
            if (help.ResetPassowrd(user.name, user.fullname, webRootPath))
            {
                categoryJson = Json(new
                {
                    ok = "OK",
                    Error = help.Errmgs
                });
            }
            else
            {
                categoryJson = Json(new
                {
                    ok = "",
                    Error = help.Errmgs
                });
            }

            return categoryJson;
        }

        [HttpPost, Route("change")]
        public JsonResult ChangePassword([FromBody]UserInfo user)
        {
            AuthenticationHelper help = new AuthenticationHelper();
            JsonResult categoryJson = null;
            if (help.ChangedPassowrd(user.name,user.password,user.fullname))
            {
                categoryJson = Json(new
                {
                    ok = "OK",
                    Error = help.Errmgs
                });
            }
            else
            {
                categoryJson = Json(new
                {
                    ok = "",
                    Error = help.Errmgs
                });
            }

            return categoryJson;
        }

    }
}