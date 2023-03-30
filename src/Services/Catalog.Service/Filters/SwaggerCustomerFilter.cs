using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.API.Filters
{
    public class SwaggerCustomerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //https://www.c-sharpcorner.com/article/add-custom-parameters-in-swagger-using-asp-net-core-3-1/
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && !descriptor.ControllerName.StartsWith("Catalog"))
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "x-correlationToken",
                    In = ParameterLocation.Header,
                    Description = "Correlation Token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
        }
    }
}