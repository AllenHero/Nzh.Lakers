using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Util.Helper
{
    public class WebHelper
    {
        private HttpContext httpContext;

        public WebHelper(HttpContext context)
        {
            httpContext = context;
        }

        public void WriteSession(string key, string value)
        {
            httpContext.Session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            bool rs = httpContext.Session.TryGetValue(key, out byte[] val);
            string value = "";
            if (rs)
            {
                value = Encoding.UTF8.GetString(val);
                return value;
            }
            value = "";
            return value;
        }

        public void RemoveSession(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return;
            httpContext.Session.Remove(key);
        }

        public void ClearSession()
        {
            httpContext.Session.Clear();
        }

        public void WriteCookie(string strName, string strValue, int minutes = 60)
        {
            httpContext.Response.Cookies.Append(strName, strValue, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        public string GetCookie(string strName)
        {
            var res = httpContext.Request.Cookies.TryGetValue(strName, out string value);
            string reslut = "";
            if (res)
                return value;
            return reslut;
        }

        public void RemoveCookie(string CookiesName)
        {
            httpContext.Response.Cookies.Delete(CookiesName);
        }
    }
}
