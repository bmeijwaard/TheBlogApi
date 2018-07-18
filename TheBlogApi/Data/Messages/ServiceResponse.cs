using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Messages
{
    public interface IServiceResponse
    {
        bool Succeeded { get; }
        string ErrorMessage { get; }
    }

    public class ServiceResponse : IServiceResponse
    {
        public ServiceResponse()
        {
            Succeeded = true;
        }
        public ServiceResponse(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
        }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class EntityResponse<T> : ServiceResponse
    {
        public EntityResponse(string errorMessage) : base(errorMessage)
        {
        }
        public EntityResponse(T entity) : base()
        {
            Entity = entity;
        }

        public T Entity { get; set; }
    }

    public class EntitiesResponse<T> : ServiceResponse
    {
        public EntitiesResponse(IEnumerable<T> entities) : base()
        {
            Entities = entities;
        }
        public IEnumerable<T> Entities { get; set; }
    }
}
