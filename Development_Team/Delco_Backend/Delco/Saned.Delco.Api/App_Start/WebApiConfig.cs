﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Saned.Delco.Api
{
    public static class WebApiConfig
    {
       

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );




          //  config.Routes.MapHttpRoute(
          //    name: "SignalR",
          //    routeTemplate: "{controller}/{id}",
          //    defaults: new { id = RouteParameter.Optional }
          //);


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
