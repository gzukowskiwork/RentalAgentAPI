using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleFakeRent.Extensions
{
    public static class MethodsExtensions
    {
        /// <summary>
        /// Extended exception details - concateted properties from Exception class
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Returns concateted properties from Exception class </returns>
        public static string ErrorMessageExtension(this Exception e)
        {
            return "Wystąpił problem: " + "\n"+ e.Message + " \n ### STACK TRACE ### " + e.StackTrace + "\n ### INNER EXCEPTION ### " + e.InnerException + "\n ### SOURCE ### " + e.Source;
            
        }

        public static int SelectLoggedUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return -1;
            }
            return int.Parse(httpContext.User.Claims.Single(x => x.Type == "id").Value);
        }
    }
}
