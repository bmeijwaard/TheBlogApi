using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Models.Requests;
using TheBlogApi.Models.Responses;
using TheBlogApi.Models.Types;

namespace TheBlogApi.Helpers.Adapters
{
    public interface IAuthenticationAdapter
    {
        Task<TokenResult> TokenExchange(TokenRequest request, GrantType grantType);
       // Task<ServiceResponse> Disconnect(DisconnectRequest request);
    }
}
