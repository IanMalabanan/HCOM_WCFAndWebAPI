
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    // Swagger IOperationFilter implementation that will validate whether an action has an applicable Authorize attribute.
    // If it does, we add the ECOMSS_api scope so IdentityServer can validate permission for that scope.
    internal class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .Union(context.MethodInfo.GetCustomAttributes(true));

            if (attributes.OfType<IAllowAnonymous>().Any())
            {
                return;
            }

            var authAttributes = attributes.OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                operation.Responses.Add("401" , new OpenApiResponse { Description = "Unauthorized" });

                if (authAttributes.Any(att => !String.IsNullOrWhiteSpace(att.Roles) || !String.IsNullOrWhiteSpace(att.Policy)))
                {
                    operation.Responses["403"] = new OpenApiResponse { Description = "Forbidden" };
                }

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer(Token) Authentication",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                };
            }
        }
        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    //Client Info
        //    //operation.Parameters.Add(new NonBodyParameter
        //    //{
        //    //    Name = "ClientInfo",
        //    //    In = "header",
        //    //    Type = "string",
        //    //    Description = "Client Info Fields"
        //    //});


        //    // Check for authorize attribute
        //    var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
        //        .Union(context.MethodInfo.GetCustomAttributes(true))
        //        .OfType<AuthorizeAttribute>()
        //        .Any();

        //    if (hasAuthorize)
        //    {
        //        operation.Responses.Add("401", new Open { Description = "Unauthorized" });

        //        operation.Security = new List<IDictionary<string, IEnumerable<string>>>
        //        {
        //            new Dictionary<string, IEnumerable<string>>
        //            {
        //                { "oauth2", new string [] { } }
        //            }
        //        };
        //    }
        //}
    }
}
