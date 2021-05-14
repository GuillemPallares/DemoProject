using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config
                .EnableSwagger(
                c => {
                    c.SingleApiVersion("v1", "Demo Project API")
                        .Description("Una API con Autenticacion JWT Basada en Roles.")
                                .TermsOfService("Términos de servicio.")
                                .Contact(x => x
                                    .Name("Guillem Pallarés")
                                    .Url("https://github.com/GuillemPallares")
                                    .Email("guillempallares@outlook.es"));

                    c.IncludeXmlComments(Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "bin", "WebApi.xml"));
                    c.ApiKey("Authorization")
                        .Description("Introduce el Token JWT aquí.")
                        .Name("Bearer")
                        .In("header");
                })
                .EnableSwaggerUi(
                c =>
                    {
                        c.EnableApiKeySupport("Authorization", "header");
                    }
                );

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
