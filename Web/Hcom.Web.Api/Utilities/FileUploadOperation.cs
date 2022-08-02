using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    public class FileUploadOperation : IOperationFilter
    {
        //added
        private const string formDataMimeType = "multipart/form-data";
        private static readonly string[] formFilePropertyNames =
            typeof(IFormFile).GetTypeInfo().DeclaredProperties.Select(p => p.Name).ToArray();

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ////Requirement
            //if (operation.OperationId == "[complete url]")
            //{
            //    operation.Parameters.Clear();
            //    operation.Parameters.Add(new NonBodyParameter
            //    {
            //        Name = "file",
            //        In = "formData",
            //        Description = "Upload File",
            //        Required = true,
            //        Type = "file"
            //    });
            //    //operation.Parameters.Add(new NonBodyParameter
            //    //{
            //    //    Name = "meterid"
            //    //});
            //    operation.Consumes.Add("multipart/form-data");
            //}

        }
    }
}
