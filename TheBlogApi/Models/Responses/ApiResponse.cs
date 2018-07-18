using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace TheBlogApi.Models.Responses
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ServerTimeUtc = DateTime.UtcNow;
            Succeeded = true;
        }

        public ApiResponse(bool succeeded) : this()
        {
            Succeeded = succeeded;
        }

        public ApiResponse(string errorMessage) : this()
        {
            Succeeded = string.IsNullOrEmpty(errorMessage);
            Errors = new[] { errorMessage };
        }

        public ApiResponse(ModelStateDictionary modelstate) : this()
        {
            var result = new List<string>();
            foreach (var modelState in modelstate.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    if (string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        result.Add(error.Exception.Message);
                    }
                    else
                    {
                        result.Add(error.ErrorMessage);
                    }
                }
            }

            Errors = result.ToArray();
            Succeeded = false;
        }

        public string[] Errors { get; set; }
        public bool Succeeded { get; set; }
        public DateTime ServerTimeUtc { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(T data) : base()
        {
            Data = data;
            Succeeded = true;
        }

        public ApiResponse(IEnumerable<T> data) : base()
        {
            Data = data;
            Succeeded = true;
        }

        public dynamic Data { get; set; }
    }
}
