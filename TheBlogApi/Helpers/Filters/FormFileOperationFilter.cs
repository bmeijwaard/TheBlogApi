using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TheBlogApi.Helpers.Filters
{
    public class FormFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) return;

            var fileParamNames = context.ApiDescription.ActionDescriptor.Parameters
                .SelectMany(param => param.ParameterType.GetProperties())
                .Where(prop => prop.PropertyType.IsAssignableFrom(typeof(IFormFile)))
                .Select(prop => prop.Name)
                .ToList();

            if (!fileParamNames.Any()) return;

            var paramsToRemove = new List<IParameter>();
            var iamgeFileParam = operation.Parameters.FirstOrDefault(param => param.Name == "imageFile");
            if (iamgeFileParam != null)
            {
                paramsToRemove.Add(iamgeFileParam);
            }

            foreach (var param in operation.Parameters)
            {
                paramsToRemove.AddRange(from fileParamName in fileParamNames where param.Name.StartsWith(fileParamName + ".") select param);
            }
            paramsToRemove.ForEach(x => operation.Parameters.Remove(x));
            ;
            foreach (var paramName in fileParamNames)
            {
                var fileParam = new NonBodyParameter
                {
                    Type = "file",
                    Name = paramName,
                    In = "formData"
                };
                operation.Parameters.Add(fileParam);
            }
            foreach (IParameter param in operation.Parameters)
            {
                param.In = "formData";
            }

            operation.Consumes = new List<string>() { "multipart/form-data" };
        }
    }

    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() == "apivaluesuploadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
