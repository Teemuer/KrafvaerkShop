using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krafvaerk_shop.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private string _cartKey;

        public RequestMiddleware(RequestDelegate next, string cartKey)
        {
            _cartKey = cartKey;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Check session and cookies for cart id
            HandleCartIdInWebStorage(context);

            // Let the middleware pipeline run
            await _next(context);
        }

        /// <summary>
        /// Checks if cartid is in cookies or session. Creates and adds id in both if it does not exist
        /// </summary>
        /// <param name="context"></param>
        private void HandleCartIdInWebStorage(HttpContext context)
        {
            string cartIdInSession = context.Session.GetString(_cartKey);

            if (!IsCartInCookies(context, out string cartIdInCookie))
            {
                if (string.IsNullOrEmpty(cartIdInSession))
                {
                    //create cartid with guid
                    cartIdInCookie = Guid.NewGuid().ToString();
                    SetCookie(context, cartIdInCookie);
                }
                else
                {
                    SetCookie(context, cartIdInSession);
                }
            }
            if (string.IsNullOrEmpty(cartIdInSession))
            {
                //lets add cartid to session also
                if (!string.IsNullOrEmpty(cartIdInCookie))
                {
                    context.Session.SetString(_cartKey, cartIdInCookie);
                }
            }
        }

        /// <summary>
        /// checks if cartid is in cookies
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cartIdInCookie"></param>
        /// <returns></returns>
        private bool IsCartInCookies(HttpContext context, out string cartIdInCookie)
        {
            var cookie = context.Request.Cookies.FirstOrDefault(x => x.Key == _cartKey);
            cartIdInCookie = cookie.Value;
            return cookie.Key != null;
        }

        /// <summary>
        /// sets cartId for 30 days in cookies
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cartId"></param>
        private void SetCookie(HttpContext context, string cartId)
        {
            SetCookie(_cartKey, cartId.ToString(), context.Response, 30); //add id for a month
        }

        public void SetCookie(string key, string value, HttpResponse response, int? expireTimeInDays)
        {
            CookieOptions option = new CookieOptions();

            if (expireTimeInDays.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTimeInDays.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);

            response.Cookies.Append(key, value, option);
        }
    }
}
