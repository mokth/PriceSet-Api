using Jose;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

           // byte[] secretKey = new byte[32];
           // rng.GetBytes(secretKey);
           //string aa1= Convert.ToBase64String(secretKey);
           // Console.WriteLine(aa1);
            string sk = "Itd0tX3+1BBP2DZwE7FfDjtaYC1sUgexPIXl+PILB8E=";
            byte[] secretKey = Base64UrlDecode(sk);

            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(10);

            var payload = new Dictionary<string, object>()
            {
                {"iss", "https://www.wincom3cloud.com/"},
                {"aud", "MOK"},
                {"sub", "TH MOK"},
                {"iat", "1500880922"},
                {"exp", "1500880922"}
            };

            string token = JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);

            Console.WriteLine(token);
            var tst = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3d3dy53aW5jb20zY2xvdWQuY29tLyIsImF1ZCI6Ik1PSyIsInN1YiI6IlRIIE1PSyIsImlhdCI6IjE1MDA4ODA5MjIiLCJleHAiOiIxNTAwODgwOTIyIn0.p12F_tqW4f9eL4AhOj_DNnd0sGynn176vsUIqKTOlDw"; //g

            string json = JWT.Decode(tst, secretKey, JwsAlgorithm.HS256);
            var aa = JObject.Parse(json);
            Console.WriteLine(json);
          
            
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        static byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        static long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}